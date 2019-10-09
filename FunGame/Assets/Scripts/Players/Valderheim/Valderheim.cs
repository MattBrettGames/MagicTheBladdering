using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Valderheim : PlayerBase
{
    [Header("More Componenets")]
    public Weapons hammer;
    public Weapons chest;

    [Header("Wide Swing")]
    public int xAttack;
    public int xKnockback;

    [Header("Ground Slam")]
    public int slamAttack;
    public int slamKnockback;
    public int kickAttack;
    public int kickKnockback;
    private bool comboTime;

    [Header("Frenzy")]
    public int frenzyDuration;
    public int frenzyBonus;
    private bool frenzy;

    [Header("Charge!")]
    public float speedMult;
    private float currentSpeed;
    private bool charging;


    public override void XAction()
    {
        print("This woud deal " + Mathf.RoundToInt(xAttack * damageMult) + " damage");
        hammer.GainInfo(Mathf.RoundToInt(xAttack * damageMult), Mathf.RoundToInt(xKnockback * damageMult), visuals.transform.forward);
        anim.SetTrigger("XAttack");
    }

    public override void YAction()
    {
        if (comboTime)
        {
            hammer.GainInfo(kickAttack, kickKnockback, visuals.transform.forward);
            anim.SetBool("Comboing", true);
            print("Combo Kick!");
        }
        else
        {
            anim.SetBool("Comboing", false);
            hammer.GainInfo(slamAttack, slamKnockback, visuals.transform.forward);
            print("Hammer Slam!");
            anim.SetTrigger("YAttack");
        }
    }
    public void OpenComboKick() { comboTime = true; }
    public void CloseComboKick() { comboTime = false; }

    public override void BAction()
    {
        if (!frenzy)
        {
            Invoke("StopFrenzy", frenzyDuration);
            anim.SetTrigger("BAttack");
            damageMult += frenzyBonus;
            incomingMult += frenzyBonus;
            frenzy = true;
        }
    }
    private void StopFrenzy()
    {
        damageMult -= frenzyBonus;
        incomingMult -= frenzyBonus;
        frenzy = false;
    }

    public override void AAction()
    {
        if (!charging)
        {
            float hori = Input.GetAxis(horiPlayerInput);
            float vert = Input.GetAxis(vertPlayerInput);
            anim.SetTrigger("AAction");
            currentSpeed = speed;
            speed /= speedMult;
            charging = true;
        }

    }
    public void BeginCharge() { anim.SetBool("Charging", true); }


    public override void FixedUpdate()
    {
        if (Input.GetKeyDown(KeyCode.Space) && thisPlayer == "P2") { TakeDamage(15); }
        if (!prone)
        {
            if (!charging)
            {
                float hori = Input.GetAxis(horiPlayerInput);
                float vert = Input.GetAxis(vertPlayerInput);

                //Rotating the Character Model
                aimTarget.position = transform.position + new Vector3(hori, 0, vert).normalized;
                visuals.transform.LookAt(aimTarget);

                //Standard Inputs
                if (Input.GetButtonDown(aPlayerInput)) { AAction(); }
                if (Input.GetButtonDown(bPlayerInput)) { BAction(); }
                if (Input.GetButtonDown(xPlayerInput)) { XAction(); }
                if (Input.GetButtonDown(yPlayerInput)) { YAction(); }

                if (Input.GetAxis(horiPlayerInput) != 0 || Input.GetAxis(vertPlayerInput) != 0) { anim.SetFloat("Movement", 1); }
                else { anim.SetFloat("Movement", 0); }
            }

            transform.position = Vector3.Lerp(transform.position, aimTarget.position, speed);
            //transform.Translate(new Vector3(hori, 0, vert).normalized * speed);

        }
        if (charging)
        {
            transform.position = Vector3.Lerp(transform.position, aimTarget.position, speed);
        }
        if (Input.GetButtonUp(thisPlayer + "AButton"))
        {
            speed = currentSpeed;
            charging = false;
            anim.SetBool("Charging", false);
            anim.SetTrigger("EndCharge");
        }

        if (poison > 0) { poison -= Time.deltaTime; }
        if (curseTimer <= 0) { LoseCurse(); }
        else { curseTimer -= Time.deltaTime; }
    }
    

    //Passive Effects - Surefooted & Building Rage
    public override void KnockedDown(int power) { }
    public override void HealthChange(int healthChange) { base.HealthChange(healthChange); damageMult = (healthMax - currentHealth) / 10; }

}