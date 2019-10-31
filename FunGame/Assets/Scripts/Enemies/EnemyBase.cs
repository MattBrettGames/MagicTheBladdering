using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using System;

public abstract class EnemyBase : MonoBehaviour
{
    [Header("RPG Stats")]
    public int health;
    public int maxHealth;
    public float speed;
    public int attackPower;

    [Header("Affectors")]
    public float atkMult;
    public float defMult;

    [Header("AI Controls")]
    public float attackRange;
    public float attackTimer;
    public float safeDistance;
    protected Vector3 distanceGoal;
    protected bool aggro;
    protected Transform targetPlayer;
    protected NavMeshAgent agent;
    protected bool attackOnCooldown;
    protected PlayerBase playerCode;
    protected EnemyDealer dealer;

    public void SetStats(Transform target, EnemyDealer gainDealer)
    {
        maxHealth = health;
        dealer = gainDealer;
        targetPlayer = target;
        agent = GetComponent<NavMeshAgent>();
        playerCode = targetPlayer.gameObject.GetComponent<PlayerBase>();
        agent.speed = speed;
        distanceGoal = new Vector3(UnityEngine.Random.Range(-safeDistance, safeDistance), 0, UnityEngine.Random.Range(-safeDistance, safeDistance));
    }

    public virtual void Update()
    {

        if (aggro)
        {
            if (Vector3.Distance(transform.position, targetPlayer.position) < attackRange && !attackOnCooldown) { actionOne(); }
            agent.SetDestination(targetPlayer.position + distanceGoal);
        }
    }

    public virtual void Beginwave() { aggro = true; }
    public virtual void actionOne() { Invoke("EndCooldown", attackTimer); attackOnCooldown = true; } // playerCode.TakeDamage(Mathf.RoundToInt(attackPower * atkMult)); }
    public virtual void actionTwo() { }
    public virtual void actionThree() { }
    private void EndCooldown() { attackOnCooldown = false; }
    public virtual void TakeDamage(int damage) { health -= Mathf.RoundToInt(damage * defMult); if (health <= 0) { dealer.EnemyDeath(gameObject); } }


}
