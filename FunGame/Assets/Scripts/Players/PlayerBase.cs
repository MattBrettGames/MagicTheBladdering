using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PlayerBase : BlankMono
{

    [Header("Player Determination")]
    public string thisPlayer;

    [Header("Movement Stats")]
    public float speed;
    protected float moving;
    public float turningSpeed;

    [Header("Common Stats")]
    protected int healthMax;
    public int currentHealth;
    protected int damageMult = 1;
    protected int incomingMult = 1;

    [Header("Components")]
    protected Animator anim;
    public Transform aimTarget;
    public GameObject visuals;

    [Header("Input Strings")]
    string horiPlayerInput;
    string vertPlayerInput;
    string aPlayerInput;
    string bPlayerInput;
    string xPlayerInput;
    string yPlayerInput;

    void Start()
    {
        anim = gameObject.GetComponentInChildren<Animator>();

        //More Efficient version of the multi-player input system
        horiPlayerInput = thisPlayer + "Horizontal";
        vertPlayerInput = thisPlayer + "Vertical";
        aPlayerInput = thisPlayer + "AButton";
        bPlayerInput = thisPlayer + "BButton";
        xPlayerInput = thisPlayer + "XButton";
        yPlayerInput = thisPlayer + "YButton";
    }

    void Update()
    {
        float hori = Input.GetAxis(horiPlayerInput);
        float vert = Input.GetAxis(vertPlayerInput);
        transform.Translate(new Vector3(-vert, 0, hori) * speed);
        if (Input.GetAxisRaw(thisPlayer + "Horizontal") != 0 || Input.GetAxisRaw(thisPlayer + "Vertical") != 0) { anim.SetFloat("Movement", 1); }
        else { anim.SetFloat("Movement", 0); }

        //Rotating the Character Model
        aimTarget.position = transform.position + new Vector3(hori, 0, vert);

        Quaternion newRot = Quaternion.LookRotation(aimTarget.position - transform.position);
        transform.rotation = Quaternion.Slerp(visuals.transform.rotation, newRot, turningSpeed);

        if (Input.GetButtonDown(aPlayerInput)) { AAction(); }
        if (Input.GetButtonDown(bPlayerInput)) { BAction(); }
        if (Input.GetButtonDown(xPlayerInput)) { XAction(); }
        if (Input.GetButtonDown(yPlayerInput)) { YAction(); }

        //Playtesting Inputs
        //if (Input.GetKeyDown(KeyCode.Space)) { TakeDamage(70); }
        if (Input.GetKeyDown(KeyCode.G)) { XAction(); }
    }

    #region Input Actions
    public virtual void AAction() { print("Whatever's on the A button would've just happened"); }
    public virtual void BAction() { anim.SetTrigger("BAttack"); }
    public virtual void XAction() { anim.SetTrigger("XAttack"); }
    public virtual void YAction() { anim.SetTrigger("YAttack"); }
    #endregion

    #region Common Events
    public virtual void TakeDamage(int damageInc) { HealthChange(-damageInc); anim.SetTrigger("Stagger"); }
    public virtual void KnockedDown(int duration) { Invoke("StandUp", duration); anim.SetTrigger("Knockdown"); }
    public virtual void StandUp() { }
    public virtual void Death() { anim.SetTrigger("Death"); Destroy(this); }
    #endregion

    #region Utility Functions
    public virtual void HealthChange(int healthChange) { currentHealth += healthChange; if (currentHealth <= 0) { Death(); } }
    #endregion


}
