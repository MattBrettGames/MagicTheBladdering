using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoomSmoke : MonoBehaviour
{
    public int knockbackPower;


    private void OnTriggerEnter(Collider other)
    {
        other.GetComponent<PlayerBase>().Knockback(knockbackPower, transform.position - other.transform.position);
    }

}
