using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PlayerBase : BlankMono
{

    [Header("Player Determination")]
    public string thisPlayer;

    [Header("Movement Stats")]
    protected float speed = 0.1f;
    protected float moving;

    [Header("Common Stats")]
    protected int healthMax;
    public int currentHealth;
    protected int damageMult = 1;
    protected int incomingMult = 1;

    [Header("Components")]
    protected Animator anim;
    public Transform aimTarget;
    public GameObject visuals;

    void Start()
    {
        anim = gameObject.GetComponentInChildren<Animator>();
    }

    void Update()
    {
        float hori = Input.GetAxis(thisPlayer + "Horizontal");
        float vert = Input.GetAxis(thisPlayer + "Vertical");
        transform.Translate(new Vector3(hori, 0, vert) * speed);
        if (Input.GetAxisRaw(thisPlayer + "Horizontal") != 0 || Input.GetAxisRaw(thisPlayer + "Vertical") != 0) { anim.SetFloat("Movement", 1); }
        else { anim.SetFloat("Movement", 0); }

        aimTarget.position = transform.position + new Vector3(hori, 0, vert);

        if (Input.GetButtonDown(thisPlayer + "AButton")) { AAction(); }
        if (Input.GetButtonDown(thisPlayer + "BButton")) { BAction(); }
        if (Input.GetButtonDown(thisPlayer + "XButton")) { XAction(); }
        if (Input.GetButtonDown(thisPlayer + "YButton")) { YAction(); }
        if (Input.GetKeyDown(KeyCode.Space)) { TakeDamage(-70); }

        visuals.transform.LookAt(aimTarget);
    }

    #region Input Actions
    public virtual void AAction() { print("Whatever's on the A button would've just happened"); }
    public virtual void BAction() { anim.SetTrigger("BAttack"); }
    public virtual void XAction() { anim.SetTrigger("XAttack"); }
    public virtual void YAction() { anim.SetTrigger("YAttack"); }
    #endregion

    #region Common Events
    public virtual void TakeDamage(int damageInc) { HealthChange(damageInc); anim.SetTrigger("Stagger"); }
    public virtual void KnockedDown(int duration) { Invoke("StandUp", duration); anim.SetTrigger("Knockdown"); }
    public virtual void StandUp() { }
    public virtual void Death() { anim.SetTrigger("Death"); Destroy(this); }
    #endregion

    #region Utility Functions
    public virtual void HealthChange(int healthChange) { currentHealth += healthChange; if (currentHealth <= 0) { Death(); } }
    #endregion


}
