using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wiosna : PlayerBase
{

    [Header("More Components")]
    public Weapons basicMelee;
    public Weapons explosionSphere;
    public Weapons finalBeam;


    [Header("X Attack")]
    [SerializeField] int xDamage;
    [SerializeField] int xKnockback;

    [Header("Vanishing Act")]
    [SerializeField] float vanishDistance;

    [Header("Y Action")]
    [SerializeField] float radiusOfStun;
    [SerializeField] float stunDur;
    [Space]
    [SerializeField] float radiusOfPull;
    [SerializeField] int pullImpact;

    [Header("BAttacks")]
    [SerializeField] float bCooldown;
    [Space]
    [SerializeField] int beamDamage;
    [SerializeField] int beamKnockback;
    [SerializeField] float beamDur;
    [Space]
    [SerializeField] int explosionDamage;
    [SerializeField] int explosionKnockback;
    [SerializeField] float explosionDur;

    public override void Update()
    {
        dir = new Vector3(player.GetAxis("HoriMove"), 0, player.GetAxis("VertMove")).normalized;
        dodgeTimer -= Time.deltaTime;

        if (poison > 0) { poison -= Time.deltaTime; }
        if (curseTimer <= 0) { LoseCurse(); }
        else { curseTimer -= Time.deltaTime; }

        aimTarget.position = transform.position + dir * 5;

        if (anim.GetCurrentAnimatorStateInfo(0).IsName("Idle") || anim.GetCurrentAnimatorStateInfo(0).IsName("Walking")) acting = false;

        bCooldown -= Time.deltaTime;

        switch (state)
        {
            case State.stun:
                break;

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
                    if (player.GetButtonDown("BAttack")) { BActionLock(); }
                    if (player.GetButtonDown("XAttack")) { XAction(); }
                    if (player.GetButtonDown("YAttack")) { YActionLock(); }

                    if (player.GetAxis("HoriMove") != 0 || player.GetAxis("VertMove") != 0) { anim.SetFloat("Movement", 1); }
                    else { anim.SetFloat("Movement", 0); }

                    anim.SetFloat("Movement_X", -Vector3.SignedAngle(dir.normalized, visuals.transform.forward.normalized, Vector3.up) * 0.09f);
                    anim.SetFloat("Movement_ZY", -Vector3.SignedAngle(dir.normalized, visuals.transform.forward.normalized, Vector3.up) * 0.09f);
                }

                aimTarget.LookAt(lookAtTarget.position + lookAtVariant);
                visuals.transform.forward = Vector3.Lerp(visuals.transform.forward, aimTarget.forward, 0.3f);

                break;

            case State.dodging:

                if (dodgeTimer < 0)
                {
                    DodgeSliding(dir);
                }
                break;

            case State.knockback:
                KnockbackContinual();
                break;
        }
    }

    #region X Attacks
    public override void XAction()
    {
        anim.SetTrigger("XAttack");
        basicMelee.GainInfo(xDamage, xKnockback, visuals.transform.forward, pvp, 0);
    }
    #endregion

    #region A Actions
    public override void AAction()
    {
        anim.SetTrigger("AAction");
    }
    public void DoTheTeleport()
    {
        transform.position += dir * vanishDistance;
    }
    #endregion

    #region Y Attacks
    private void YActionLock()
    {
        anim.SetTrigger("YAttackLock");
        if (Vector3.Distance(lookAtTarget.position, gameObject.transform.position) <= radiusOfStun)
        {
            lookAtTarget.GetComponentInParent<PlayerBase>().BecomeStunned(stunDur);
        }
    }
    public override void YAction()
    {
        anim.SetTrigger("YAttack");
        if (Vector3.Distance(lookAtTarget.position, gameObject.transform.position) <= radiusOfPull)
        {
            lookAtTarget.GetComponentInParent<PlayerBase>().Knockback(pullImpact, transform.position - lookAtTarget.position);
        }
    }
    #endregion

    #region B Attacks
    public override void BAction()
    {
        if (bCooldown < 0)
        {
            anim.SetTrigger("BAttack");
            explosionSphere.GainInfo(explosionDamage, explosionKnockback, lookAtTarget.position - transform.position, pvp, 0);
        }
    }
    public void BeginExplosion() { explosionSphere.gameObject.SetActive(true); Invoke("EndExplosion", explosionDur); explosionSphere.StartAttack(); }
    private void EndExplosion() { explosionSphere.gameObject.SetActive(false); explosionSphere.EndAttack(); }

    public void BActionLock()
    {
        if (bCooldown < 0)
        {
            anim.SetTrigger("BAttackLock");
            finalBeam.GainInfo(beamDamage, beamKnockback, visuals.transform.forward, pvp, 0);
        }
    }
    public void BeginBeam() { finalBeam.gameObject.SetActive(true); Invoke("EndBeam", beamDur); finalBeam.StartAttack(); }
    public void EndBeam() { finalBeam.gameObject.SetActive(false); finalBeam.EndAttack(); }
    #endregion
}