using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Carmen : PlayerBase
{
    [SerializeField] AudioClip ySoundBonus;

    [Header("Unique Components")]
    public Weapons backStabBox;
    [SerializeField] GameObject stabSymbol;

    [Header("Dagger Stab")]
    public int stabDamage;
    public int stabKnockback;

    [Header("Dash-Slash")]
    public int slashDamage;
    public int slashKnockback;
    [SerializeField] float slashKnockbackDuration;
    public float slashTravelDuration;
    [SerializeField] Weapons spinSphere;
    [SerializeField] float spinRadius;

    [Header("Grapple")]
    [SerializeField] float grapplingSpeed;
    [SerializeField] int bDamage;
    GrapplingTrap grapplingTrap;
    private Vector3 grappleDir;

    [Header("Backstab")]
    public int backstabAngle;
    public float backStabDamageMult;
    private GameObject enemyVisual;

    public override void SetInfo(UniverseController uni, int layerNew)
    {
        base.SetInfo(uni, layerNew);
        StartCoroutine(GainBack());
        spinSphere.GetComponent<SphereCollider>().radius = spinRadius;
        GetComponentInChildren<CarmenEventHandler>().NewStart();
    }

    IEnumerator GainBack()
    {
        yield return new WaitForEndOfFrame();
        ObjectPooler pooler = GameObject.FindGameObjectWithTag("ObjectPooler").GetComponent<ObjectPooler>();
        grapplingTrap = pooler.grapplerList[playerID].GetComponent<GrapplingTrap>();
        EndDodge();
    }

    public override void Update()
    {
        if (!isAI)
        {
            if (aTimer > 0) aTimer -= Time.deltaTime;
            if (bTimer > 0) bTimer -= Time.deltaTime;
            if (xTimer > 0) xTimer -= Time.deltaTime;
            if (yTimer > 0) yTimer -= Time.deltaTime;

            dir = new Vector3(player.GetAxis("HoriMove"), 0, player.GetAxis("VertMove"));
            if (dir != Vector3.zero)
                lastDir = dir;

            aimTarget.position = transform.position + (dir * 2) + lastDir;

            // if (anim.GetCurrentAnimatorStateInfo(0).IsName("Idle") || anim.GetCurrentAnimatorStateInfo(0).IsName("Walking")) acting = false;

            transform.position = new Vector3(transform.position.x, 0, transform.position.z);

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
                        if (player.GetButtonDown("AAction")) { AAction(true); }
                        if (player.GetButtonDown("BAttack") || Input.GetKeyDown(KeyCode.B)) { BAction(); }
                        if (player.GetButtonDown("XAttack")) { XAction(); }
                        if (player.GetButtonDown("YAttack")) { YAction(); }

                        anim.SetFloat("Movement", dir.magnitude + 0.001f);
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

                        if (player.GetButtonDown("AAction")) { AAction(true); }
                        if (player.GetButtonDown("BAttack")) { BAction(); }
                        if (player.GetButtonDown("XAttack")) { XAction(); }
                        if (player.GetButtonDown("YAttack")) { YAction(); }

                        /*
                        if (player.GetAxis("HoriMove") != 0 || player.GetAxis("VertMove") != 0) { anim.SetFloat("Movement", 1); }
                        else { anim.SetFloat("Movement", 0); }
                        */

                        anim.SetFloat("Movement", dir.magnitude + 0.001f);
                        anim.SetFloat("Movement_X", visuals.transform.InverseTransformDirection(rb2d.velocity).x / speed);
                        anim.SetFloat("Movement_ZY", visuals.transform.InverseTransformDirection(rb2d.velocity).z / speed);

                    }
                    aimTarget.LookAt(lockTargetList[currentLock].position + lookAtVariant);
                    visuals.transform.forward = Vector3.Lerp(visuals.transform.forward, aimTarget.forward, lockOnLerpSpeed);
                    LockOnScroll();

                    break;

                case State.dodging:

                    if (aTimer < 0)
                    {
                        DodgeSliding(visuals.transform.forward);
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
                    trueIFrames = true;
                    if (Vector3.Distance(transform.position, grappleDir) <= 2)
                    {
                        grapplingTrap.End();
                        anim.SetBool("Grappling", false);
                        trueIFrames = false;
                        EndActing();
                        state = State.normal;
                    }
                    break;
            }

        }
        else
        {
            AIUpdate();
        }
    }

    public override void OnHit(PlayerBase target, AttackType hitWith)
    {
        base.OnHit(target, hitWith);

        if (hitWith == AttackType.Y && stabSymbol.activeSelf)
        {
            PlaySound(ySoundBonus);
        }
    }

    public override void OnKill()
    {
        base.OnKill();
        stabSymbol.SetActive(false);
    }

    public override void OnVictory()
    {
        base.OnVictory();
        stabSymbol.SetActive(false);
    }

    public void LateUpdate()
    {
        float lookDif = Vector3.Angle(visuals.transform.forward, enemyVisual.transform.forward);

        if (Vector3.Distance(transform.position, lockTargetList[currentLock].position) <= 11 && lookDif <= backstabAngle && yTimer <= 0)
        {
            stabSymbol.SetActive(true);
        }
        else
        {
            stabSymbol.SetActive(false);
        }
    }

    public override void XAction()
    {
        if (xTimer <= 0)
        {
            base.XAction();

            anim.SetTrigger("XAttack");
            spinSphere.GainInfo(slashDamage, slashKnockback, visuals.transform.forward, pvp, 0, this, false, AttackType.X, slashKnockbackDuration);
            state = State.dodging;
            Invoke("StopKnockback", slashTravelDuration);

            xTimer = xCooldown;
            PlaySound(xSound);
        }
    }

    public override void YAction()
    {
        if (yTimer <= 0)
        {
            base.YAction();

            enemyVisual = lockTargetList[currentLock].parent.GetComponentInChildren<Animator>().gameObject;
            float lookDif = Vector3.Angle(visuals.transform.forward, enemyVisual.transform.forward);
            anim.SetTrigger("YAttack");

            yTimer = yCooldown;

            if (lookDif <= backstabAngle)
            {
                backStabBox.GainInfo(Mathf.RoundToInt(stabDamage * backStabDamageMult), stabKnockback, visuals.transform.forward, pvp, 0, this, true, AttackType.Y, 0);

                if (Vector3.Distance(transform.position, lockTargetList[currentLock].position) <= 11)
                {
                    PlayerBase target = enemyVisual.GetComponentInParent<PlayerBase>();
                    target.enabled = false;
                    Time.timeScale = 0.2f;
                    StartCoroutine(HideSymbol(0.5f, target));
                }
            }
            else
            {
                backStabBox.GainInfo(stabDamage, stabKnockback, visuals.transform.forward, pvp, 0, this, true, AttackType.Y, 0);
                PlaySound(ySound);
            }
        }
    }

    IEnumerator HideSymbol(float time, PlayerBase target)
    {
        yield return new WaitForSecondsRealtime(time);
        Time.timeScale = 1;
        target.enabled = true;
    }

    public override void BAction()
    {
        if (bTimer <= 0)
        {
            base.BAction();

            anim.SetTrigger("BAttack");

            grapplingTrap.gameObject.transform.position = transform.position + new Vector3(0, 5, 0);
            grapplingTrap.transform.eulerAngles = transform.forward;

            grapplingTrap.gameObject.SetActive(true);

            grapplingTrap.OnThrow(visuals.transform.forward, this, playerID + 13, bDamage);

            bTimer = bCooldown;
            PlaySound(bSound);
        }
    }

    public void GetLocation(Vector3 dirTemp)
    {
        grappleDir = dirTemp - new Vector3(0, 5, 0);
        BeginActing();
        state = State.unique;
    }

    public override void StopKnockback()
    {
        base.StopKnockback();
        anim.SetBool("Grappling", false);
        hazardFrames = false;
    }

    public override void TakeDamage(int damageInc, Vector3 dirTemp, int knockback, bool fromAttack, bool stopAttack, PlayerBase attacker, float knockbackDur)
    {
        anim.SetBool("Grappling", false);
        base.TakeDamage(damageInc, dirTemp, knockback, fromAttack, stopAttack, attacker, knockbackDur);
    }

    public override void Death(PlayerBase killer)
    {
        base.Death(killer);
        stabSymbol.SetActive(false);
    }

    public override void Respawn()
    {
        base.Respawn();
        EndDodge();
        anim.SetBool("Grappling", false);
    }
}
