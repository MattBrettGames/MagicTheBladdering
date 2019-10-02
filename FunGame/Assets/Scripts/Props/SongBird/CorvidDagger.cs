using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CorvidDagger : Weapons
{
    public bool poisonActive;

    public override void OnTriggerEnter(Collider other)
    {
        base.OnTriggerEnter(other);
        if (poisonActive) { other.GetComponent<PlayerBase>().poison *= 2; }
    }

}
