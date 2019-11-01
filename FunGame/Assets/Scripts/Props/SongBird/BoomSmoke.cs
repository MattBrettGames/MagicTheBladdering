using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoomSmoke : SmokeBase
{
    public int knockbackPower;
    public float duration;

    public override void Begin(string tagToGet)
    {
        Invoke("Return", duration);
        tag = tagToGet;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag != gameObject.tag)
        {
            other.GetComponent<PlayerBase>().Knockback(knockbackPower, transform.position - other.transform.position);
        }
    }

    private void Return()
    {
        GameObject.Find("ObjectPooler").GetComponent<ObjectPooler>().ReturnToBoomSmokePool(gameObject);
    }
}