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
    UniverseController universe;
    private bool bothPlayersAlive;
    bool rumble;

    void Start()
    {
        universe = GameObject.Find("UniverseController").GetComponent<UniverseController>();
        universe.GetCam(this);
        bothPlayersAlive = true;
    }

    public void Death() { bothPlayersAlive = false; }
    public void RespawnedAPlayer() { bothPlayersAlive = true; }

    void Update()
    {
        float distanceBetweenTargets = Vector3.Distance(leftTarget.position, rightTarget.position) - closeness;
        centerPosition = (leftTarget.position + rightTarget.position) / 2;

        if (bothPlayersAlive)
        {
            transform.position = new Vector3(
                centerPosition.x,
                distanceBetweenTargets + 5,
                centerPosition.z
                ) + offset;
        }
    }

    void LateUpdate()
    {
        if (bothPlayersAlive)
        {
            if (rumble)
            {
                centerPosition += new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), Random.Range(-1f, 1f));
            }
            transform.LookAt(centerPosition);
        }
        else { transform.LookAt(Vector3.zero); }
    }

    public void CamShake(float dur)
    {
        rumble = true;
        Invoke("EndRumble", dur);
    }
    void EndRumble()
    {
        rumble = false;
    }


}
