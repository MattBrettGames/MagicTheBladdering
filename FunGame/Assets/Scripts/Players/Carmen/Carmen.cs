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
    }

    public override void XAction()
    {
        Debug.Log("XPRESS");
        anim.SetTrigger("XAttack");
        leftDagger.GainInfo(slashDamage, slashKnockback, visuals.transform.forward, pvp, 0);
        rightDagger.GainInfo(slashDamage, slashKnockback, visuals.transform.forward, pvp, 0);
        state = State.dodging;
        Invoke("StopKnockback", slashTravelDuration);
    }

    public override void YAction()
    {
        float lookDif = Vector3.Angle(visuals.transform.forward, enemyVisual.transform.forward);

        if (lookDif <= backstabAngle)
        {
            leftDagger.GainInfo(Mathf.RoundToInt(stabDamage * backStabDamageMult), stabKnockback, visuals.transform.forward, pvp, 0);
            rightDagger.GainInfo(Mathf.RoundToInt(stabDamage * backStabDamageMult), stabKnockback, visuals.transform.forward, pvp, 0);
        }
        else
        {
            leftDagger.GainInfo(stabDamage, stabKnockback, visuals.transform.forward, pvp, 0);
            rightDagger.GainInfo(stabDamage, stabKnockback, visuals.transform.forward, pvp, 0);
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