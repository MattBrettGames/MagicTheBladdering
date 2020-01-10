using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wiosna : PlayerBase
{

    [Header("More Components")]
    public Weapons basicMelee;
    public Weapons explosion;
    private ObjectPooler objectPooler;

    [Header("X Attack")]
    [SerializeField] int xDamage;
    [SerializeField] int xKnockback;

    [Header("Y Attack")]
    [SerializeField] int yDamage;
    [SerializeField] int yKnockback;

    private GameObject flamingClone;

    public override void SetInfo(UniverseController uni, int layerNew)
    {
        base.SetInfo(uni, layerNew);
        Invoke("GainClone", 0.1f);
    }

    void GainClone()
    {
        objectPooler = GameObject.FindGameObjectWithTag("ObjectPooler").GetComponent<ObjectPooler>();
        flamingClone = objectPooler.cloneList[playerID];
        flamingClone.GetComponent<FlamingWiosna>().SetInfo(lookAtTarget, thisPlayer);
    }

    public override void XAction()
    {
        if (xTimer <= 0)
        {
            anim.SetTrigger("XAttack");
            basicMelee.GainInfo(xDamage, xKnockback, visuals.transform.forward, pvp, 0, this, true);
            xTimer = xCooldown;
            universe.PlaySound(xSound);
        }
    }
    
    public override void AAction()
    {
        if (aTimer <= 0)
        {
            int thisLayer;
            if (playerID == 0)
            {
                thisLayer = 13;
            }
            else
            {
                thisLayer = 14;
            }
            Physics.IgnoreLayerCollision(thisLayer, 12, true);
            outline.OutlineColor = Color.grey;

            anim.SetTrigger("AAction");

            state = State.dodging;

            StartCoroutine(EndDig(thisLayer));

            universe.PlaySound(aSound);
        }
    }
    IEnumerator EndDig(int layer)
    {
        yield return new WaitForSeconds(dodgeDur);
        outline.OutlineColor = Color.black;
        aTimer = aCooldown;
        base.EndDodge();
        Physics.IgnoreLayerCollision(layer, 12, false);
    }

    public override void YAction()
    {
        if (yTimer <= 0)
        {
            anim.SetTrigger("YAttack");
            explosion.GainInfo(yDamage, yKnockback, visuals.transform.forward, pvp, 0, this, true);
            yTimer = yCooldown;
            universe.PlaySound(ySound);
        }
    }

    public override void BAction()
    {
        if (bTimer <= 0)
        {
            flamingClone.transform.position = transform.position + dir;
            flamingClone.GetComponent<FlamingWiosna>().AwakenClone();
            flamingClone.SetActive(true);
            bTimer = bCooldown;

            universe.PlaySound(bSound);
        }
    }
}