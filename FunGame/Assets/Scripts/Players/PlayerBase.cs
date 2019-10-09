using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    [Header("Components")]
    public Transform aimTarget;
    public GameObject visuals;
    protected Animator anim;
    protected Rigidbody rb2d;

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
    }

    public virtual void FixedUpdate()
    {
        float hori = Input.GetAxis(horiPlayerInput);
        float vert = Input.GetAxis(vertPlayerInput);
        if (!prone && !knockbackForce)
        {
            //Rotating the Character Model
            aimTarget.position = transform.position + new Vector3(hori, 0, vert).normalized * 3;
            visuals.transform.LookAt(aimTarget);

            transform.position = Vector3.Slerp(transform.position, aimTarget.position, speed);
            //transform.Translate(new Vector3(hori, 0, vert).normalized * speed);
            if (Input.GetAxisRaw(horiPlayerInput) != 0 || Input.GetAxisRaw(vertPlayerInput) != 0) { anim.SetFloat("Movement", 1); }

            //Standard Inputs
            if (Input.GetButtonDown(aPlayerInput)) { AAction(); }
            if (Input.GetButtonDown(bPlayerInput)) { BAction(); }
            if (Input.GetButtonDown(xPlayerInput)) { XAction(); }
            if (Input.GetButtonDown(yPlayerInput)) { YAction(); }
        }

        if (poison > 0) { poison -= Time.deltaTime; }
        if (curseTimer <= 0) { LoseCurse(); }
        else { curseTimer -= Time.deltaTime; }

        //Testing Inputs
        //if (Input.GetKeyDown(KeyCode.Space)) { TakeDamage(70); }
        //print(Input.GetAxis(horiPlayerInput)+" - Horizontal");
        //print(Input.GetAxis(vertPlayerInput)+" - Vertical");
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

    public virtual void Death() { anim.SetTrigger("Death"); enabled = false; print(gameObject.name + " has just been killed!"); GameObject.Find("UniverseController").GetComponent<UniverseController>().PlayerDeath(gameObject); }
    public virtual void Knockback(int power, Vector3 direction) { rb2d.AddForce(direction * power * 10, ForceMode.Impulse); Invoke("StopKnockback", power / 10f); print(power / 10f); knockbackForce = true; }
    private void StopKnockback() { rb2d.velocity = Vector3.zero; knockbackForce = false; }
    #endregion

    #region Utility Functions
    public virtual void HealthChange(int healthChange) { currentHealth += healthChange; if (currentHealth <= 0) { Death(); } }
    public virtual void GainCurse(float duration) { cursed = true; speed /= 2; curseTimer += duration; }

    public virtual void LoseCurse() { cursed = false; speed = baseSpeed; }

    public void GainHA() { hyperArmour = true; }
    public void LoseHA() { hyperArmour = false; }

    public void Respawn() { currentHealth = healthMax; cursed = false; curseTimer = 0; poison = 0; prone = false; }
    #endregion
}