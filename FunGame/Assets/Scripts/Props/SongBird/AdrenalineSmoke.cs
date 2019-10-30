using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdrenalineSmoke : SmokeBase
{
    public float powerMultBonus;
    public float defenseMultBonus;
    public float duration;

    public override void Begin(string tagToGet)
    {
        Invoke("Return", duration);
        tag = tagToGet;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == gameObject.tag)
        {
            other.GetComponent<PlayerBase>().damageMult += powerMultBonus*0.01f;
            other.GetComponent<PlayerBase>().incomingMult -= defenseMultBonus*0.01f;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == gameObject.tag)
        {
            other.GetComponent<PlayerBase>().damageMult -= powerMultBonus*0.01f;
            other.GetComponent<PlayerBase>().incomingMult += defenseMultBonus*0.01f;
        }
    }

    private void Return()
    {
        GameObject.Find("ObjectPooler").GetComponent<ObjectPooler>().ReturnToAdrenalineSmokePool(gameObject);
    }
}