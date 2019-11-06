using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skjegg : PlayerBase
{
    [Header("'tache Smash")]
    public int tacheDamage;
    public int tacheKnockback;
    public Weapons fist;

    [Header("Deadlock")]
    public int deadlockDamage;
    public int deadlockKnockback;
    public SpiritDagger dagger;

    [Header("Side Burners")]
    public int burnerForce;
    public int boosterSpeed;
    private bool burning;

    [Header("Hairline")]
    public int hairlineKnockback;
    public Weapons hairline;

    public override void AAction()
    {
        anim.SetTrigger("AAction");
        Knockback(burnerForce, visuals.transform.forward);
        Invoke("StopKnockback", 0.4f);
    }

    public override void BAction()
    {
        anim.SetTrigger("BAttack");
    }
    public void BEGINCOUNTER() { counterFrames = true; }
    public void ENDCOUNTER() { counterFrames = false; }

    public override void XAction()
    {
        anim.SetTrigger("XAction");
        fist.GainInfo(tacheDamage, tacheKnockback, visuals.transform.forward, pvp);
    }

    public override void YAction()
    {
        anim.SetTrigger("YAction");
        dagger.GainInfo(deadlockDamage, deadlockKnockback, visuals.transform.forward, pvp);
    }

    public void HairLine()
    {
        anim.SetTrigger("HairLine");
        hairline.GainInfo(0, hairlineKnockback, visuals.transform.forward, pvp);
    }

    public void FurtherBurn()
    {
        anim.SetTrigger("AAction");
        Knockback(boosterSpeed, visuals.transform.forward);
        Invoke("StopKnockback", 0.4f);
    }
}