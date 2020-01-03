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
    int deadPlayer = 2;

    void Start()
    {
        universe = GameObject.Find("UniverseController").GetComponent<UniverseController>();
        universe.GetCam(this);
        bothPlayersAlive = true;
    }

    public void Death(int charDeath) { bothPlayersAlive = false; deadPlayer = charDeath; }
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
        else
        {
            if (deadPlayer == 0)
            {
                transform.position = Vector3.Lerp(transform.position, new Vector3(leftTarget.transform.position.x, leftTarget.transform.position.y + 5, leftTarget.transform.position.z) + offset, 0.3f * Time.deltaTime);
            }
            else
            {
                transform.position = Vector3.Lerp(transform.position, new Vector3(rightTarget.transform.position.x, rightTarget.transform.position.y + 5, rightTarget.transform.position.z) + offset, 0.3f * Time.deltaTime);
            }
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
        else
        {
            if (deadPlayer == 0)
            {
                transform.LookAt(leftTarget);
            }
            else
            {
                transform.LookAt(rightTarget);
            }
        }
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
