using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdrenalineSmoke : MonoBehaviour
{
    public float powerMultBonus;
    public float defenseMultBonus;

    private List<GameObject> afflicted = new List<GameObject>();

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == gameObject.tag)
        {
            other.GetComponent<PlayerBase>().damageMult += powerMultBonus/10;
            other.GetComponent<PlayerBase>().incomingMult += defenseMultBonus/10;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == gameObject.tag)
        {
            other.GetComponent<PlayerBase>().damageMult -= powerMultBonus/10;
            other.GetComponent<PlayerBase>().incomingMult -= defenseMultBonus/10;
        }
    }
}