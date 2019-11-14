using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;

public class Valderheim : PlayerBase
{
    [Header("More Componenets")]
    public Weapons hammer;
    public Outline outline;

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

    public override void Start()
    {
        base.Start();
        hammer.gameObject.tag = tag;
    }

    public override void Update()
    {
        dir = new Vector3(player.GetAxis("HoriMove"), 0, player.GetAxis("VertMove")).normalized;
        dodgeTimer -= Time.deltaTime;

        if (poison > 0) { poison -= Time.deltaTime; }
        if (curseTimer <= 0) { LoseCurse(); }
        else { curseTimer -= Time.deltaTime; }

        aimTarget.position = transform.position + dir * 5;

        switch (state)
        {
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
                else
                {
                    dir = Vector3.zero;
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

                    if (Vector3.Angle(visuals.transform.forward, dir) >= 130)
                    {
                        anim.SetFloat("Movement_ZY", -1);
                    }
                    else if (Vector3.Angle(visuals.transform.forward, dir) <= 50)
                    {
                        anim.SetFloat("Movement_ZY", 1);
                    }
                    if (Vector3.SignedAngle(visuals.transform.forward, dir, Vector3.up) < 130 && Vector3.SignedAngle(visuals.transform.forward, dir, Vector3.up) > 50)
                    {
                        anim.SetFloat("Movement_X", 1);
                    }
                    else if (Vector3.SignedAngle(visuals.transform.forward, dir, Vector3.up) < -130 && Vector3.SignedAngle(visuals.transform.forward, dir, Vector3.up) > -50)
                    {
                        anim.SetFloat("Movement_X", -1);
                    }
                }

                visuals.transform.LookAt(lookAtTarget.position + lookAtVariant);

                break;


            case State.dodging:
                if (dodgeTimer < 0) DodgeSliding(dir);
                break;

            case State.knockback:
                KnockbackContinual();
                break;
        }
    }

    public override void XAction()
    {
        if (!comboTime)
        {
            hammer.GainInfo(Mathf.RoundToInt(xAttack * damageMult), Mathf.RoundToInt(xKnockback * damageMult), visuals.transform.forward, pvp);
            anim.SetTrigger("XAttack");
        }
        else
        {
            hammer.GainInfo(Mathf.RoundToInt(spinDamage * damageMult), Mathf.RoundToInt(spinKnockback * damageMult), visuals.transform.forward, pvp);
            anim.SetTrigger("Spin");
        }
    }

    public override void YAction()
    {
        if (comboTime)
        {
            hammer.GainInfo(Mathf.RoundToInt(kickAttack * damageMult), Mathf.RoundToInt(kickKnockback * damageMult), visuals.transform.forward, pvp);
            anim.SetTrigger("ComboKick");
        }
        else
        {
            hammer.GainInfo(Mathf.RoundToInt(slamAttack * damageMult), Mathf.RoundToInt(slamKnockback * damageMult), visuals.transform.forward, pvp);
            anim.SetTrigger("YAttack");
        }
    }
    public void OpenComboKick() { comboTime = true; outline.OutlineColor = new Color(1, 1, 1); }
    public void CloseComboKick() { comboTime = false; outline.OutlineColor = new Color(0, 0, 0); }

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
        state = State.dodging;
        Invoke("EndDodge", dodgeDur);
    }

    public void EndDodge()
    {
        state = State.normal;
        dodgeTimer = dodgeCooldown;

    }


    //Passive Effects - Surefooted & Building Rage
    public override void HealthChange(int healthChange) { base.HealthChange(healthChange); damageMult = Mathf.RoundToInt((healthMax - currentHealth) / growingRageDiv) + 1; }

}