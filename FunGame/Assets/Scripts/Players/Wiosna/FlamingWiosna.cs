using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlamingWiosna : MonoBehaviour
{
    private Transform target;
    [SerializeField] float speed;

    void Update()
    {
        transform.position = Vector3.Lerp(transform.position, target.transform.position, speed);
    }

    public void SetInfo(Transform targetTemp) { target = targetTemp; }
}
