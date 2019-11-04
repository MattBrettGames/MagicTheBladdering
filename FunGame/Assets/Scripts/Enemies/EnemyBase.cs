using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using System;

public abstract class EnemyBase : MonoBehaviour
{
    [SerializeField] int enemyID;

    [Header("RPG Stats")]
    public int health;
    public int maxHealth;
    public float speed;
    public int attackPower;

    [Header("Affectors")]
    public float atkMult;
    public float defMult;

    [Header("AI Controls")]
    public float safeDistance;
    public float aggroRange;
    public Transform targetPlayer;
    protected bool aggro;
    protected Vector3 distanceGoal;
    protected NavMeshAgent agent;
    protected PlayerBase playerCode;
    protected ScoreTracker tracker;
    protected Vector3 homeSpot;

    [Header("AttackStats")]    
    public float attackOneRange;
    public float attackOneTimer;
    protected bool attackOneOnCooldown;
    [Space]
    public float attackTwoRange;
    public float attackTwoTimer;
    protected bool attackTwoOnCooldown;
    [Space]
    public float attackThreeRange;
    public float attackThreeTimer;
    protected bool attackThreeOnCooldown;

    public void SetStats(Transform target, ScoreTracker scoreTracker)
    {
        maxHealth = health;
        tracker = scoreTracker;
        targetPlayer = target;
        agent = GetComponent<NavMeshAgent>();
        playerCode = targetPlayer.gameObject.GetComponent<PlayerBase>();
        agent.speed = speed;
        distanceGoal = new Vector3(UnityEngine.Random.Range(-safeDistance, safeDistance), 0, UnityEngine.Random.Range(-safeDistance, safeDistance));
        homeSpot = transform.position;
    }

    public virtual void Update()
    {        
        if(Vector3.Distance(transform.position, targetPlayer.position) <= aggroRange)
        {
            aggro = true;
        }

        if (aggro)
        {
            if (Vector3.Distance(transform.position, targetPlayer.position) < attackOneRange && !attackOneOnCooldown) { actionOne(); }
            agent.SetDestination(targetPlayer.position + distanceGoal);
        }
        else { agent.SetDestination(homeSpot); }
    }

    public virtual void actionOne() { Invoke("EndOneCooldown", attackOneTimer); attackOneOnCooldown = true; } // playerCode.TakeDamage(Mathf.RoundToInt(attackPower * atkMult)); }
    public virtual void actionTwo() { Invoke("EndTwoCooldown", attackTwoTimer); attackTwoOnCooldown = true; }
    public virtual void actionThree() { Invoke("EndThreeCooldown", attackThreeTimer); attackThreeOnCooldown = true; }
    private void EndOneCooldown() { attackOneOnCooldown = false; }
    private void EndTwoCooldown() { attackTwoOnCooldown = false; }
    private void EndThreeCooldown() { attackThreeOnCooldown = false; }

    public virtual void TakeDamage(int damage, string player) { health -= Mathf.RoundToInt(damage * defMult); if (health <= 0) { tracker.EnemyDeath(player, enemyID); } }


}
