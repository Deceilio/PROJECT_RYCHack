using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEngine;

class RootMotionCreatorUniversal : RootMotionCreator
{
    public Vector3 AllowedOffsets = new Vector3(1, 0, 1);
    public bool ConstantSpeed = false;
    public bool AllowReverseMotion = false;
    public bool StraightMotion = false;
    public float StepHeight = 0.025f; //percent from full bosy height. Y values above this level are jumps and do not affect root motion
    public string FootNameRegex = "Foot|Toes|Ankle|Leg";
    public int MinDepthFootBone = 3;

    List<string> pathes = new List<string>();
    List<string> footPathes = new List<string>();
    Dictionary<string, Transform> pathToTransform = new Dictionary<string, Transform>();
    GameObject model;
    Bounds modelBounds;
    string rootNodeNameForLegacyAnimation;

    public override void Build(AnimationClip clip)
    {
        this.clip = clip;
        bindings = AnimationUtility.GetCurveBindings(clip);
        timeStep = 1f / clip.frameRate;

        //crate clone of model
        model = GameObject.Instantiate(ReferencedModel);
        model.transform.rotation = Quaternion.identity;
        model.transform.position = Vector3.zero;
        model.transform.localScale = Vector3.one;

        //grab transforms
        GrabModelTransforms();

        //restore curves if original presented
        RestoreOriginalCurves();
        CreateRootCurvesIfNotPresented();

        //save original curves
        SaveOriginalRootCurvesIfPresented(clip);

        //clear root motion by XZ
        ClearRootMotionCurves();

        //prepare list of foot pathes
        FindFootPathes();

        //foreach (var b in bindings)
        //    Debug.Log(b.path);

        AnimationMode.StartAnimationMode();
        try
        {
            //calc preferable direction and min/max Y of foots
            var preferableDirection = Vector3.zero;
            var minMaxFootY = Vector2.zero;
            CreateRootMotion(clip, false, ref preferableDirection, ref minMaxFootY);

            //Debug.Log("preferableDirection: " + preferableDirection);

            //limit max foot Y
            minMaxFootY.y = minMaxFootY.x + modelBounds.size.y * StepHeight;

            //adjust preferable direction
            if (AllowReverseMotion && !StraightMotion)
                preferableDirection = Vector3.zero;

            //final motion
            CreateRootMotion(clip, true, ref preferableDirection, ref minMaxFootY);

            if (Mathf.Abs(AddTurn) > 1)
                CreateArc(clip);
        }
        finally
        {
            if (model != null)
                GameObject.DestroyImmediate(model);
            AnimationMode.StopAnimationMode();
        }
    }

    #region Arc motion
    private void CreateArc(AnimationClip clip)
    {
        var legacyBinding = new EditorCurveBinding { path = rootNodeNameForLegacyAnimation, propertyName = "m_LocalRotation", type = typeof(Transform) };

        var frames = clip.legacy ?
            GetQuaternionList(legacyBinding) :
            GetQuaternionList("RootQ"); 

        if (frames.Count < 2)
            return;

        var list = new List<Quaternion>();
        for (int iFrame = 0; iFrame < frames.Count; iFrame++)
        {
            Quaternion q = GetRotation(frames.Count, iFrame);
            list.Add(frames[iFrame] * q);
        }

        if (clip.legacy)
        {
            var b = legacyBinding;  b.propertyName += ".x";
            SetCurve(b, list.Select(q => q.x).ToList());
            b = legacyBinding; b.propertyName += ".y";
            SetCurve(b, list.Select(q => q.y).ToList());
            b = legacyBinding; b.propertyName += ".z";
            SetCurve(b, list.Select(q => q.z).ToList());
            b = legacyBinding; b.propertyName += ".w";
            SetCurve(b, list.Select(q => q.w).ToList());
        }
        else
        {
            SetCurve("RootQ.x", list.Select(q => q.x).ToList());
            SetCurve("RootQ.y", list.Select(q => q.y).ToList());
            SetCurve("RootQ.z", list.Select(q => q.z).ToList());
            SetCurve("RootQ.w", list.Select(q => q.w).ToList());
        }
    }

    private Quaternion GetRotation(int framesCount, int iFrame)
    {
        var degree = AddTurn * iFrame / (framesCount - 1);
        var q = Quaternion.Euler(0, degree, 0);
        return q;
    }
    #endregion

    #region Root motion
    private void CreateRootMotion(AnimationClip clip, bool isFinalPass, ref Vector3 preferableDirection, ref Vector2 minMaxFootY)
    {
        var prevPos = new Dictionary<string, Vector3>();
        Vector3 undefinedOffset = new Vector3(float.MaxValue, float.MaxValue, float.MaxValue);
        var offset = undefinedOffset;
        var minmaxY = new Vector2(float.MaxValue, float.MinValue);

        //init transforms
        UpdateModelTransforms(clip.length - timeStep);

        //play animation, remember offsets of foots (most bottom bone)
        var offsets = new List<Vector3>();
        for (float time = 0f; time <= clip.length + timeStep / 4f; time += timeStep)
        {
            //if (time > 0.2f)
            //break;

            //remember prev positions
            foreach (var path in footPathes)
                prevPos[path] = pathToTransform[path].position;

            //calc new postions
            UpdateModelTransforms(time);

            //find path with minimal Y
            var bestPath = "";
            var bestY = float.MaxValue;

            foreach (var path in footPathes)
            if (pathToTransform[path].position.y < bestY)
            {
                bestY = pathToTransform[path].position.y;
                bestPath = path;
            }

            if (bestPath != "")
            {
                //Debug.Log(bestPath + " y: " + pathToTransform[bestPath].position.y);

                var newOffset = pathToTransform[bestPath].position - prevPos[bestPath];
                //calc min max Y of foots
                var footY = pathToTransform[bestPath].position.y;
                if (footY < minmaxY.x) minmaxY.x = footY;
                if (footY > minmaxY.y) minmaxY.y = footY;
                //check diapason of Y (if provided)
                if (minMaxFootY == Vector2.zero || (footY >= minMaxFootY.x && footY <= minMaxFootY.y))
                //check preferable direction of moving (if provided)
                if (preferableDirection == Vector3.zero || Vector3.Dot(preferableDirection, newOffset) <= 0)
                    offset = newOffset;//assign new offset
            }

            offsets.Add(offset);
        }

        //fill first undefined offsets
        offset = offsets[offsets.Count - 1];
        for (int i = 0; i < offsets.Count; i++)
        {
            if (offsets[i] == undefinedOffset)
                offsets[i] = offset;
            else
                break;
        }

        //calc average offset
        Vector3 avgOffset;
        avgOffset = offsets.Aggregate((o1, o2) => o1 + o2);
        avgOffset /= offsets.Count;

        //calc motion position
        var rootPosition = Vector3.zero;
        var rootPositions = new List<Vector3>();
        foreach (var o in offsets)
        {
            var off = o;
            if (ConstantSpeed)
                off = avgOffset;
            
            if (off != undefinedOffset)
            {
                if (StraightMotion && preferableDirection != Vector3.zero)
                {
                    //rotate offset in preferable direction
                    var len = off.magnitude;
                    off = Vector3.Project(off, preferableDirection);
                    off = off.normalized * len;
                }

                off.Scale(AllowedOffsets);

                if (off.magnitude < MinSpeed / clip.frameRate)
                    off = off.normalized * MinSpeed / clip.frameRate;
                if (off.magnitude > MaxSpeed / clip.frameRate)
                    off = off.normalized * MaxSpeed / clip.frameRate;

                rootPosition += -off;
            }
            rootPositions.Add(rootPosition);
        }

        //
        preferableDirection = (rootPositions[rootPositions.Count - 1] - rootPositions[0]).normalized;
        minMaxFootY = minmaxY;

        if (isFinalPass)
        {
            FlushCurvesToAnimation(rootPositions);
        }
    }


    private void FindFootPathes()
    {
        footPathes.Clear();

        //try get foot info from HUMANOID animation
        var animator = model.GetComponent<Animator>();
        if (clip.humanMotion && animator != null && animator.avatar != null)
        {
            var footTransforms = new Transform[]{
            animator.GetBoneTransform(HumanBodyBones.LeftFoot),
            animator.GetBoneTransform(HumanBodyBones.RightFoot) };

            var allowedTransforms = new HashSet<Transform>();
            foreach (var footTr in footTransforms)
            {
                allowedTransforms.Add(footTr);
                foreach (var tr in footTr.GetComponentsInChildren<Transform>())
                    allowedTransforms.Add(tr);
            }

            foreach (var path in pathes)
                if (allowedTransforms.Contains(pathToTransform[path]))
                    footPathes.Add(path);

            return;
        }

        //find transforms in GENERIC and LEGACY
        var allowedNameRegex = new Regex(FootNameRegex, RegexOptions.IgnoreCase);
        foreach (var path in pathes)
        if (path.Split('/').Length > MinDepthFootBone)
        if (allowedNameRegex.IsMatch(path))
            footPathes.Add(path);
    }

    private void FlushCurvesToAnimation(List<Vector3> rootPositions)
    {
        if (!clip.legacy)
        {
            SetCurve("RootT.z", rootPositions.Select(p => p.z).ToList());
            SetCurve("RootT.x", rootPositions.Select(p => p.x).ToList());
            if (AllowedOffsets.y != 0)
                SetCurve("RootT.y", rootPositions.Select(p => p.y).ToList());
        }else
        {
            //legacy
            var root = rootNodeNameForLegacyAnimation;
            SetCurve(new EditorCurveBinding {path = root, propertyName = "m_LocalPosition.z", type = typeof(Transform) }, rootPositions.Select(p => p.z).ToList());
            SetCurve(new EditorCurveBinding { path = root, propertyName = "m_LocalPosition.x", type = typeof(Transform) }, rootPositions.Select(p => p.x).ToList());
            if (AllowedOffsets.y != 0)
                SetCurve(new EditorCurveBinding { path = root, propertyName = "m_LocalPosition.y", type = typeof(Transform) }, rootPositions.Select(p => p.y).ToList());
        }
    }

    private void UpdateModelTransforms(float time)
    {
        if (clip.legacy)
        {
            //update Legacy animation
            foreach (var path in pathes)
                UpdateModelTransforms(time, path, pathToTransform[path]);
        }
        else
        {
            AnimationMode.BeginSampling();
            AnimationMode.SampleAnimationClip(model, clip, time);
            AnimationMode.EndSampling();
        }
    }

    private void UpdateModelTransforms(float time, string path, Transform transform)
    {
        var curves = GetVector3Curves(new EditorCurveBinding { path = path, propertyName = "m_LocalPosition", type = typeof(Transform) });
        if (curves[0] != null)
            transform.localPosition = new Vector3(curves[0].Evaluate(time), curves[1].Evaluate(time), curves[2].Evaluate(time));

        curves = GetQuaternionCurves(new EditorCurveBinding { path = path, propertyName = "m_LocalRotation", type = typeof(Transform) });
        if (curves[0] != null)
            transform.localRotation = new Quaternion(curves[0].Evaluate(time), curves[1].Evaluate(time), curves[2].Evaluate(time), curves[3].Evaluate(time));

        curves = GetVector3Curves(new EditorCurveBinding { path = path, propertyName = "m_LocalScale", type = typeof(Transform) });
        if (curves[0] != null)
            transform.localScale = new Vector3(curves[0].Evaluate(time), curves[1].Evaluate(time), curves[2].Evaluate(time));
    }
    #endregion

    #region Grab model
    private void GrabModelTransforms()
    {
        pathToTransform.Clear();
        pathes.Clear();

        foreach (Transform child in model.transform)
        {
            AddPath(child.name, child);
        }

        if (clip.legacy)
        {
            rootNodeNameForLegacyAnimation = model.transform.Cast<Transform>().FirstOrDefault(t => t.GetComponentsInChildren<Transform>().Count() > 5)?.name;
            if (rootNodeNameForLegacyAnimation == null)
                throw new Exception("Can not find root transform in model");
        }

        //calc bounds of model
        modelBounds = new Bounds(pathToTransform.First().Value.position, Vector3.zero);
        foreach (var tr in pathToTransform.Values)
            modelBounds.Encapsulate(tr.position);
    }

    private void AddPath(string path, Transform tr)
    {
        pathes.Add(path);
        pathToTransform[path] = tr;

        foreach (Transform child in tr)
            AddPath(path + "/" + child.name, child);
    }

    private (string, string) GetNameAndParentPath(string fullPath)
    {
        var i = fullPath.LastIndexOf('/');
        if (i < 0)
            return (fullPath, "");
        var name = fullPath.Substring(i + 1, fullPath.Length - i - 1);
        var path = fullPath.Substring(0, i);
        return (name, path);
    }
    #endregion

    #region Save/restore/create root curves
    private void SaveOriginalRootCurvesIfPresented(AnimationClip clip)
    {
        if (clip.legacy)
        {
            SaveOriginalCurve(rootNodeNameForLegacyAnimation, "m_LocalPosition.x");
            SaveOriginalCurve(rootNodeNameForLegacyAnimation, "m_LocalPosition.y");
            SaveOriginalCurve(rootNodeNameForLegacyAnimation, "m_LocalPosition.z");
            SaveOriginalCurve(rootNodeNameForLegacyAnimation, "m_LocalRotation.x");
            SaveOriginalCurve(rootNodeNameForLegacyAnimation, "m_LocalRotation.y");
            SaveOriginalCurve(rootNodeNameForLegacyAnimation, "m_LocalRotation.z");
            SaveOriginalCurve(rootNodeNameForLegacyAnimation, "m_LocalRotation.w");
            bindings = AnimationUtility.GetCurveBindings(clip);
        }
        else
        {
            SaveOriginalCurve("RootT.x");
            SaveOriginalCurve("RootT.y");
            SaveOriginalCurve("RootT.z");
            SaveOriginalCurve("RootQ.x");
            SaveOriginalCurve("RootQ.y");
            SaveOriginalCurve("RootQ.z");
            SaveOriginalCurve("RootQ.w");
            bindings = AnimationUtility.GetCurveBindings(clip);
        }
    }

    private void ClearRootMotionCurves()
    {
        if (!clip.legacy)
        {
            SetCurve("RootT.x", default(AnimationCurve));
            SetCurve("RootT.z", default(AnimationCurve));
            if (AllowedOffsets.y > 0)
                SetCurve("RootT.y", default(AnimationCurve));
        }else
        {
            SetCurve(new EditorCurveBinding() { path = rootNodeNameForLegacyAnimation, propertyName = "m_LocalPosition.x", type = typeof(Transform) }, default(AnimationCurve));
            SetCurve(new EditorCurveBinding() { path = rootNodeNameForLegacyAnimation, propertyName = "m_LocalPosition.z", type = typeof(Transform) }, default(AnimationCurve));
            if (AllowedOffsets.y > 0)
                SetCurve(new EditorCurveBinding() { path = rootNodeNameForLegacyAnimation, propertyName = "m_LocalPosition.y", type = typeof(Transform) }, default(AnimationCurve));
        }
    }

    public override void RestoreOriginalCurves()
    {
        bindings = AnimationUtility.GetCurveBindings(clip);

        if (clip.legacy)
        {
            RestoreOriginalCurveIfPresented(rootNodeNameForLegacyAnimation, "m_LocalPosition.x");
            RestoreOriginalCurveIfPresented(rootNodeNameForLegacyAnimation, "m_LocalPosition.y");
            RestoreOriginalCurveIfPresented(rootNodeNameForLegacyAnimation, "m_LocalPosition.z");
            RestoreOriginalCurveIfPresented(rootNodeNameForLegacyAnimation, "m_LocalRotation.x");
            RestoreOriginalCurveIfPresented(rootNodeNameForLegacyAnimation, "m_LocalRotation.y");
            RestoreOriginalCurveIfPresented(rootNodeNameForLegacyAnimation, "m_LocalRotation.z");
            RestoreOriginalCurveIfPresented(rootNodeNameForLegacyAnimation, "m_LocalRotation.w");
        }
        else
        if (clip.humanMotion)
        {
            RestoreOriginalCurveIfPresented("RootT.x");
            RestoreOriginalCurveIfPresented("RootT.y");
            RestoreOriginalCurveIfPresented("RootT.z");
            RestoreOriginalCurveIfPresented("RootQ.x");
            RestoreOriginalCurveIfPresented("RootQ.y");
            RestoreOriginalCurveIfPresented("RootQ.z");
            RestoreOriginalCurveIfPresented("RootQ.w");
        }
        else
        {   
            //clear Root curves for Generic animation
            SetCurve("RootT.x", default(AnimationCurve));
            SetCurve("RootT.y", default(AnimationCurve));
            SetCurve("RootT.z", default(AnimationCurve));
            SetCurve("RootQ.x", default(AnimationCurve));
            SetCurve("RootQ.y", default(AnimationCurve));
            SetCurve("RootQ.z", default(AnimationCurve));
            SetCurve("RootQ.w", default(AnimationCurve));
        }

        bindings = AnimationUtility.GetCurveBindings(clip);
    }

    private void CreateRootCurvesIfNotPresented()
    {
        if (clip.legacy)
            return;

        var props = new string[] { "RootT.x", "RootT.y", "RootT.z", "RootQ.x", "RootQ.y", "RootQ.z", "RootQ.w" };

        foreach (var prop in props)
        if (!bindings.Any(b => b.propertyName == prop))
        {
            var curve = new AnimationCurve();
            if (prop == "RootQ.w")
                curve.AddKey(0, 1);

            SetCurve(prop, curve);
        }
        bindings = AnimationUtility.GetCurveBindings(clip);
    }

    private void RestoreOriginalCurveIfPresented(string fullPropertyName)
    {
        var curve = GetCurve("original_" + fullPropertyName);
        if (curve != null)
            SetCurve(fullPropertyName, curve);
    }

    private void RestoreOriginalCurveIfPresented(string path, string propertyName)
    {
        var binding = new EditorCurveBinding
        {
            path = "original_" + path,
            propertyName = propertyName,
            type = typeof(Transform)
        };

        var curve = GetCurve(binding);
        if (curve != null)
        {
            binding.path = path;
            SetCurve(binding, curve);
        }
    }

    private void SaveOriginalCurve(string fullPropertyName)
    {
        var curve = GetCurve(fullPropertyName);
        if (curve != null)
            SetCurve("original_" + fullPropertyName, curve);
    }

    private void SaveOriginalCurve(string path, string propertyName)
    {
        var binding = new EditorCurveBinding
        {
            path = path,
            propertyName = propertyName,
            type = typeof(Transform)
        };
        var curve = GetCurve(binding);
        if (curve != null)
        {
            binding.path = "original_" + path;
            SetCurve(binding, curve);
        }
    }
    #endregion
}
