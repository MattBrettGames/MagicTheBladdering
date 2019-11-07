using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DualObjectiveCamera : MonoBehaviour {
    public Transform leftTarget;
    public Transform rightTarget;

    public float minDistance;
	
	// Update is called once per frame
	void Update () {
        float distanceBetweenTargets = Mathf.Abs(leftTarget.position.x - rightTarget.position.x) * 2;
        float centerPosition = (leftTarget.position.x + rightTarget.position.x) / 2;

        transform.position = new Vector3(
            centerPosition, transform.position.y,
            distanceBetweenTargets > minDistance ? -distanceBetweenTargets : -minDistance
            );
        transform.position = new Vector3(transform.position.x, transform.position.y, Mathf.Clamp(this.gameObject.transform.position.z, -15, 1000));

    }
}
