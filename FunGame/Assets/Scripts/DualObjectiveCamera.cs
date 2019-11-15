using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DualObjectiveCamera : MonoBehaviour
{
    public Transform leftTarget;
    public Transform rightTarget;
    public Vector3 offset;

    // Update is called once per frame
    void Update()
    {
        float distanceBetweenTargets = Vector3.Distance(leftTarget.position, rightTarget.position);
        Vector3 centerPosition = (leftTarget.position + rightTarget.position) / 2;

        transform.position = new Vector3(
            centerPosition.x,
            distanceBetweenTargets,
            centerPosition.z
            )+offset;
    }
}
