using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WRLD_BILLBOARD : MonoBehaviour
{
    public Camera cam;

    void Update()
    {
        if (cam == null)
            return;

        transform.LookAt(cam.transform);
        transform.Rotate(Vector3.up * 180);
    }
}
