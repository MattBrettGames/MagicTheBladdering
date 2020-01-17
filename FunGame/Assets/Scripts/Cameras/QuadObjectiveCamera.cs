using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuadObjectiveCamera : TriObjectiveCamera
{

    [SerializeField] Transform fourthTarget;
    float[] pos = new float[4];

    public override void Start() 
    {
    
    }

    public override void Update()
    {
        pos[0] = firstTarget.position.x;
        pos[1] = secondTarget.position.x;
        pos[2] = thirdTarget.position.x;
        pos[3] = fourthTarget.position.x;

        centerPosition = (firstTarget.position + secondTarget.position + thirdTarget.position + fourthTarget.position) / 4;

        transform.position = new Vector3(
            centerPosition.x + Mathf.Min(pos),
            distanceBetweenTargets + 35,
            centerPosition.z
            ) + offset;
    }

    public override void LateUpdate() { transform.LookAt(centerPosition); }

}
