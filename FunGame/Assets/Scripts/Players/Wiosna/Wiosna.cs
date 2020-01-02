using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wiosna : PlayerBase
{

    [Header("More Components")]
    public Weapons basicMelee;
    private ObjectPooler objectPooler;

    [Header("X Attack")]
    [SerializeField] int xDamage;
    [SerializeField] int xKnockback;
    [SerializeField] float xCooldown = 0.8f;
    float xTimer;

    [Header("Vanishing Act")]
    [SerializeField] float vanishDistance;
    [SerializeField] float actCooldown = 1;
    float actTimer;

    [Header("Y Action")]
    [SerializeField] float radiusOfStun;
    [SerializeField] float stunDur;
    [SerializeField] ParticleSystem stunParts;
    [Space]
    [SerializeField] float radiusOfPull;
    [SerializeField] int pullImpact;
    [Space]
    [SerializeField] float yCooldown = 1.1f;
    float yTimer;

    [Header("BAttacks")]
    [SerializeField] float bCooldown = 14;
    float bTimer;
    private GameObject flamingClone;

    public override void SetInfo()
    {
        base.SetInfo();
        Invoke("GainClone", 0.1f);
    }

    void GainClone()
    {
        objectPooler = GameObject.FindGameObjectWithTag("ObjectPooler").GetComponent<ObjectPooler>();
        flamingClone = objectPooler.cloneList[playerID];
        flamingClone.GetComponent<FlamingWiosna>().SetInfo(lookAtTarget, thisPlayer);
    }

    public override void Update()
    {
        base.Update();

        bTimer -= Time.deltaTime;
        actTimer -= Time.deltaTime;
    }

    #region X Attacks
    public override void XAction()
    {
        if (xTimer < 0)
        {
            anim.SetTrigger("XAttack");
            basicMelee.GainInfo(xDamage, xKnockback, visuals.transform.forward, pvp, 0);
            xTimer = xCooldown;
        }
    }
    #endregion

    #region A Actions
    public override void AAction()
    {
        if (actTimer <= 0)
        {
            anim.SetTrigger("AAction");
            actTimer = actCooldown;
        }
    }
    public void DoTheTeleport()
    {
        transform.position += dir * vanishDistance;
    }
    #endregion

    #region Y Attacks
    public override void YAction()
    {

    }
    #endregion

    #region B Attacks
    public override void BAction()
    {
        print(bTimer);

        if (bTimer <= 0)
        {
            print(flamingClone.name);
            flamingClone.transform.position = transform.position + dir;
            flamingClone.GetComponent<FlamingWiosna>().AwakenClone();
            flamingClone.SetActive(true);
            bTimer = bCooldown;
        }
    }
    #endregion
}