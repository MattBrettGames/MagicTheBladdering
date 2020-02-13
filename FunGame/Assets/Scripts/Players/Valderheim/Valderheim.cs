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
    private float overheadStun;
    [SerializeField] int ySpeedDebuff = 10;

    [Header("Kick Up")]
    public int kickAttack;
    public int kickKnockback;

    [Header("Frenzy")]
    public int frenzyDuration;
    public int frenzyBonus;
    public int frenzySpeedBuff;
    private bool frenzy;
    public GameObject frenzyEffects;
    float dodgeDurDefault;
    float dodgeSpeedDefault;

    [Header("Passives")]
    [SerializeField] private float comboTimeDur;
    [SerializeField] private bool comboTime;

    //[Header("Polish")]
    //[SerializeField] Color skinColour;
    GameObject crack1;
    GameObject crack2;

    public override void Start()
    {
        base.Start();
        hammer.gameObject.tag = tag;
        dodgeDurDefault = dodgeDur;
        dodgeSpeedDefault = dodgeSpeed;
    }

    public override void SetInfo(UniverseController uni, int layerNew)
    {
        base.SetInfo(uni, layerNew);

        ObjectPooler objectPooler = GameObject.FindGameObjectWithTag("ObjectPooler").GetComponent<ObjectPooler>();
        crack1 = objectPooler.crackList[playerID * 2];
        crack2 = objectPooler.crackList[(playerID * 2) + 1];
    }


    public override void Update()
    {
        //This bit is the controls
        if (!isAI)
        {


            if (player.GetButtonDown("BAttack") || Input.GetKeyDown(KeyCode.B)) { BAction(); }

            if (aTimer > 0) aTimer -= Time.deltaTime;
            if (bTimer > 0) bTimer -= Time.deltaTime;
            if (xTimer > 0) xTimer -= Time.deltaTime;
            if (yTimer > 0) yTimer -= Time.deltaTime;

            dir = new Vector3(player.GetAxis("HoriMove"), 0, player.GetAxis("VertMove"));

            aimTarget.position = transform.position + dir * 5;

            if (anim.GetCurrentAnimatorStateInfo(0).IsName("Idle") || anim.GetCurrentAnimatorStateInfo(0).IsName("Walking")) acting = false;

            transform.position = new Vector3(transform.position.x, 0, transform.position.z);

            switch (state)
            {
                case State.attack:
                    break;

                case State.normal:

                    anim.SetBool("LockOn", false);
                    if (player.GetAxis("LockOn") >= 0.4f) { state = State.lockedOn; }

                    if (!acting)
                    {
                        //Rotating the Character Model
                        visuals.transform.LookAt(aimTarget);
                        rb2d.velocity = dir * (speed + bonusSpeed);

                        //Standard Inputs
                        if (player.GetButtonDown("AAction")) { AAction(true); }
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
                        rb2d.velocity = dir * (speed + bonusSpeed);

                        if (player.GetButtonDown("AAction")) { AAction(true); }
                        if (player.GetButtonDown("XAttack")) { XAction(); }
                        if (player.GetButtonDown("YAttack")) { YAction(); }
                        /*
                        if (player.GetAxis("HoriMove") != 0 || player.GetAxis("VertMove") != 0) { anim.SetFloat("Movement", 1); }
                        else { anim.SetFloat("Movement", 0); }
                        */

                        anim.SetFloat("Movement", dir.magnitude + 0.001f);
                        anim.SetFloat("Movement_X", visuals.transform.InverseTransformDirection(rb2d.velocity).x / speed);
                        anim.SetFloat("Movement_ZY", visuals.transform.InverseTransformDirection(rb2d.velocity).z / speed);

                        aimTarget.LookAt(lockTargetList[currentLock].position + lookAtVariant);
                        visuals.transform.forward = Vector3.Lerp(visuals.transform.forward, aimTarget.forward, lockOnLerpSpeed);
                        LockOnScroll();
                    }

                    break;

                case State.dodging:
                    if (aTimer <= 0) DodgeSliding(dir);
                    break;

                case State.knockback:
                    KnockbackContinual();
                    break;
            }
        }

        // This bit is the AI
        else
        {
            aimTarget.transform.position = lockTargetList[currentLock].position + lookAtVariant;

            AILogic();

            if (aTimer > 0) aTimer -= Time.deltaTime;
            if (bTimer > 0) bTimer -= Time.deltaTime;
            if (xTimer > 0) xTimer -= Time.deltaTime;
            if (yTimer > 0) yTimer -= Time.deltaTime;

            dir = visuals.transform.forward;

            if (anim.GetCurrentAnimatorStateInfo(0).IsName("Idle") || anim.GetCurrentAnimatorStateInfo(0).IsName("Walking")) acting = false;

            transform.position = new Vector3(transform.position.x, 0, transform.position.z);

            switch (state)
            {
                case State.attack:
                    break;

                case State.normal:

                    anim.SetBool("LockOn", false);
                    if (player.GetAxis("LockOn") >= 0.4f) { state = State.lockedOn; }

                    if (!acting)
                    {
                        //Rotating the Character Model
                        visuals.transform.LookAt(aimTarget);
                        rb2d.velocity = dir * (speed + bonusSpeed);

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
                        rb2d.velocity = dir * (speed + bonusSpeed);


                        anim.SetFloat("Movement", dir.magnitude + 0.001f);
                        anim.SetFloat("Movement_X", visuals.transform.InverseTransformDirection(rb2d.velocity).x / speed);
                        anim.SetFloat("Movement_ZY", visuals.transform.InverseTransformDirection(rb2d.velocity).z / speed);

                        aimTarget.LookAt(lockTargetList[currentLock].position + lookAtVariant);
                        visuals.transform.forward = Vector3.Lerp(visuals.transform.forward, aimTarget.forward, lockOnLerpSpeed);
                        LockOnScroll();
                    }

                    break;

                case State.dodging:
                    if (aTimer <= 0) DodgeSliding(dir);
                    break;

                case State.knockback:
                    KnockbackContinual();
                    break;
            }
        }
    }

    public override void XAction()
    {
        if (!comboTime)
        {
            if (xTimer <= 0)
            {
                base.XAction();

                hammer.GainInfo(Mathf.RoundToInt(xAttack * damageMult), Mathf.RoundToInt(xKnockback * damageMult), visuals.transform.forward, pvp, 0, this, true);
                anim.SetTrigger("XAttack");
                xTimer = xCooldown;
                PlaySound(xSound);
            }
        }
        else
        {
            hammer.GainInfo(Mathf.RoundToInt(spinDamage * damageMult), Mathf.RoundToInt(spinKnockback * damageMult), visuals.transform.forward, pvp, 0, this, true);
            anim.SetTrigger("Spin");
            PlaySound(xSound);
        }
    }

    public override void YAction()
    {
        if (comboTime)
        {
            hammer.GainInfo(Mathf.RoundToInt(kickAttack * damageMult), Mathf.RoundToInt(kickKnockback * damageMult), visuals.transform.forward, pvp, 0, this, true);
            anim.SetTrigger("ComboKick");
            comboTime = false;
            PlaySound(ySound);
        }
        else
        {
            if (yTimer <= 0)
            {
                base.YAction();

                hammer.GainInfo(Mathf.RoundToInt(slamAttack * damageMult), Mathf.RoundToInt(slamKnockback * damageMult), visuals.transform.forward, pvp, overheadStun, this, true);
                anim.SetTrigger("YAttack");
                yTimer = yCooldown;
                PlaySound(ySound);
            }
        }
    }
    public void OpenComboKick() { comboTime = true; outline.OutlineColor = new Color(1, 1, 1); StartCoroutine(CallCloseCombo()); }
    IEnumerator CallCloseCombo() { yield return new WaitForSeconds(comboTimeDur); CloseComboKick(); }

    public void BeginSlow() { bonusSpeed -= ySpeedDebuff; Invoke("EndSlow", 1); }
    public void EndSlow() { bonusSpeed += ySpeedDebuff; }

    private void CloseComboKick()
    {
        comboTime = false;
        anim.ResetTrigger("ComboKick");
        anim.ResetTrigger("Spin");
        outline.OutlineColor = new Color(0, 0, 0);
    }

    public override void Death(PlayerBase killer)
    {
        bonusSpeed = 0;
        base.Death(killer);
        StopCoroutine(StopFrenzy());
        dodgeDur -= 0.2f;
        dodgeSpeed -= 5;
        frenzy = false;
        frenzyEffects.SetActive(false);
    }
    public override void Respawn()
    {
        CloseComboKick();
        base.Respawn();
        bonusSpeed = 0;
        dodgeDur = dodgeDurDefault;
        dodgeSpeed = dodgeSpeedDefault;
    }

    public override void LeaveCrack(Vector3 pos)
    {
        crack1.transform.position = new Vector3(pos.x, 0.4f, pos.z);
        crack1.transform.eulerAngles = new Vector3(0, Random.Range(0f, 359f), 0);

        if (frenzy)
        {
            crack1.transform.localScale = new Vector3(1f, 1f, 1f);
        }
        else
        {
            crack1.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
        }

        crack1.SetActive(true);
        StartCoroutine(EndCrack(crack1));
    }
    IEnumerator EndCrack(GameObject crack)
    {
        yield return new WaitForSeconds(2);
        crack.SetActive(false);
    }
    public void LeaveCrack(Vector3 pos, bool isCrack)
    {
        crack2.transform.position = new Vector3(pos.x, 0.4f, pos.z);
        crack2.transform.eulerAngles = new Vector3(0, Random.Range(0f, 359f), 0);

        if (frenzy)
        {
            crack2.transform.localScale = new Vector3(1f, 1f, 1f);
        }
        else
        {
            crack2.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
        }

        crack2.SetActive(true);
        StartCoroutine(EndCrack(crack2));
    }

    public override void BAction()
    {
        if (!frenzy && bTimer <= 0)
        {
            base.BAction();

            StopCoroutine(EndCrack(crack2));
            //LeaveCrack(transform.position, true);
            StartCoroutine(StopFrenzy());
            anim.SetTrigger("BAttack");

            bonusSpeed += frenzySpeedBuff;
            damageMult += frenzyBonus;
            incomingMult += frenzyBonus;
            frenzy = true;
            dodgeDur += 0.2f;
            dodgeSpeed += 5;

            frenzyEffects.SetActive(true);
            bTimer = bCooldown;

            PlaySound(bSound);
        }
    }
    private IEnumerator StopFrenzy()
    {
        yield return new WaitForSeconds(frenzyDuration);

        damageMult -= frenzyBonus;
        incomingMult -= frenzyBonus;

        dodgeDur -= 0.2f;
        dodgeSpeed -= 5;
        frenzy = false;
        frenzyEffects.SetActive(false);

        bonusSpeed -= frenzySpeedBuff;
    }
}