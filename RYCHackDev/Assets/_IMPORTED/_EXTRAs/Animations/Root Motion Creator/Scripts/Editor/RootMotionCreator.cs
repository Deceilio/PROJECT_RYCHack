using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

abstract class RootMotionCreator
{
    public AnimationClip clip;
    public float MinSpeed = 0f;//m/s
    public float MaxSpeed = 20f;//m/s
    public int AddTurn = 0;//degrees
    public GameObject ReferencedModel;

    protected float timeStep = 0.01f;
    protected EditorCurveBinding[] bindings;

    public abstract void Build(AnimationClip clip);
    public abstract void RestoreOriginalCurves();

    #region Utils
    protected void SetCurve(EditorCurveBinding binding, IList<float> curve)
    {
        var animCurve = new AnimationCurve();
        float time = 0f;
        for (int i = 0; i < curve.Count; i++, time += timeStep)
        {
            animCurve.AddKey(time, curve[i]);
        }
        AnimationUtility.SetEditorCurve(clip, binding, animCurve);
    }

    protected void SetCurve(EditorCurveBinding binding, AnimationCurve curve)
    {
        AnimationUtility.SetEditorCurve(clip, binding, curve);
    }

    protected void SetCurve(string propertyName, IList<float> curve)
    {
        var animCurve = new AnimationCurve();
        float time = 0f;
        for (int i = 0; i < curve.Count; i++, time += timeStep)
        {
            animCurve.AddKey(time, curve[i]);
        }

        SetCurve(propertyName, animCurve);
    }

    protected void SetCurve(string propertyName, AnimationCurve curve)
    {
        var binding = bindings.FirstOrDefault(b => b.propertyName == propertyName);
        if (binding.propertyName == null)
            binding = new EditorCurveBinding() { path = "", propertyName = propertyName, type = typeof(Animator) };
        AnimationUtility.SetEditorCurve(clip, binding, curve);
    }

    protected AnimationCurve GetCurve(string propertyName)
    {
        var binding = bindings.FirstOrDefault(b => b.propertyName == propertyName);
        if (binding.propertyName == null)
            return null;
        return AnimationUtility.GetEditorCurve(clip, binding);
    }

    protected AnimationCurve GetCurve(EditorCurveBinding binding)
    {
        //AnimationUtility.
        return AnimationUtility.GetEditorCurve(clip, binding);
    }

    protected AnimationCurve[] GetVector3Curves(string propertyName)
    {
        return new AnimationCurve[] {
            GetCurve(propertyName + ".x"),
            GetCurve(propertyName + ".y"),
            GetCurve(propertyName + ".z")};
    }

    protected AnimationCurve[] GetVector3Curves(EditorCurveBinding binding)
    {
        var x = new EditorCurveBinding { path = binding.path, propertyName = binding.propertyName + ".x", type = binding.type };
        var y = new EditorCurveBinding { path = binding.path, propertyName = binding.propertyName + ".y", type = binding.type };
        var z = new EditorCurveBinding { path = binding.path, propertyName = binding.propertyName + ".z", type = binding.type };
        return new AnimationCurve[] {
            GetCurve(x),
            GetCurve(y),
            GetCurve(z)};
    }

    protected AnimationCurve[] GetQuaternionCurves(EditorCurveBinding binding)
    {
        var x = new EditorCurveBinding { path = binding.path, propertyName = binding.propertyName + ".x", type = binding.type };
        var y = new EditorCurveBinding { path = binding.path, propertyName = binding.propertyName + ".y", type = binding.type };
        var z = new EditorCurveBinding { path = binding.path, propertyName = binding.propertyName + ".z", type = binding.type };
        var w = new EditorCurveBinding { path = binding.path, propertyName = binding.propertyName + ".w", type = binding.type };
        return new AnimationCurve[] {
            GetCurve(x),
            GetCurve(y),
            GetCurve(z),
            GetCurve(w)};
    }

    protected AnimationCurve[] GetQuaternionCurves(string propertyName)
    {
        return new AnimationCurve[] {
            GetCurve(propertyName + ".x"),
            GetCurve(propertyName + ".y"),
            GetCurve(propertyName + ".z"),
            GetCurve(propertyName + ".w")};
    }

    protected List<Vector3> GetVector3List(string propertyName)
    {
        var curves = GetVector3Curves(propertyName);
        var list = new List<Vector3>();
        for (float time = 0f; time <= clip.length; time += timeStep)
        {
            list.Add(new Vector3(curves[0].Evaluate(time), curves[1].Evaluate(time), curves[2].Evaluate(time)));
        }

        return list;
    }

    protected List<Vector3> GetVector3List(EditorCurveBinding binding)
    {
        var curves = GetVector3Curves(binding);
        var list = new List<Vector3>();
        for (float time = 0f; time <= clip.length; time += timeStep)
        {
            list.Add(new Vector3(curves[0].Evaluate(time), curves[1].Evaluate(time), curves[2].Evaluate(time)));
        }

        return list;
    }

    protected List<Quaternion> GetQuaternionList(string propertyName)
    {
        var curves = GetQuaternionCurves(propertyName);

        var list = new List<Quaternion>();
        for (float time = 0f; time <= clip.length; time += timeStep)
        {
            list.Add(new Quaternion(curves[0].Evaluate(time), curves[1].Evaluate(time), curves[2].Evaluate(time), curves[3].Evaluate(time)));
        }

        return list;
    }

    protected List<Quaternion> GetQuaternionList(EditorCurveBinding binding)
    {
        var curves = GetQuaternionCurves(binding);

        var list = new List<Quaternion>();
        for (float time = 0f; time <= clip.length; time += timeStep)
        {
            list.Add(new Quaternion(curves[0].Evaluate(time), curves[1].Evaluate(time), curves[2].Evaluate(time), curves[3]?.Evaluate(time) ?? 1));
        }

        return list;
    }
    #endregion
}
