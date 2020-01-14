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

    [Header("Polish")]
    [SerializeField] Color skinColour;
    GameObject crack;

    public override void Start()
    {
        base.Start();
        hammer.gameObject.tag = tag;
    }

    public override void SetInfo(UniverseController uni, int layerNew)
    {
        base.SetInfo(uni, layerNew);

        ObjectPooler  objectPooler = GameObject.FindGameObjectWithTag("ObjectPooler").GetComponent<ObjectPooler>();
        crack = objectPooler.crackList[playerID];
        crack.GetComponent<Material>().SetColor("_EmissionColor", skinColour);

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

        if (player.GetButtonDown("BAttack")) { BAction(); }

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
                    rb2d.velocity = dir * speed;

                    //Standard Inputs
                    if (player.GetButtonDown("AAction")) { AAction(); }
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
                hammer.GainInfo(Mathf.RoundToInt(xAttack * damageMult), Mathf.RoundToInt(xKnockback * damageMult), visuals.transform.forward, pvp, 0, this, true);
                anim.SetTrigger("XAttack");
                xTimer = xCooldown;
                universe.PlaySound(xSound);
            }
        }
        else
        {
            hammer.GainInfo(Mathf.RoundToInt(spinDamage * damageMult), Mathf.RoundToInt(spinKnockback * damageMult), visuals.transform.forward, pvp, 0, this, true);
            anim.SetTrigger("Spin");
            universe.PlaySound(xSound);
        }
    }

    public override void YAction()
    {
        if (comboTime)
        {
            hammer.GainInfo(Mathf.RoundToInt(kickAttack * damageMult), Mathf.RoundToInt(kickKnockback * damageMult), visuals.transform.forward, pvp, 0, this, true);
            anim.SetTrigger("ComboKick");
            comboTime = false;
            universe.PlaySound(ySound);
        }
        else
        {
            if (yTimer <= 0)
            {
                hammer.GainInfo(Mathf.RoundToInt(slamAttack * damageMult), Mathf.RoundToInt(slamKnockback * damageMult), visuals.transform.forward, pvp, overheadStun, this, true);
                anim.SetTrigger("YAttack");
                yTimer = yCooldown;
                universe.PlaySound(ySound);
            }
        }
    }
    public void OpenComboKick() { comboTime = true; outline.OutlineColor = new Color(1, 1, 1); Invoke("CloseComboKick", comboTimeDur); }
    private void CloseComboKick() { comboTime = false; outline.OutlineColor = new Color(0, 0, 0); }

    public override void LeaveCrack(Vector3 pos)
    {
        crack.transform.position = pos;
        crack.transform.eulerAngles = new Vector3(0, Random.Range(0f,359f), 0);
        crack.SetActive(true);
        StartCoroutine(EndCrack());
    }
    IEnumerator EndCrack()
    {
        yield return new WaitForSeconds(7);
        crack.SetActive(false);
    }

    public override void BAction()
    {
        if (!frenzy && bTimer <= 0)
        {

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

            universe.PlaySound(bSound);
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