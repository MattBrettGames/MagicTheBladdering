using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PlayerBase : BlankMono
{

    [Header("Player Determination")]
    public string thisPlayer;

    [Header("Movement Stats")]
    public float speed;
    private float baseSpeed;
    protected float moving;

    [Header("Common Stats")]
    public int currentHealth;
    protected int healthMax;
    protected int damageMult = 1;
    protected int incomingMult = 1;

    [Header("Status Effects")]
    public bool cursed;
    public bool prone;
    public float poison;

    [Header("Components")]
    public Transform aimTarget;
    public GameObject visuals;
    protected Animator anim;
    protected Rigidbody rb2d;

    private string horiPlayerInput;
    private string vertPlayerInput;
    private string aPlayerInput;
    private string bPlayerInput;
    private string xPlayerInput;
    private string yPlayerInput;

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

    public virtual void Update()
    {
        float hori = Input.GetAxis(horiPlayerInput);
        float vert = Input.GetAxis(vertPlayerInput);
        if (!prone)
        {
            //Rotating the Character Model
            aimTarget.position = transform.position + new Vector3(hori, 0, vert).normalized * 3;
            visuals.transform.LookAt(aimTarget);

            transform.Translate(new Vector3(hori, 0, vert).normalized * speed);
            if (Input.GetAxisRaw(thisPlayer + "Horizontal") != 0 || Input.GetAxisRaw(thisPlayer + "Vertical") != 0) { anim.SetFloat("Movement", 1); }
            else { anim.SetFloat("Movement", 0); }


            //Standard Inputs
            if (Input.GetButtonDown(aPlayerInput)) { AAction(); }
            if (Input.GetButtonDown(bPlayerInput)) { BAction(); }
            if (Input.GetButtonDown(xPlayerInput)) { XAction(); }
            if (Input.GetButtonDown(yPlayerInput)) { YAction(); }
        }

        if (poison > 0) { poison -= Time.deltaTime; }

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
    public virtual void TakeDamage(int damageInc) { HealthChange(-damageInc * incomingMult); anim.SetTrigger("Stagger"); }
    public virtual void KnockedDown(int duration) { Invoke("StandUp", duration); prone = true; anim.SetTrigger("Knockdown"); }
    public virtual void StandUp() { anim.SetTrigger("StandUp"); prone = false; }
    public virtual void Death() { anim.SetTrigger("Death"); this.enabled = false; }
    public virtual void Knockback(int power, Vector3 direction) { rb2d.AddForce(direction * power, ForceMode.Impulse); }
    #endregion

    #region Utility Functions
    public virtual void HealthChange(int healthChange) { currentHealth += healthChange; if (currentHealth <= 0) { Death(); } }
    public virtual void GainCurse(float duration) { cursed = true; speed /= 2; Invoke("LoseCurse", duration); }
    public virtual void LoseCurse() { cursed = false; speed = baseSpeed; }
    #endregion

}
