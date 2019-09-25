using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PlayerBase : BlankMono
{

    [Header("Player Determination")]
    public string thisPlayer;

    [Header("Movement Stats")]
    private float speed = 0.1f;

    [Header("Common Stats")]
    private int healthMax;
    public int currentHealth;

    [Header("Components")]
    private Animator anim;
    public Transform aimTarget;

    void Start()
    {
        anim = gameObject.GetComponent<Animator>();
    }

    void Update()
    {
        float hori = Input.GetAxis(thisPlayer + "Horizontal");
        float vert = Input.GetAxis(thisPlayer + "Vertical");
        anim.speed = hori + vert;
        transform.Translate(new Vector3(hori, 0, vert)*speed);
        aimTarget.position = transform.position + new Vector3(hori, 0, vert);
        transform.LookAt(aimTarget);

        if (Input.GetButtonDown(thisPlayer + "AButton")) { AAction(); }
        if (Input.GetButtonDown(thisPlayer + "BButton")) { BAction(); }
        if (Input.GetButtonDown(thisPlayer + "XButton")) { XAction(); }
        if (Input.GetButtonDown(thisPlayer + "YButton")) { YAction(); }

    }

    public virtual void AAction() { print("Whatever's on the A button would've just happened"); }
    public virtual void BAction() { anim.SetTrigger("BAttack"); }
    public virtual void XAction() { anim.SetTrigger("XAttack"); }
    public virtual void YAction() { anim.SetTrigger("YAttack"); }

}
