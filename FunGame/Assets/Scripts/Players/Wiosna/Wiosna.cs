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
    [SerializeField] float yDelay;
    [SerializeField] int numberOfBlasts;
    [SerializeField] GameObject explosionPrefabs;

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
    [SerializeField] GameObject summonParticles;

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

        blast1 = GameObject.Instantiate(explosionPrefabs).GetComponent<WiosnaExplosions>();
        blast2 = GameObject.Instantiate(explosionPrefabs).GetComponent<WiosnaExplosions>();

        vanishEffect.transform.SetParent(gameObject.transform.parent);
        appearEffect.transform.SetParent(gameObject.transform.parent);

    }

    public override void XAction()
    {
        if (xTimer <= 0)
        {
            base.XAction();

            anim.SetTrigger("XAttack");
            basicMelee.GainInfo(xDamage, xKnockback, visuals.transform.forward, pvp, 0, this, true, AttackType.X);
            xTimer = xCooldown;
            PlaySound(xSound);
        }
    }

    public override void AAction(bool playAnim)
    {
        if (aTimer <= 0)
        {
            anim.SetTrigger("AAction");
            PlaySound(aSound);
        }
    }

    public void DotheDodge()
    {
        base.AAction(false);

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
        
        state = State.dodging;

        StartCoroutine(EndDig(thisLayer));
        vanishEffect.SetActive(false);
        appearEffect.SetActive(false);
        vanishEffect.transform.localPosition = transform.localPosition;
        vanishEffect.SetActive(true);
    }

    IEnumerator EndDig(int layer)
    {
        yield return new WaitForSeconds(dodgeDur);
        print("Ended Dodge");
        outline.OutlineColor = Color.black;
        base.EndDodge();
        Physics.IgnoreLayerCollision(layer, 12, false);
        appearEffect.transform.localPosition = transform.localPosition;
        appearEffect.SetActive(true);
    }

    public override void YAction()
    {
        if (yTimer <= 0)
        {
            base.YAction();

            anim.SetTrigger("YAttack");
            StartCoroutine(DelayedY());

            yTimer = yCooldown;
        }
    }

    IEnumerator DelayedY()
    {
        yield return new WaitForSeconds(yDelay);

        blast1.transform.position = gameObject.transform.position;
        blast2.transform.position = gameObject.transform.position;

        blast1.StartChain(this, yDamage, yKnockback, blast2, transform.position, visuals.transform.forward, ySpacing, yTimeBetweenBlasts, numberOfBlasts, universe, ySound);
    }

    public override void BAction()
    {
        if (bTimer <= 0)
        {
            summonParticles.SetActive(false);
            base.BAction();

            bTimer = bCooldown;
            anim.SetTrigger("BAttack");
            summonParticles.SetActive(true);

            PlaySound(bSound);
        }
    }

    public void SummonClone()
    {
        flamingClone.transform.position = transform.position;
        flamingClone.GetComponent<FlamingWiosna>().AwakenClone(lockTargetList[currentLock].transform);
        flamingClone.SetActive(true);
    }
}