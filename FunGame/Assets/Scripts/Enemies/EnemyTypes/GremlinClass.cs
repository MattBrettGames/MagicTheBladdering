using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class GremlinClass : EnemyBase
{
    public override void actionOne()
    {
        base.actionOne();
        playerCode.TakeDamage(Mathf.RoundToInt(attackPower * atkMult));
    }
}