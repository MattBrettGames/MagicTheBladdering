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
    [SerializeField] private float overheadStun;

    [Header("Kick Up")]
    public int kickAttack;
    public int kickKnockback;

    [Header("Frenzy")]
    public int frenzyDuration;
    public int frenzyBonus;
    public int frenzySpeedBuff;
    private bool frenzy;
    public ParticleSystem frenzyEffects;

    [Header("Passives")]
    [SerializeField] private float comboTimeDur;
    private bool comboTime;


    public override void Start()
    {
        base.Start();
        hammer.gameObject.tag = tag;
    }

    public override void Update()
    {
        if (aTimer > 0) aTimer -= Time.deltaTime;
        if (bTimer > 0) bTimer -= Time.deltaTime;
        if (xTimer > 0) xTimer -= Time.deltaTime;
        if (yTimer > 0) yTimer -= Time.deltaTime;

        dir = new Vector3(player.GetAxis("HoriMove"), 0, player.GetAxis("VertMove")).normalized;

        if (poison > 0) { poison -= Time.deltaTime; }

        aimTarget.position = transform.position + dir * 5;

        if (anim.GetCurrentAnimatorStateInfo(0).IsName("Idle") || anim.GetCurrentAnimatorStateInfo(0).IsName("Walking")) acting = false;

        if (player.GetButtonDown("BAttack")) { BAction(); }


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
                    if (player.GetButtonDown("XAttack")) { XAction(); }
                    if (player.GetButtonDown("YAttack")) { YAction(); }

                    if (player.GetAxis("HoriMove") != 0 || player.GetAxis("VertMove") != 0) { anim.SetFloat("Movement", 1); }
                    else { anim.SetFloat("Movement", 0); }

                    anim.SetFloat("Movement_X", -Vector3.SignedAngle(dir.normalized, visuals.transform.forward.normalized, Vector3.up) * 0.09f);
                    anim.SetFloat("Movement_ZY", -Vector3.SignedAngle(dir.normalized, visuals.transform.forward.normalized, Vector3.up) * 0.09f);
                }

                aimTarget.LookAt(lookAtTarget.position + lookAtVariant);
                visuals.transform.forward = Vector3.Lerp(visuals.transform.forward, aimTarget.forward, 0.3f);
                break;

            case State.dodging:
                if (aTimer < 0) DodgeSliding(dir);
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
            if (xTimer <= 0)
            {
                universe.PlaySound(xSound);
                hammer.GainInfo(Mathf.RoundToInt(xAttack * damageMult), Mathf.RoundToInt(xKnockback * damageMult), visuals.transform.forward, pvp, 0, this);
                anim.SetTrigger("XAttack");
                xTimer = xCooldown;
            }
        }
        else
        {
            universe.PlaySound(xSound);
            hammer.GainInfo(Mathf.RoundToInt(spinDamage * damageMult), Mathf.RoundToInt(spinKnockback * damageMult), visuals.transform.forward, pvp, 0, this);
            anim.SetTrigger("Spin");
        }
    }

    public override void YAction()
    {
        if (comboTime)
        {
            universe.PlaySound(ySound);
            hammer.GainInfo(Mathf.RoundToInt(kickAttack * damageMult), Mathf.RoundToInt(kickKnockback * damageMult), visuals.transform.forward, pvp, 0, this);
            anim.SetTrigger("ComboKick");
            comboTime = false;
        }
        else
        {
            if (yTimer <= 0)
            {
                universe.PlaySound(ySound);
                hammer.GainInfo(Mathf.RoundToInt(slamAttack * damageMult), Mathf.RoundToInt(slamKnockback * damageMult), visuals.transform.forward, pvp, overheadStun, this);
                anim.SetTrigger("YAttack");
                yTimer = yCooldown;
            }
        }
    }
    public void OpenComboKick() { comboTime = true; outline.OutlineColor = new Color(1, 1, 1); Invoke("CloseComboKick", comboTimeDur); }
    private void CloseComboKick() { comboTime = false; outline.OutlineColor = new Color(0, 0, 0); }

    public override void BAction()
    {
        if (!frenzy && state != State.stun && bTimer <= 0)
        {
            universe.PlaySound(bSound);

            Invoke("StopFrenzy", frenzyDuration);
            speed += frenzySpeedBuff;
            anim.SetTrigger("BAttack");
            damageMult += frenzyBonus;
            incomingMult += frenzyBonus;
            frenzy = true;
            frenzyEffects.Play();
            bTimer = bCooldown;

            dodgeDur += 0.2f;
            dodgeSpeed += 5;

        }
    }
    private void StopFrenzy()
    {
        damageMult -= frenzyBonus;
        incomingMult -= frenzyBonus;

        dodgeDur -= 0.2f;
        dodgeSpeed -= 5;

        speed -= frenzySpeedBuff;
        frenzy = false;
        frenzyEffects.Clear();
        frenzyEffects.Stop();
    }
}