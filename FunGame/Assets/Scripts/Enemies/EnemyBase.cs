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
    [HideInInspector] public int maxHealth;
    public float speed;
    public int attackPower;

    [Header("Affectors")]
    public float atkMult;
    public float defMult;

    [Header("AI Controls")]
    public float safeDistance;
    public float aggroRange;
    public Transform targetPlayer;
    public bool aggro;
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

    public void SetStats(ScoreTracker scoreTracker)
    {
        maxHealth = health;
        tracker = scoreTracker;
        agent = GetComponent<NavMeshAgent>();
        agent.speed = speed;
        distanceGoal = new Vector3(UnityEngine.Random.Range(-safeDistance, safeDistance), 0, UnityEngine.Random.Range(-safeDistance, safeDistance));
        homeSpot = transform.position;

        targetPlayer = GameObject.FindGameObjectWithTag("Player" + UnityEngine.Random.Range(1, 3)).transform;
        playerCode = targetPlayer.gameObject.GetComponent<PlayerBase>();

        ReTarget();

    }

    public virtual void Update()
    {
        //print(targetPlayer);
        if (Vector3.Distance(transform.position, targetPlayer.position) <= aggroRange)
        {
            aggro = true;
        }
        else { aggro = false; }

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

    public virtual void ReTarget()
    {
        targetPlayer = GameObject.FindGameObjectWithTag("Player" + UnityEngine.Random.Range(1, 3)).transform;
        playerCode = targetPlayer.gameObject.GetComponent<PlayerBase>();
        if (playerCode.currentHealth <= 0)
        {
            ReTarget();
        }
    }
}