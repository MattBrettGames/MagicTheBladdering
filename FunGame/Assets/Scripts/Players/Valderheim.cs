using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Valderheim : PlayerBase
{

    [Header("More Componenets")]
    public Weapons weapon;

    [Header("Wide Swing")]
    public int xAttack;
    public int xKnockback;

    [Header("Ground Slam")]
    public int movingAttack;
    public int movingKnockback;
    public int statAttack;
    public int statKnockback;

    [Header("Frenzy")]
    public int frenzyDuration;
    public int frenzyMult;

    public override void XAction()
    {
        weapon.GainInfo(xAttack, xKnockback, visuals.transform.forward);
        anim.SetTrigger("XAttack");
    }

    public override void YAction()
    {
        if (moving != 0)
        {
            weapon.GainInfo(movingAttack, movingKnockback, visuals.transform.forward);
            anim.SetTrigger("YAttack");
        }
        else
        {
            weapon.GainInfo(statAttack, statKnockback, visuals.transform.forward);
            print("Stationary Hammer Slam!");
            anim.SetTrigger("YAttack");
        }
    }

    public override void BAction()
    {
        Invoke("StopFrenzy", frenzyDuration);
        damageMult = frenzyMult;
        incomingMult = frenzyMult;
    }

    private void StopFrenzy()
    {
        damageMult = 1;
        incomingMult = 1;
    }


    //Passive Effects - Surefooted & Building Rage
    public override void KnockedDown(int power) { }
    public override void HealthChange(int healthChange) { base.HealthChange(healthChange); damageMult = (healthMax - currentHealth) / 10; }

}
