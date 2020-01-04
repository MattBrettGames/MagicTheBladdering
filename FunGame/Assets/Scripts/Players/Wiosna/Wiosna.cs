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

    [Header("Vanishing Act")]
    [SerializeField] float vanishDistance;


    //[Header("BAttacks")]
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

    #region X Attacks
    public override void XAction()
    {
        if (xTimer <= 0)
        {
            anim.SetTrigger("XAttack");
            basicMelee.GainInfo(xDamage, xKnockback, visuals.transform.forward, pvp, 0, this);
            xTimer = xCooldown;
        }
    }
    #endregion

    #region A Actions
    public override void AAction()
    {
        if (aTimer <= 0)
        {
            anim.SetTrigger("AAction");
            aTimer = aCooldown;
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
        if (yTimer <= 0)
        {
            print(gameObject.name + " has done the Y Attack");
            anim.SetTrigger("YAttack");
            yTimer = yCooldown;
        }
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