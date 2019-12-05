using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Carmen : PlayerBase
{

    [Header("Unique Components")]
    public Weapons leftDagger;
    public Weapons rightDagger;

    [Header("Dagger Stab")]
    public int stabDamage;
    public int stabKnockback;

    [Header("Dash-Slash")]
    public int slashDamage;
    public int slashKnockback;
    public float slashTravelDuration;

    [Header("Dig")]
    public int digDistance;

    [Header("Backstab")]
    public int backstabAngle;
    public float backStabDamageMult;
    private GameObject enemyVisual;

    public override void SetInfo()
    {
        base.SetInfo();
        StartCoroutine(GainBack());
    }

    IEnumerator GainBack()
    {
        yield return new WaitForEndOfFrame();
        enemyVisual = lookAtTarget.parent.GetComponentInChildren<Animator>().gameObject;
        print(enemyVisual);
    }

    public override void XAction()
    {
        anim.SetTrigger("XAttack");
        leftDagger.GainInfo(slashDamage, slashKnockback, visuals.transform.forward, pvp);
        rightDagger.GainInfo(slashDamage, slashKnockback, visuals.transform.forward, pvp);
        state = State.dodging;
        Invoke("StopKnockback", slashTravelDuration);
    }

    public override void YAction()
    {
        float lookDif = Vector3.Angle(visuals.transform.forward, enemyVisual.transform.forward);

        if (lookDif <= backstabAngle)
        {
            leftDagger.GainInfo(Mathf.RoundToInt(stabDamage * backStabDamageMult), stabKnockback, visuals.transform.forward, pvp);
            rightDagger.GainInfo(Mathf.RoundToInt(stabDamage * backStabDamageMult), stabKnockback, visuals.transform.forward, pvp);
        }
        else
        {
            leftDagger.GainInfo(stabDamage, stabKnockback, visuals.transform.forward, pvp);
            rightDagger.GainInfo(stabDamage, stabKnockback, visuals.transform.forward, pvp);
        }
        anim.SetTrigger("YAttack");
    }

    public override void BAction()
    {
        anim.SetTrigger("BAttack");
    }
    public void DigTravel()
    {
        transform.position += dir * digDistance;
    }

}