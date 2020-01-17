using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuadObjectiveCamera : TriObjectiveCamera
{

    [SerializeField] Transform fourthTarget;

    public override void Start() { }

    public override void Update()
    {
        distanceBetweenTargets = Vector3.Distance(firstTarget.position, secondTarget.position) - closeness;
        centerPosition = (firstTarget.position + secondTarget.position + thirdTarget.position + fourthTarget.position) / 4;

        transform.position = new Vector3(
            centerPosition.x,
            distanceBetweenTargets + 5,
            centerPosition.z
            ) + offset;
    }


}
