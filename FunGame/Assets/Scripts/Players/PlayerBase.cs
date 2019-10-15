using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;
public abstract class PlayerBase : BlankMono
{

    [Header("GameMode Stuff")]
    public string thisPlayer;
    public int numOfDeaths = 0;

    [Header("Movement Stats")]
    public float speed;
    private float baseSpeed;
    protected float moving;

    [Header("Common Stats")]
    public int currentHealth;
    public int healthMax;
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
    protected bool knockbackForce;
    protected bool acting;

    [Header("Components")]
    public Transform aimTarget;
    public GameObject visuals;
    protected Animator anim;
    protected Rigidbody rb2d;
    protected PlayerController playerCont;

    protected string horiPlayerInput;
    protected string vertPlayerInput;
    protected string aPlayerInput;
    protected string bPlayerInput;
    protected string xPlayerInput;
    protected string yPlayerInput;

    public virtual void Start()
    {
        anim = gameObject.GetComponentInChildren<Animator>();
        rb2d = gameObject.GetComponent<Rigidbody>();

        //More Efficient version of the multi-player input system
        horiPlayerInput = thisPlayer + "Horizontal";
        vertPlayerInput = thisPlayer + "Vertical";
        aPlayerInput = thisPlayer + "AButton";
        bPlayerInput = thisPlayer + "BButton";
        xPlayerInput = thisPlayer + "XButton";
        yPlayerInput = thisPlayer + "YButton";
        baseSpeed = speed;

        InvokeRepeating("PoisonTick", 0, 0.5f);
    }

    public virtual void Update()
    {
        if (!prone && !knockbackForce && !acting)
        {
            //Rotating the Character Model
            aimTarget.position = transform.position + new Vector3(playerCont.GetAxis(0), 0, playerCont.GetAxis(0)).normalized * 3;
            visuals.transform.LookAt(aimTarget);

            transform.position = Vector3.Slerp(transform.position, aimTarget.position, speed);

            if (playerCont.GetAxis(0) != 0 || playerCont.GetAxis(1) != 0) { anim.SetFloat("Movement", 1); }

            //Standard Inputs
            if (playerCont.GetButtonDown(4)) { AAction(); }
            if (playerCont.GetButtonDown(5)) { BAction(); }
            if (playerCont.GetButtonDown(6)) { XAction(); }
            if (playerCont.GetButtonDown(7)) { YAction(); }
        }

        if (poison > 0) { poison -= Time.deltaTime; }
        if (curseTimer <= 0) { LoseCurse(); }
        else { curseTimer -= Time.deltaTime; }
    }

    #region Input Actions
    public virtual void AAction() { anim.SetTrigger("AAction"); }
    public virtual void BAction() { anim.SetTrigger("BAttack"); }
    public virtual void XAction() { anim.SetTrigger("XAttack"); }
    public virtual void YAction() { anim.SetTrigger("YAttack"); }
    #endregion

    #region Common Events
    public virtual void TakeDamage(int damageInc) { if (!counterFrames && !iFrames) { HealthChange(Mathf.RoundToInt(-damageInc * incomingMult)); if (!hyperArmour) { anim.SetTrigger("Stagger"); } } }

    public virtual void KnockedDown(int duration) { Invoke("StandUp", duration); prone = true; anim.SetTrigger("Knockdown"); }
    public virtual void StandUp() { anim.SetTrigger("StandUp"); prone = false; }

    public virtual void Death() { anim.SetTrigger("Death"); this.enabled = false; print(gameObject.name + " has just been killed!"); GameObject.Find("UniverseController").GetComponent<UniverseController>().PlayerDeath(gameObject); }
    public virtual void Knockback(int power, Vector3 direction) { rb2d.AddForce(direction * power * 10, ForceMode.Impulse); Invoke("StopKnockback", power / 10f); knockbackForce = true; }
    protected void StopKnockback() { rb2d.velocity = Vector3.zero; knockbackForce = false; }
    #endregion

    #region Utility Functions
    public virtual void HealthChange(int healthChange) { currentHealth += healthChange; if (currentHealth <= 0) { Death(); } }

    public virtual void GainCurse(float duration) { cursed = true; speed /= 2; curseTimer += duration; }
    public virtual void LoseCurse() { cursed = false; speed = baseSpeed; curseTimer = 0; }

    public void GainHA() { hyperArmour = true; }
    public void LoseHA() { hyperArmour = false; }

    public void GainIFrames() { iFrames = true; }
    public void LoseIFrames() { iFrames = false; }

    public void Respawn() { currentHealth = healthMax; cursed = false; curseTimer = 0; poison = 0; prone = false; }
    protected void PoisonTick() { if (poison > 0) { currentHealth--; print("PoisonTick"); } }


    public void BeginActing() { acting = true; }
    public void EndActing() { acting = false; }

    #endregion
}