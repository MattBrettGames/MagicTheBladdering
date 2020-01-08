using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CorvidDagger : Weapons
{
    [HideInInspector]public bool poisonActive;

    public override void OnTriggerEnter(Collider other)
    {
        base.OnTriggerEnter(other);
    }
}