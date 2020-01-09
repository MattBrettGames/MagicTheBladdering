using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Carmen : PlayerBase
{

    [SerializeField] string ySoundBonus;

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
    [SerializeField] Weapons spinSphere;
    [SerializeField] float spinRadius;

    [Header("Dig")]
    public float digDur;
    [SerializeField] float digSpeedBonus;

    [Header("Backstab")]
    public int backstabAngle;
    public float backStabDamageMult;
    private GameObject enemyVisual;

    public override void SetInfo(UniverseController uni, int layerNew)
    {
        base.SetInfo(uni, layerNew);
        StartCoroutine(GainBack());
        spinSphere.GetComponent<SphereCollider>().radius = spinRadius;
    }

    IEnumerator GainBack()
    {
        yield return new WaitForEndOfFrame();
        enemyVisual = lookAtTarget.parent.GetComponentInChildren<Animator>().gameObject;
    }

    public override void XAction()
    {
        if (xTimer <= 0)
        {
            anim.SetTrigger("XAttack");
            spinSphere.GainInfo(slashDamage, slashKnockback, visuals.transform.forward, pvp, 0, this);
            state = State.dodging;
            Invoke("StopKnockback", slashTravelDuration);

            xTimer = xCooldown;
            universe.PlaySound(xSound);
        }
    }

    public override void YAction()
    {
        if (yTimer <= 0)
        {
            float lookDif = Vector3.Angle(visuals.transform.forward, enemyVisual.transform.forward);
            anim.SetTrigger("YAttack");

            if (lookDif <= backstabAngle)
            {
                leftDagger.GainInfo(Mathf.RoundToInt(stabDamage * backStabDamageMult), stabKnockback, visuals.transform.forward, pvp, 0, this);
                //rightDagger.GainInfo(Mathf.RoundToInt(stabDamage * backStabDamageMult), stabKnockback, visuals.transform.forward, pvp, 0, this);
                universe.PlaySound(ySoundBonus);
            }
            else
            {
                leftDagger.GainInfo(stabDamage, stabKnockback, visuals.transform.forward, pvp, 0, this);
                // rightDagger.GainInfo(stabDamage, stabKnockback, visuals.transform.forward, pvp, 0, this);
                universe.PlaySound(ySound);
            }

            yTimer = yCooldown;
        }
    }

    public override void BAction()
    {
        if (bTimer <= 0)
        {
            int thisLayer;
            int otherLayer;
            if (playerID == 0)
            {
                thisLayer = 13;
                otherLayer = 14;
                Physics.IgnoreLayerCollision(thisLayer, 12, true);
                Physics.IgnoreLayerCollision(thisLayer, otherLayer, true);
            }
            else
            {
                thisLayer = 14;
                otherLayer = 13;
                Physics.IgnoreLayerCollision(thisLayer, 12, true);
                Physics.IgnoreLayerCollision(thisLayer, otherLayer, true);
            }

            outline.OutlineColor = Color.grey;

            dodgeSpeed += digSpeedBonus;

            anim.SetTrigger("BAttack");

            state = State.dodging;

            StartCoroutine(EndDig(thisLayer, otherLayer));

            universe.PlaySound(bSound);
        }
    }
    IEnumerator EndDig(int layer, int otherLayer)
    {
        yield return new WaitForSeconds(dodgeDur);
        outline.OutlineColor = Color.black;
        dodgeSpeed -= digSpeedBonus;
        base.EndDodge();
        Physics.IgnoreLayerCollision(layer, 12, false);
        Physics.IgnoreLayerCollision(layer, otherLayer, false);
    }
}