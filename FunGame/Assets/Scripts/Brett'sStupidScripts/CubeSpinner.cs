using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeSpinner : BlankMono
{
    public Vector3 Angle;
    public float speed;
    void Update()
    {
        gameObject.transform.Rotate(Angle, speed);
    }
}