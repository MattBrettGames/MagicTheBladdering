using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;

public class Valderheim : PlayerBase
{
    [Header("More Componenets")]
    public Weapons hammer;

    [Header("Wide Swing")]
    public int xAttack;
    public int xKnockback;

    [Header("Spin")]
    public int spinDamage;
    public int spinKnockback;

    [Header("Ground Slam")]
    public int slamAttack;
    public int slamKnockback;

    [Header("Kick Up")]
    public int kickAttack;
    public int kickKnockback;

    [Header("Frenzy")]
    public int frenzyDuration;
    public int frenzyBonus;
    private bool frenzy;
    public ParticleSystem frenzyEffects;

    [Header("Passives")]
    public int growingRageDiv;
    private bool comboTime;

    public override void XAction()
    {
        if (!comboTime)
        {
            hammer.GainInfo(Mathf.RoundToInt(xAttack * damageMult), Mathf.RoundToInt(xKnockback * damageMult), visuals.transform.forward, pvp);
        }
        else
        {
            Vector3 dir = visuals.transform.forward;
            anim.SetBool("Comboing", true);
            hammer.GainInfo(Mathf.RoundToInt(spinDamage * damageMult), Mathf.RoundToInt(spinKnockback * damageMult), visuals.transform.forward, pvp);
        }
        anim.SetTrigger("XAttack");
    }

    public override void YAction()
    {
        if (comboTime)
        {
            hammer.GainInfo(Mathf.RoundToInt(kickAttack * damageMult), Mathf.RoundToInt(kickKnockback * damageMult), visuals.transform.forward, pvp);
            anim.SetBool("Comboing", true);
            anim.SetTrigger("YAttack");
        }
        else
        {
            anim.SetBool("Comboing", false);
            hammer.GainInfo(Mathf.RoundToInt(slamAttack * damageMult), Mathf.RoundToInt(slamKnockback * damageMult), visuals.transform.forward, pvp);
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
            frenzyEffects.Play();
        }
    }
    private void StopFrenzy()
    {
        damageMult -= frenzyBonus;
        incomingMult -= frenzyBonus;
        frenzy = false;
        frenzyEffects.Clear();
        frenzyEffects.Stop();
    }

    public override void AAction()
    {
        anim.SetTrigger("AAction");
        dodgeTimer = dodgeTimer;
        state = State.dodging;
        Invoke("EndDodge", dodgeDur);
    }

    public void EndDodge() { state = State.normal; }


    //Passive Effects - Surefooted & Building Rage
    public override void HealthChange(int healthChange) { base.HealthChange(healthChange); damageMult = Mathf.RoundToInt((healthMax - currentHealth) / growingRageDiv) + 1; }

}