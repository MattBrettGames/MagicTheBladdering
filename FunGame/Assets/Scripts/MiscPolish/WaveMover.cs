using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveMover : MonoBehaviour
{
    float time = 0;
    public float x;
    public float y;
    public float z;
    public float speed;
    public float exageration;

    void Update()
    {
        time += Time.deltaTime*speed;

        float xMove = Mathf.Cos(time);
        float yMove = Mathf.Sin(time);

        transform.localPosition = new Vector3(x+xMove, y+yMove, z)*exageration;

    }
}
