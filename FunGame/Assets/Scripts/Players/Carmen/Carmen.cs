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
    [SerializeField] float yCooldown;
    float yTimer;

    [Header("Dash-Slash")]
    public int slashDamage;
    public int slashKnockback;
    public float slashTravelDuration;
    [SerializeField] float xCooldown;
    float xTimer;

    [Header("Dig")]
    public int digDistance;
    [SerializeField] float bCooldown;
    float btimer;

    [Header("Backstab")]
    public int backstabAngle;
    public float backStabDamageMult;
    private GameObject enemyVisual;

    public override void SetInfo()
    {
        base.SetInfo();
        StartCoroutine(GainBack());
    }

    public override void Update()
    {
        base.Update();
        yTimer -= Time.deltaTime;
        xTimer -= Time.deltaTime;
        btimer -= Time.deltaTime;
    }

    IEnumerator GainBack()
    {
        yield return new WaitForEndOfFrame();
        enemyVisual = lookAtTarget.parent.GetComponentInChildren<Animator>().gameObject;
    }

    public override void XAction()
    {
        if (xTimer < 0)
        {
            anim.SetTrigger("XAttack");
            leftDagger.GainInfo(slashDamage, slashKnockback, visuals.transform.forward, pvp, 0);
            rightDagger.GainInfo(slashDamage, slashKnockback, visuals.transform.forward, pvp, 0);
            state = State.dodging;
            Invoke("StopKnockback", slashTravelDuration);

            xTimer = xCooldown;
        }
    }

    public override void YAction()
    {
        if (yTimer < 0)
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

            yTimer = yCooldown;
        }
    }

    public override void BAction()
    {
        if (btimer < 0)
        {
            btimer = bCooldown;
            anim.SetTrigger("BAttack");
        }
    }
    public void DigTravel()
    {
        transform.position += dir * digDistance;
    }

}