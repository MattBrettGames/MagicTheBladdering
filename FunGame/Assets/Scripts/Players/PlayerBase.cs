using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;

public abstract class PlayerBase : BlankMono
{
    [Header("GameMode Stuff")]
    public string thisPlayer;
    public int playerID;
    [HideInInspector] public int numOfDeaths = 0;
    public bool pvp;

    [Header("Movement Stats")]
    public float speed;
    public float dodgeSpeed;
    public float dodgeDur;
    public float dodgeCooldown;
    protected float dodgeTimer;
    private float baseSpeed;
    protected float moving;
    protected Vector3 dir;

    [Header("Common Stats")]
    public int currentHealth;
    [HideInInspector] public int healthMax;
    public float damageMult = 1;
    public float incomingMult = 1;

    [Header("Status Effects")]
    public bool cursed;
    protected float curseTimer;
    public bool prone;
    public float poison;
    private bool hyperArmour;
    protected bool iFrames;
    protected bool counterFrames;
    protected bool acting;
    protected Vector3 knockbackForce;
    private float knockBackPower;
    [HideInInspector] public State state;
    [HideInInspector]
    public enum State
    {
        normal,
        lockedOn,
        dodging,
        knockback,
        attack
    }

    [Header("Components")]
    public Transform aimTarget;
    public GameObject visuals;
    protected Animator anim;
    protected Rigidbody rb2d;
    protected PlayerController playerCont;
    protected Player player;
    protected Transform lookAtTarget;
    protected Vector3 lookAtVariant = new Vector3(0, -5, 0);
    protected Transform walkDirection;

    public virtual void Start()
    {
        anim = gameObject.GetComponentInChildren<Animator>();
        rb2d = gameObject.GetComponent<Rigidbody>();
        dodgeTimer = dodgeCooldown;
        baseSpeed = speed;

        walkDirection = GameObject.Find("WalkDirector").transform;

        InvokeRepeating("PoisonTick", 0, 0.5f);
        player = ReInput.players.GetPlayer(playerID);
    }
    
    public virtual void SetInfo()
    {
        if (playerID == 0) { lookAtTarget = GameObject.Find("Player2Base").transform; }
        else { lookAtTarget = GameObject.Find("Player1Base").transform; }
    }

    public virtual void Update()
    {
        dir = new Vector3(player.GetAxis("HoriMove"), 0, player.GetAxis("VertMove")).normalized;
        dodgeTimer -= Time.deltaTime;

        if (poison > 0) { poison -= Time.deltaTime; }
        if (curseTimer <= 0) { LoseCurse(); }
        else { curseTimer -= Time.deltaTime; }

        aimTarget.position = transform.position + dir * 5;

        if (anim.GetCurrentAnimatorStateInfo(0).IsName("Idle") || anim.GetCurrentAnimatorStateInfo(0).IsName("Walking")) acting = false;

        switch (state)
        {
            case State.attack:
                break;

            case State.normal:

                anim.SetBool("LockOn", false);
                if (player.GetAxis("LockOn") >= 0.4f) { state = State.lockedOn; }

                if (!prone && !acting)
                {
                    //Rotating the Character Model
                    visuals.transform.LookAt(aimTarget);
                    rb2d.velocity = dir * speed;

                    //Standard Inputs
                    if (player.GetButtonDown("AAction")) { AAction(); }
                    if (player.GetButtonDown("BAttack")) { BAction(); }
                    if (player.GetButtonDown("XAttack")) { XAction(); }
                    if (player.GetButtonDown("YAttack")) { YAction(); }

                    if (player.GetAxis("HoriMove") != 0 || player.GetAxis("VertMove") != 0) { anim.SetFloat("Movement", 1); }
                    else { anim.SetFloat("Movement", 0); }
                }
                else
                {
                    dir = Vector3.zero;
                }
                break;

            case State.lockedOn:

                walkDirection.position = dir + transform.position;

                anim.SetBool("LockOn", true);
                if (player.GetAxis("LockOn") <= 0.4f) { state = State.normal; }

                if (!prone && !acting)
                {
                    rb2d.velocity = dir * speed;

                    if (player.GetButtonDown("AAction")) { AAction(); }
                    if (player.GetButtonDown("BAttack")) { BAction(); }
                    if (player.GetButtonDown("XAttack")) { XAction(); }
                    if (player.GetButtonDown("YAttack")) { YAction(); }

                    if (player.GetAxis("HoriMove") != 0 || player.GetAxis("VertMove") != 0) { anim.SetFloat("Movement", 1); }
                    else { anim.SetFloat("Movement", 0); }
                  
                    anim.SetFloat("Movement_X", -Vector3.SignedAngle(dir.normalized, visuals.transform.forward.normalized, Vector3.up) * 0.09f);
                    anim.SetFloat("Movement_ZY", -Vector3.SignedAngle(dir.normalized, visuals.transform.forward.normalized, Vector3.up) * 0.09f);
                }

                visuals.transform.LookAt(lookAtTarget.position + lookAtVariant);
                break;

            case State.dodging:
                if (dodgeTimer < 0) DodgeSliding(dir);
                break;

            case State.knockback:
                KnockbackContinual();
                break;
        }
    }

    #region Input Actions
    public virtual void AAction() { }
    public virtual void BAction() { }
    public virtual void XAction() { }
    public virtual void YAction() { }
    #endregion

    #region Common Events
    public virtual void TakeDamage(int damageInc) { if (!counterFrames && !iFrames) { HealthChange(Mathf.RoundToInt(-damageInc * incomingMult)); Time.timeScale = 0.5f; Invoke("EndTimeScale", 0.1f); } }
    private void EndTimeScale() { Time.timeScale = 1; }

    public virtual void KnockedDown(int duration) { Invoke("StandUp", duration); prone = true; anim.SetTrigger("Knockdown"); }
    public virtual void StandUp() { anim.SetTrigger("StandUp"); prone = false; }

    public virtual void Death() { anim.SetTrigger("Death"); this.enabled = false; GameObject.Find("UniverseController").GetComponent<UniverseController>().PlayerDeath(gameObject); GainIFrames(); }
    public virtual void KnockbackContinual()
    {
        transform.position += knockbackForce * knockBackPower * Time.deltaTime;
        visuals.transform.LookAt(lookAtTarget.position + lookAtVariant);
    }
    public virtual void Knockback(int power, Vector3 direction)
    {
        knockbackForce = direction;
        knockBackPower = power * 10;
        state = State.knockback;
        Invoke("StopKnockback", power * 0.01f);
    }
    public void StopKnockback() { knockbackForce = Vector3.zero; state = State.normal; }
    #endregion

    #region Utility Functions
    public virtual void HealthChange(int healthChange) { currentHealth += healthChange; if (currentHealth <= 0) { Death(); } }

    public virtual void GainCurse(float duration) { cursed = true; speed /= 2; curseTimer += duration; }
    public virtual void LoseCurse() { cursed = false; speed = baseSpeed; curseTimer = 0; }

    public void GainHA() { hyperArmour = true; }
    public void LoseHA() { hyperArmour = false; }

    public void GainIFrames() { iFrames = true; }
    public void LoseIFrames() { iFrames = false; }

    public virtual void Respawn()
    {
        currentHealth = healthMax;
        cursed = false;
        curseTimer = 0;
        poison = 0;
        prone = false;
        gameObject.SetActive(true);
        GainIFrames();
        Invoke("LoseIFrames", 3);
        anim.SetTrigger("Respawn");
        LoseIFrames();
        damageMult = 1;
        incomingMult = 1;

        state = State.normal;
        anim.SetFloat("Movement", 0);

    }
    protected void PoisonTick() { if (poison > 0) { currentHealth--; print("PoisonTick"); } }

    public void BeginActing() { acting = true; rb2d.velocity = Vector3.zero; state = State.attack; }
    public void EndActing() { acting = false; rb2d.velocity = Vector3.zero; state = State.normal; }

    public virtual void DodgeSliding(Vector3 dir) { print("Dodging"); transform.position += dir * dodgeSpeed * Time.deltaTime; }

    #endregion

    #region Returns
    public virtual int AccessUniqueFeature(int v)
    {
        return 0;
    }
    #endregion

}