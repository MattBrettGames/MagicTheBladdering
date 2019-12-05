using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DualObjectiveCamera : MonoBehaviour
{
    public Transform leftTarget;
    public Transform rightTarget;
    public Vector3 offset;
    Vector3 centerPosition;
    [SerializeField] float closeness;
    [SerializeField] float minHeight = 0;
    [SerializeField] float maxHeight = 50;

    void Update()
    {
        float distanceBetweenTargets = Vector3.Distance(leftTarget.position, rightTarget.position)-closeness;
        centerPosition = (leftTarget.position + rightTarget.position) / 2;

        transform.position = new Vector3(
            centerPosition.x,
            Mathf.Abs(Mathf.Clamp(distanceBetweenTargets, minHeight, maxHeight)),
            centerPosition.z
            )+offset;
    }

    void LateUpdate()
    {
        transform.LookAt(centerPosition);
        //transform.position = new Vector3(transform.position.x, offset.y, transform.position.z);
    }

}
