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
    float camShake;
    UniverseController universe;

    public void Start()
    {
        RetryTargets();
        blank = new GameObject("BlankCameraTarget");

        universe = GameObject.Find("UniverseController").GetComponent<UniverseController>();
        universe.GetCam(null, this);
    }

    public void Update()
    {
        transform.position = Vector3.Lerp(transform.position, boundBox.center + new Vector3(offset.x, Mathf.Max(boundBox.size.x, boundBox.size.z) + offset.y, offset.z), 0.6f);

        //transform.position = Vector3.SmoothDamp(transform.position, new Vector3(offset.x, Mathf.Max(boundBox.size.x, boundBox.size.z) + offset.y, offset.z), ref velocity, Time.deltaTime);

        blank.transform.LookAt(boundBox.center + lookatOffset + (new Vector3(Random.Range(-1f, 1f), 0, Random.Range(-1f, 1f)) * camShake));
        blank.transform.position = transform.position;

        transform.forward = Vector3.Lerp(transform.forward, blank.transform.forward, Time.deltaTime);

        RetryTargets();
    }

    public void AddTarget(int targetNum)
    {
        targets.Add(GameObject.Find("Player" + targetNum + "Base").transform);
        RetryTargets();
    }

    public void RemoveTarget(Transform target)
    {
        targets.Remove(target);
        RetryTargets();
    }

    public void CamShake(float degree)
    {
        camShake = degree;
        StartCoroutine(StopShake());
    }

    IEnumerator StopShake()
    {
        yield return new WaitForSeconds(0.2f);
        camShake = 0;
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