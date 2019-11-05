using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossBase : EnemyBase
{






    public void BeginActive(Transform target)
    {
        gameObject.SetActive(true);
        aggroRange = 100;
        targetPlayer = target;
    }

    public override void TakeDamage(int damage, string player)
    {
        base.TakeDamage(damage, player);
        UniverseController universe = GameObject.Find("UniverseController").GetComponent<UniverseController>();
        universe.Invoke("BossDeath()", 5);
    }

}
