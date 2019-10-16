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
        fist.GainInfo(tacheDamage, tacheKnockback, visuals.transform.forward);
    }

    public override void YAction()
    {
        anim.SetTrigger("YAction");
        dagger.GainInfo(deadlockDamage, deadlockKnockback, visuals.transform.forward);
    }

    public void HairLine()
    {
        anim.SetTrigger("HairLine");
        hairline.GainInfo(0, hairlineKnockback, visuals.transform.forward);
    }

    public void FurtherBurn()
    {
        anim.SetTrigger("AAction");
        Knockback(boosterSpeed, visuals.transform.forward);
        Invoke("StopKnockback", 0.4f);
    }



    /*public override void Update()
    {
        float hori = Input.GetAxis(horiPlayerInput);
        float vert = Input.GetAxis(vertPlayerInput);
        if (!prone)
        {
            //Rotating the Character Model
            aimTarget.position = transform.position + new Vector3(hori, 0, vert).normalized * 3;
            visuals.transform.LookAt(aimTarget);

            transform.position = Vector3.Slerp(transform.position, aimTarget.position, speed);
            //transform.Translate(new Vector3(hori, 0, vert).normalized * speed);
            if (Input.GetAxisRaw(horiPlayerInput) != 0 || Input.GetAxisRaw(vertPlayerInput) != 0) { anim.SetFloat("Movement", 1); }

            //Standard Inputs
            if (Input.GetButtonDown(aPlayerInput) && !burning) { AAction(); }
            if (Input.GetButtonDown(bPlayerInput)) { BAction(); }
            if (Input.GetButtonDown(xPlayerInput)) { XAction(); }
            if (Input.GetButtonDown(yPlayerInput)) { YAction(); }

            if (Input.GetButtonDown(aPlayerInput) && burning)
            {
                EndBurn();
            }
        }

        if (poison > 0) { poison -= Time.deltaTime; }
        if (curseTimer <= 0) { LoseCurse(); }
        else { curseTimer -= Time.deltaTime; }

    }*/

}