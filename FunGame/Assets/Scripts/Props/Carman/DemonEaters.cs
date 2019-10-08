using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DemonEaters : Weapons
{
    public bool demonSlayer;

    public override void OnTriggerEnter(Collider other)
    {
        base.OnTriggerEnter(other);

        if (demonSlayer) { other.GetComponent<PlayerBase>().cursed = false; }

    }

}
