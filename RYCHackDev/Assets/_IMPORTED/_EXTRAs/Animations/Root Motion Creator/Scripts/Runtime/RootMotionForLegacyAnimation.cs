using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RootMotionForLegacyAnimation : MonoBehaviour
{
    public Transform RootNode;
    Vector3 originalPos;
    Vector3 prevPos;
    Quaternion originalRot;
    Quaternion prevRot;
    bool isOut;

    void Awake()
    {
        prevPos = originalPos = RootNode.localPosition;
        prevRot = originalRot = RootNode.localRotation;
    }

    private void LateUpdate()
    {
        if ((RootNode.localPosition - originalPos).sqrMagnitude < 0.001f)
        {
            if (isOut)
            {
                transform.position = prevPos;
                transform.rotation = prevRot;
            }
        }
        else
        {
            isOut = true;
        }

        prevPos = transform.TransformPoint(RootNode.localPosition);
        prevRot = RootNode.rotation;
    }
}
