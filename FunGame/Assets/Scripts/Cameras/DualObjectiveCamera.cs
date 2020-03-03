using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DualObjectiveCamera : MonoBehaviour
{

    public Vector3 offset;
    protected Vector3 centerPosition;
    [SerializeField] protected float closeness;
    protected UniverseController universe;
    protected bool bothPlayersAlive;
    protected bool rumble;
    [HideInInspector] public float distanceBetweenTargets; 
    int deadPlayer = 2;

    [Header("Targets")]
    public Transform firstTarget;
    public Transform secondTarget;

    public virtual void Start()
    {
        universe = GameObject.Find("UniverseController").GetComponent<UniverseController>();
        universe.GetCam(this, null);
        bothPlayersAlive = true;
    }

    public void Death(int charDeath) { bothPlayersAlive = false; deadPlayer = charDeath; }
    public void RespawnedAPlayer() { bothPlayersAlive = true; }

    public virtual void Update()
    {
        float distanceBetweenTargets = Vector3.Distance(firstTarget.position, secondTarget.position) - closeness;
        centerPosition = (firstTarget.position + secondTarget.position) / 2;

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
                transform.position = Vector3.Lerp(transform.position, new Vector3(firstTarget.transform.position.x, firstTarget.transform.position.y + 5, firstTarget.transform.position.z) + offset, 0.3f);
            }
            else
            {
                transform.position = Vector3.Lerp(transform.position, new Vector3(secondTarget.transform.position.x, secondTarget.transform.position.y + 5, secondTarget.transform.position.z) + offset, 0.3f);
            }
        }
    }

    public virtual void LateUpdate()
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
                transform.LookAt(firstTarget);
            }
            else
            {
                transform.LookAt(secondTarget);
            }
        }
    }

    public void CamShake(float dur)
    {
        rumble = true;
        StartCoroutine(EndRumble(dur));
    }
    IEnumerator EndRumble(float dur)
    {
        yield return new WaitForSecondsRealtime(dur);
        rumble = false;
    }


}
