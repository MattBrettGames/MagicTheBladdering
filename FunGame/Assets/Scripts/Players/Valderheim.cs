using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Valderheim : PlayerBase
{

    [Header("More Componenets")]
    public Weapons weapon;

    [Header("Wide Swing")]
    public int xAttack;

    [Header("Ground Slam")]
    public int yAttack;

    [Header("Frenzy")]
    public int bAttack;
    public int frenzyDuration;


    public override void XAction()
    {
        anim.SetTrigger("XAttack");
        weapon.GainInfo(xAttack);
    }

    public override void YAction()
    {
        if (moving != 0)
        {
            anim.SetTrigger("YAttack");
        }
        else
        {
            print("Stationary Hammer Slam!");
            anim.SetTrigger("YAttack");
        }
    }

    public override void BAction()
    {
        Invoke("StopFrenzy", frenzyDuration);
        damageMult = 2;
        incomingMult = 2;
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
