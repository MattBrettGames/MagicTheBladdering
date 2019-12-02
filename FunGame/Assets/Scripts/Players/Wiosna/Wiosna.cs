using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wiosna : PlayerBase
{

    [Header("More Components")]
    public Weapons shotgunCone;
    public Weapons flameJet;
    public Weapons explosionSphere;

    [Header("Flame Jet")]
    public int flameJetDamage;
    public int flameJetKnockback;
    public int flameJetCost;

    [Header("Shotgun Burst")]
    public int shotgunDamage;
    public int shotgunKnockback;
    public int shotgunCost;

    [Header("Charge")]
    public int chargePerSecond;
    float currentCharge;
    public int maximumCharge;
    public ParticleSystem chargeDisplay;

    [Header("Explosion")]
    public int explosionDamage;
    public int explosionKnockback;

    [Header("Vanishing Act")]
    public float travelDistance;
    public float vanishingActCoodlown;

    public override void Update()
    {
        dir = new Vector3(player.GetAxis("HoriMove"), 0, player.GetAxis("VertMove")).normalized;
        dodgeTimer -= Time.deltaTime;

        if (poison > 0) { poison -= Time.deltaTime; }
        if (curseTimer <= 0) { LoseCurse(); }
        else { curseTimer -= Time.deltaTime; }

        aimTarget.position = transform.position + dir * 5;

        if (anim.GetCurrentAnimatorStateInfo(0).IsName("Idle") || anim.GetCurrentAnimatorStateInfo(0).IsName("Walking")) acting = false;

        switch (state)
        {
            case State.attack:
                break;

            case State.normal:

                anim.SetBool("LockOn", false);
                if (player.GetAxis("LockOn") >= 0.4f) { state = State.lockedOn; }

                if (!prone && !acting)
                {
                    //Rotating the Character Model
                    visuals.transform.LookAt(aimTarget);
                    rb2d.velocity = dir * speed;

                    //Standard Inputs
                    if (player.GetButtonDown("AAction")) { AAction(); }
                    if (player.GetButtonDown("BAttack")) { BAction(); }
                    if (player.GetButtonDown("XAttack")) { XAction(); }
                    if (player.GetButtonDown("YAttack")) { YAction(); }

                    if (player.GetAxis("HoriMove") != 0 || player.GetAxis("VertMove") != 0) { anim.SetFloat("Movement", 1); }
                    else { anim.SetFloat("Movement", 0); }
                }
                break;

            case State.lockedOn:

                walkDirection.position = dir + transform.position;

                anim.SetBool("LockOn", true);
                if (player.GetAxis("LockOn") <= 0.4f) { state = State.normal; }

                if (!prone && !acting)
                {
                    rb2d.velocity = dir * speed;

                    if (player.GetButtonDown("AAction")) { AAction(); }
                    if (player.GetButtonDown("BAttack")) { BAction(); }
                    if (player.GetButtonDown("XAttack")) { XAction(); }
                    if (player.GetButtonDown("YAttack")) { YAction(); }

                    if (player.GetAxis("HoriMove") != 0 || player.GetAxis("VertMove") != 0) { anim.SetFloat("Movement", 1); }
                    else { anim.SetFloat("Movement", 0); }

                    anim.SetFloat("Movement_X", -Vector3.SignedAngle(dir.normalized, visuals.transform.forward.normalized, Vector3.up) * 0.09f);
                    anim.SetFloat("Movement_ZY", -Vector3.SignedAngle(dir.normalized, visuals.transform.forward.normalized, Vector3.up) * 0.09f);
                }

                visuals.transform.LookAt(lookAtTarget.position + lookAtVariant);
                break;

            case State.dodging:
                if (dodgeTimer < 0) DodgeSliding(dir);
                break;

            case State.knockback:
                KnockbackContinual();
                break;

            case State.unique:
                currentCharge += chargePerSecond * Time.deltaTime;
                if (player.GetButtonUp("BAttack")) { print(currentCharge + " is Wiosna's currentCharge"); state = State.normal; anim.SetBool("Charging", false); }
                if (currentCharge <= maximumCharge)
                {
                    Explosion();
                }
                chargeDisplay.emission.rateOverTime.Equals(currentCharge * 10);
                break;

        }

        print("Wiosna is " + state);
    }

    private void Explosion()
    {
        anim.SetTrigger("Explosion");
        explosionSphere.gameObject.SetActive(true);
        Knockback(explosionKnockback, new Vector3(UnityEngine.Random.Range(-1f, 1f), 0, UnityEngine.Random.Range(-1f, 1f)));
        TakeDamage(explosionDamage);
        explosionSphere.GainInfo(explosionDamage, explosionKnockback, visuals.transform.forward, pvp);
        Invoke("EndExplsion", 0.3f);
    }
    void EndExplsion()
    {
        explosionSphere.gameObject.SetActive(false);
    }

    public override void BAction()
    {
        anim.SetBool("Charging", true);
        state = State.unique;
    }

    public override void AAction()
    {
        anim.SetTrigger("AAction");
    }
    public void DoTheTeleport()
    {
        transform.position += dir * travelDistance;
    }

    public override void XAction()
    {
        anim.SetTrigger("XAttack");
        flameJet.GainInfo(Mathf.RoundToInt(flameJetDamage + currentCharge), flameJetKnockback, visuals.transform.forward, pvp);
        currentCharge -= flameJetCost;
        if (currentCharge < 0) currentCharge = 0;
    }
    public void FlameJetOn() { flameJet.gameObject.SetActive(true); flameJet.GetComponentInChildren<ParticleSystem>().Play(); }
    public void FlameJetOff() { flameJet.gameObject.SetActive(false); }

    public override void YAction()
    {
        anim.SetTrigger("YAttack");
        shotgunCone.GainInfo(shotgunDamage, Mathf.RoundToInt(shotgunKnockback + currentCharge), visuals.transform.forward, pvp);
        currentCharge -= shotgunCost;
        if (currentCharge < 0) currentCharge = 0;
    }

    public void ShotgunOn() { shotgunCone.gameObject.SetActive(true); }
    public void ShotgunOff() { shotgunCone.gameObject.SetActive(false); }



    public override void BeginActing() { acting = true; rb2d.velocity = Vector3.zero; }

}