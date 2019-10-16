using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;
public class Valderheim : PlayerBase
{
    [Header("More Componenets")]
    public Weapons hammer;
    public Weapons chest;

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
    public int frenzyBonus;
    private bool frenzy;

    [Header("Charge!")]
    public int dodgeForce;

    [Header("Passives")]
    public int growingRageDiv;


    public override void XAction()
    {
        hammer.GainInfo(Mathf.RoundToInt(xAttack * damageMult), Mathf.RoundToInt(xKnockback * damageMult), visuals.transform.forward);
        anim.SetTrigger("XAttack");
    }

    public override void YAction()
    {
        if (comboTime)
        {
            hammer.GainInfo(Mathf.RoundToInt(kickAttack * damageMult), Mathf.RoundToInt(kickKnockback * damageMult), visuals.transform.forward);
            anim.SetBool("Comboing", true);
            anim.SetTrigger("YAttack");
        }
        else
        {
            anim.SetBool("Comboing", false);
            hammer.GainInfo(Mathf.RoundToInt(slamAttack * damageMult), Mathf.RoundToInt(slamKnockback * damageMult), visuals.transform.forward);
            anim.SetTrigger("YAttack");
        }
    }
    public void OpenComboKick() { comboTime = true; }
    public void CloseComboKick() { comboTime = false; }

    public override void BAction()
    {
        if (!frenzy)
        {
            Invoke("StopFrenzy", frenzyDuration);
            anim.SetTrigger("BAttack");
            damageMult += frenzyBonus;
            incomingMult += frenzyBonus;
            frenzy = true;
        }
    }
    private void StopFrenzy()
    {
        damageMult -= frenzyBonus;
        incomingMult -= frenzyBonus;
        frenzy = false;
    }

    public override void AAction()
    {
        anim.SetTrigger("AAction");
        Knockback(dodgeForce, visuals.transform.forward);
        Invoke("StopKnockback", 0.4f);
    }

    //Passive Effects - Surefooted & Building Rage
    //public override void KnockedDown(int power) { }
    public override void HealthChange(int healthChange) { base.HealthChange(healthChange); damageMult = Mathf.RoundToInt((healthMax - currentHealth) / growingRageDiv) + 1; }

}