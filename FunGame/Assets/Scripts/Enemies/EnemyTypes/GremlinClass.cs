using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GremlinClass : EnemyBase
{
    public override void Update()
    {
        if (aggro)
        {
            if (Vector3.Distance(transform.position, targetPlayer.position) < attackRange && !attackOnCooldown) { actionOne(); }
            agent.SetDestination(targetPlayer.position + distanceGoal);
        }
    }

    public override void actionOne()
    {
        base.actionOne();
        playerCode.TakeDamage(Mathf.RoundToInt(attackPower * atkMult));
    }
}