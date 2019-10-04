using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Valderheim : PlayerBase
{

    [Header("More Componenets")]
    public Weapons weapon;

    [Header("Wide Swing")]
    public int xAttack;
    public int xKnockback;

    [Header("Ground Slam")]
    public int slamAttack;
    public int slamKnockback;
    public int kickAttack;
    public int kickKnockback;
    private bool comboTime;

    [Header("Frenzy")]
    public int frenzyDuration;
    public int frenzyMult;

    [Header("Charge!")]
    public float speedMult;
    private float currentSpeed;

    public override void XAction()
    {
        weapon.GainInfo(xAttack, xKnockback, visuals.transform.forward);
        anim.SetTrigger("XAttack");
    }

    public override void YAction()
    {
        if (comboTime)
        {
            weapon.GainInfo(kickAttack, kickKnockback, visuals.transform.forward);
            print("Combo Kick!");
            anim.SetTrigger("ComboKick");
        }
        else
        {
            weapon.GainInfo(slamAttack, slamKnockback, visuals.transform.forward);
            print("Hammer Slam!");
            anim.SetTrigger("YAttack");
        }
    }
    public void OpenComboKick() { comboTime = true; }
    public void CloseComboKick() { comboTime = false; }

    public override void BAction()
    {
        Invoke("StopFrenzy", frenzyDuration);
        damageMult = frenzyMult;
        incomingMult = frenzyMult;
    }
    private void StopFrenzy()
    {
        damageMult = 1;
        incomingMult = 1;
    }

    public override void AAction()
    {
        float hori = Input.GetAxis(horiPlayerInput);
        float vert = Input.GetAxis(vertPlayerInput);
        anim.SetTrigger("AAction");
        currentSpeed = speed;
        speed *= speedMult;
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();
        if (Input.GetButtonUp(thisPlayer + "AButton"))
        {
            speed = currentSpeed;
        }
    }

    //Passive Effects - Surefooted & Building Rage
    public override void KnockedDown(int power) { }
    public override void HealthChange(int healthChange) { base.HealthChange(healthChange); damageMult = (healthMax - currentHealth) / 10; }

}
