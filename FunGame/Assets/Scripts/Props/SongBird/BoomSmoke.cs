using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoomSmoke : BlankMono
{
    public int knockbackPower;
    public float duration;

    public void Begin()
    {
        Invoke("Return", duration);
    }

    private void OnTriggerEnter(Collider other)
    {
        other.GetComponent<PlayerBase>().Knockback(knockbackPower, transform.position - other.transform.position);
    }

    private void Return()
    {
        GameObject.Find("ObjectPooler").GetComponent<ObjectPooler>().ReturnToBoomSmokePool(gameObject);
    }

}
