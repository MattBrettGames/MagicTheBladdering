using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriObjectiveCamera : MonoBehaviour
{
    [SerializeField]
    public List<Transform> targets;
    private Bounds boundBox;
    public float dampTime;
    GameObject blank;
    Vector3 velocity;

    [Header("Offsets")]
    [SerializeField] Vector3 offset;
    [SerializeField] Vector3 lookatOffset;

    public void Start() { RetryTargets(); blank = new GameObject("BlankCameraTarget"); }

    public void LateUpdate()
    {
        transform.position = boundBox.center + new Vector3(offset.x, Mathf.Max(boundBox.size.x, boundBox.size.z) + offset.y, offset.z);

        blank.transform.LookAt(boundBox.center + lookatOffset);
        blank.transform.position = transform.position;

        transform.forward = Vector3.Lerp(transform.forward, blank.transform.forward, Time.deltaTime);

        RetryTargets();
    }

    public void AddTarget(int targetNum)
    {
        targets.Add(GameObject.Find("Player" + targetNum + "Base").transform);
        RetryTargets();
    }

    public void RemoveTarget(int targetNum)
    {
        targets.RemoveAt(targetNum);
        RetryTargets();
    }

    void RetryTargets()
    {
        boundBox = new Bounds(targets[0].position, Vector3.zero);

        for (int i = 0; i < targets.Count; i++)
        {
            boundBox.Encapsulate(targets[i].position);
        }
    }

}