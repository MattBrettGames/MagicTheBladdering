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
    [SerializeField] float xKnockbackDuration;

    [Header("Spin")]
    public int spinDamage;
    public int spinKnockback;
    [SerializeField] float spinKnockbackDuration;

    [Header("Ground Slam")]
    public int slamAttack;
    public int slamKnockback;
    private float overheadStun;
    [SerializeField] int ySpeedDebuff = 10;
    [SerializeField] float slamKnockbackDuration;
    bool canInput;

    [Header("Kick Up")]
    public int kickAttack;
    public int kickKnockback;
    [SerializeField] float kickKnockbackDuration;

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
    [SerializeField] GameObject crackEffect;

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
        crackEffect = Instantiate(crackEffect);
        crackEffect.SetActive(false);
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
            if (dir != Vector3.zero)
                lastDir = dir;

            aimTarget.position = transform.position + (dir * 2) + lastDir;

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
            AIUpdate();
        }
    }

    public override void XAction()
    {
        if (!comboTime)
        {
            if (xTimer <= 0 && canInput)
            {
                base.XAction();

                canInput = false;
                hammer.GainInfo(Mathf.RoundToInt(xAttack * damageMult), Mathf.RoundToInt(xKnockback * damageMult), visuals.transform.forward, pvp, 0, this, true, AttackType.X, xKnockbackDuration);
                anim.SetTrigger("XAttack");
                xTimer = xCooldown;
                PlaySound(xSound);
            }
        }
        else
        {
            canInput = false;
            hammer.GainInfo(Mathf.RoundToInt(spinDamage * damageMult), Mathf.RoundToInt(spinKnockback * damageMult), visuals.transform.forward, pvp, 0, this, true, AttackType.X, spinKnockbackDuration);
            anim.SetTrigger("Spin");
            PlaySound(xSound);
        }
    }

    public override void YAction()
    {
        if (!comboTime)
        {
            if (yTimer <= 0 && canInput)
            {
                canInput = false;
                base.YAction();

                hammer.GainInfo(Mathf.RoundToInt(slamAttack * damageMult), Mathf.RoundToInt(slamKnockback * damageMult), visuals.transform.forward, pvp, overheadStun, this, true, AttackType.Y, slamKnockbackDuration);
                anim.SetTrigger("YAttack");
                yTimer = yCooldown;
                PlaySound(ySound);
            }
        }
        else
        {
            canInput = false;
            hammer.GainInfo(Mathf.RoundToInt(kickAttack * damageMult), Mathf.RoundToInt(kickKnockback * damageMult), visuals.transform.forward, pvp, 0, this, true, AttackType.Y, kickKnockbackDuration);
            anim.SetTrigger("ComboKick");
            comboTime = false;
            PlaySound(ySound);
        }
    }
    public void OpenComboKick() { comboTime = true; outline.OutlineColor = new Color(1, 1, 1); StartCoroutine(CallCloseCombo()); }
    IEnumerator CallCloseCombo() { yield return new WaitForSeconds(comboTimeDur); CloseComboKick(); }

    public void BeginSlow() { bonusSpeed -= ySpeedDebuff; Invoke("EndSlow", 1);  canInput = false; }
    public void EndSlow() { bonusSpeed += ySpeedDebuff; }

    public override void EndActing()
    {
        canInput = true;
        base.EndActing();
    }

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
        crackEffect.transform.position = new Vector3(pos.x, 0.4f, pos.z);
        crackEffect.transform.eulerAngles = new Vector3(0, Random.Range(0f, 359f), 0);

        if (frenzy)
        {
            crackEffect.transform.localScale = new Vector3(1f, 1f, 1f);
        }
        else
        {
            crackEffect.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
        }

        crackEffect.SetActive(true);
        StartCoroutine(EndCrack(crackEffect));
    }
    IEnumerator EndCrack(GameObject crack)
    {
        yield return new WaitForSeconds(2);
        crack.SetActive(false);
    }
    

    public override void BAction()
    {
        if (!frenzy && bTimer <= 0)
        {
            base.BAction();

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