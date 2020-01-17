using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriObjectiveCamera : DualObjectiveCamera
{

    [SerializeField] public Transform thirdTarget;
    protected Vector3 firstPos;
    protected float secondDistance;

    public override void Start() { }

    public override void Update()
    {

        distanceBetweenTargets = (firstTarget.position.x + secondTarget.position.x + thirdTarget.position.x) - (firstTarget.position.y + secondTarget.position.y + thirdTarget.position.y);

        centerPosition = (firstTarget.position + secondTarget.position + thirdTarget.position) / 3;

        transform.position = new Vector3(
            centerPosition.x,
            distanceBetweenTargets + 5,
            centerPosition.z
            ) + offset;

    }

    public override void LateUpdate() { }

}
