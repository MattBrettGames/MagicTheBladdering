using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Carmen : PlayerBase
{

    [SerializeField] string ySoundBonus;

    [Header("Unique Components")]
    public Weapons leftDagger;

    [Header("Dagger Stab")]
    public int stabDamage;
    public int stabKnockback;

    [Header("Dash-Slash")]
    public int slashDamage;
    public int slashKnockback;
    public float slashTravelDuration;
    [SerializeField] Weapons spinSphere;
    [SerializeField] float spinRadius;

    [Header("Grapple")]
    [SerializeField] float grapplingSpeed;
    GrapplingTrap grapplingTrap;

    [Header("Backstab")]
    public int backstabAngle;
    public float backStabDamageMult;
    private GameObject enemyVisual;
    private Vector3 grappleDir;

    public override void SetInfo(UniverseController uni, int layerNew)
    {
        base.SetInfo(uni, layerNew);
        StartCoroutine(GainBack());
        spinSphere.GetComponent<SphereCollider>().radius = spinRadius;
    }

    IEnumerator GainBack()
    {
        yield return new WaitForEndOfFrame();
        ObjectPooler pooler = GameObject.FindGameObjectWithTag("ObjectPooler").GetComponent<ObjectPooler>();
        grapplingTrap = pooler.grapplerList[playerID].GetComponent<GrapplingTrap>();

        enemyVisual = lookAtTarget.parent.GetComponentInChildren<Animator>().gameObject;
    }


    public override void Update()
    {
        if (aTimer > 0) aTimer -= Time.deltaTime;
        if (bTimer > 0) bTimer -= Time.deltaTime;
        if (xTimer > 0) xTimer -= Time.deltaTime;
        if (yTimer > 0) yTimer -= Time.deltaTime;

        dir = new Vector3(player.GetAxis("HoriMove"), 0, player.GetAxis("VertMove"));

        aimTarget.position = transform.position + dir * 5;

        if (anim.GetCurrentAnimatorStateInfo(0).IsName("Idle") || anim.GetCurrentAnimatorStateInfo(0).IsName("Walking")) acting = false;

        switch (state)
        {
            case State.stun:
                anim.SetBool("Stunned", true);
                break;

            case State.attack:
                break;

            case State.normal:

                anim.SetBool("LockOn", false);
                if (player.GetAxis("LockOn") >= 0.4f) { state = State.lockedOn; }

                if (!acting)
                {
                    //Rotating the Character Model
                    visuals.transform.LookAt(aimTarget);
                    rb2d.velocity = dir * speed;

                    //Standard Inputs
                    if (player.GetButtonDown("AAction")) { AAction(); }
                    if (player.GetButtonDown("BAttack")) { BAction(); }
                    if (player.GetButtonDown("XAttack")) { XAction(); }
                    if (player.GetButtonDown("YAttack")) { YAction(); }

                    anim.SetFloat("Movement", Mathf.Abs((player.GetAxis("HoriMove") + player.GetAxis("VertMove")) * 0.5f));
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

                if (!acting)
                {
                    rb2d.velocity = dir * speed;

                    if (player.GetButtonDown("AAction")) { AAction(); }
                    if (player.GetButtonDown("BAttack")) { BAction(); }
                    if (player.GetButtonDown("XAttack")) { XAction(); }
                    if (player.GetButtonDown("YAttack")) { YAction(); }

                    if (player.GetAxis("HoriMove") != 0 || player.GetAxis("VertMove") != 0) { anim.SetFloat("Movement", 1); }
                    else { anim.SetFloat("Movement", 0); }

                    anim.SetFloat("Movement_X", transform.InverseTransformDirection(rb2d.velocity).x / speed);
                    anim.SetFloat("Movement_ZY", transform.InverseTransformDirection(rb2d.velocity).z / speed);

                    aimTarget.LookAt(lookAtTarget.position + lookAtVariant);
                    visuals.transform.forward = Vector3.Lerp(visuals.transform.forward, aimTarget.forward, 0.3f);
                }


                break;

            case State.dodging:

                if (aTimer < 0)
                {
                    DodgeSliding(dir);
                }
                break;

            case State.knockback:
                KnockbackContinual();
                break;

            case State.unique:
                visuals.transform.LookAt(grappleDir);
                DodgeSliding(visuals.transform.forward * grapplingSpeed);
                anim.SetBool("Grappling", true);
                CancelInvoke();
                if (Vector3.Distance(transform.position, grappleDir) <= 2)
                {
                    anim.SetBool("Grappling", false);
                    grapplingTrap.End();
                    EndActing();
                    state = State.normal;
                }
                break;

        }

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            if (thisPlayer == "P2")
            {
                TakeDamage(3000, true, false);
            }
        }
    }


    public override void XAction()
    {
        if (xTimer <= 0)
        {
            anim.SetTrigger("XAttack");
            spinSphere.GainInfo(slashDamage, slashKnockback, visuals.transform.forward, pvp, 0, this, false);
            state = State.dodging;
            Invoke("StopKnockback", slashTravelDuration);

            xTimer = xCooldown;
            universe.PlaySound(xSound);
        }
    }

    public override void YAction()
    {
        if (yTimer <= 0)
        {
            float lookDif = Vector3.Angle(visuals.transform.forward, enemyVisual.transform.forward);
            anim.SetTrigger("YAttack");

            yTimer = yCooldown;

            if (lookDif <= backstabAngle)
            {
                leftDagger.GainInfo(Mathf.RoundToInt(stabDamage * backStabDamageMult), stabKnockback, visuals.transform.forward, pvp, 0, this);
                universe.PlaySound(ySoundBonus);
            }
            else
            {
                leftDagger.GainInfo(stabDamage, stabKnockback, visuals.transform.forward, pvp, 0, this);
                universe.PlaySound(ySound);
            }
        }
    }

    public override void BAction()
    {
        if (bTimer <= 0)
        {
            grapplingTrap.gameObject.transform.position = transform.position + new Vector3(0, 5, 0);
            grapplingTrap.transform.eulerAngles = transform.forward;

            grapplingTrap.gameObject.SetActive(true);

            grapplingTrap.OnThrow(visuals.transform.forward, this, playerID + 13);

            bTimer = bCooldown;
            universe.PlaySound(bSound);
        }
    }

    public void GetLocation(Vector3 dirTemp)
    {
        grappleDir = dirTemp - new Vector3(0, 5, 0);
        state = State.unique;
        BeginActing();
    }

    public override void StopKnockback()
    {
        base.StopKnockback();
        anim.SetBool("Grappling", false);
    }

    public override void Respawn()
    {
        base.Respawn();
        anim.SetBool("Grappling", false);
    }


}