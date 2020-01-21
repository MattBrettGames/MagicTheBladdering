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

    [Header("Y Attack")]
    [SerializeField] int yDamage;
    [SerializeField] int yKnockback;
    [SerializeField] float ySpacing;
    [SerializeField] float yTimeBetweenBlasts;
    [SerializeField] int numberOfBlasts;

    WiosnaExplosions blast1;
    WiosnaExplosions blast2;

    [Header("Teleport")]
    [SerializeField] GameObject vanishEffect;
    [SerializeField] GameObject appearEffect;

    [Header("Flaming Clone")]
    [SerializeField] int cloneDamage;
    [SerializeField] Color cloneColour;
    private GameObject flamingClone;
    GameObject cloneExplosion;

    public override void SetInfo(UniverseController uni, int layerNew)
    {
        base.SetInfo(uni, layerNew);
        Invoke("GainClone", 0.1f);
    }

    void GainClone()
    {
        objectPooler = GameObject.FindGameObjectWithTag("ObjectPooler").GetComponent<ObjectPooler>();
        flamingClone = objectPooler.cloneList[playerID];
        cloneExplosion = objectPooler.cloneExplosionList[playerID];

        flamingClone.GetComponent<FlamingWiosna>().SetInfo(thisPlayer, cloneDamage, cloneColour, tag, cloneExplosion, this);        

        blast1 = objectPooler.blastList[playerID * 2].GetComponent<WiosnaExplosions>();
        blast2 = objectPooler.blastList[(playerID * 2) + 1].GetComponent<WiosnaExplosions>();

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
            appearEffect.SetActive(false); ;
            vanishEffect.SetActive(true);

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
        vanishEffect.SetActive(false);
        appearEffect.SetActive(true);
    }

    public override void YAction()
    {
        if (yTimer <= 0)
        {
            anim.SetTrigger("YAttack");

            blast1.transform.position = gameObject.transform.position;
            blast2.transform.position = gameObject.transform.position;

            blast1.StartChain(this, yDamage, yKnockback, blast2, transform.position, visuals.transform.forward, ySpacing, yTimeBetweenBlasts, numberOfBlasts, universe, ySound);

            yTimer = yCooldown;
        }
    }

    public override void BAction()
    {
        if (bTimer <= 0)
        {
            flamingClone.transform.position = transform.position;
            flamingClone.GetComponent<FlamingWiosna>().AwakenClone(lockTargetList[currentLock].transform);
            flamingClone.SetActive(true);
            bTimer = bCooldown;
            
            universe.PlaySound(bSound);
        }
    }
}