using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DualObjectiveCamera : MonoBehaviour
{
    public Transform leftTarget;
    public Transform rightTarget;

    public float minDistance;

    // Update is called once per frame
    void Update()
    {
        float distanceBetweenTargets = Vector3.Distance(leftTarget.position, rightTarget.position);
        Vector3 centerPosition = (leftTarget.position + rightTarget.position) / 2;

        transform.position = new Vector3(
            centerPosition.x,
            distanceBetweenTargets,
            centerPosition.z + 10
            );

        //        transform.position = new Vector3(transform.position.x, transform.position.y, Mathf.Clamp(this.gameObject.transform.position.z, -15, 1000));

    }
}
