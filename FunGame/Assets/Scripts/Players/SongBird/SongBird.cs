using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SongBird : PlayerBase
{

    [SerializeField] AudioClip bSoundBonus;

    [Header("More Components")]
    public CorvidDagger weapon;
    private GameObject smokeCloud;
    private GameObject smokeCloudCannister;
    private GameObject smokeCloudDodge;
    private GameObject cannister;
    [SerializeField] Color playerColour;

    [Header("Dagger Swipe")]
    public int baseXDamage;
    public int baseXKnockback;

    [Header("All Prefabs for Instantiation")]
    [SerializeField] GameObject smokeCloudPrefab;
    [SerializeField] GameObject cannisterPrefab;

    [Header("Cannister Cloud")]
    [SerializeField] int cannisterCloudSize;
    [SerializeField] int cannisterBurstDamage;
    [SerializeField] float cannisterPoisonTime;
    [SerializeField] int cannisterSmokeKnockback;
    [SerializeField] float cannisterImpactDur;
    [SerializeField] bool cannisterInterrupt;
    private bool hasCannister;

    [Header("Dodge Cloud")]
    [SerializeField] int dodgeCloudSize;
    [SerializeField] int dodgeBurstDamage;
    [SerializeField] float dodgePoisonTime;
    [SerializeField] int dodgeSmokeKnockback;
    [SerializeField] float dodgeImpactdur;
    [SerializeField] bool dodgeInterrupt;

    [Header("Thrown Cloud")]
    [SerializeField] int thrownCloudSize;
    [SerializeField] int thrownBurstDamage;
    [SerializeField] float thrownCloudTime;
    [SerializeField] int thrownSmokeKnockback;
    [SerializeField] float thrownImpactDur;
    [SerializeField] bool thrownInterrupt;

    public override void SetInfo(UniverseController uni, int layerNew)
    {
        base.SetInfo(uni, layerNew);
        Invoke("GainSmokes", 0.1f);
    }

    void GainSmokes()
    {
        smokeCloud = GenerateSmokeCloud();
        smokeCloud.tag = tag;
        smokeCloudCannister = GenerateSmokeCloud();
        smokeCloudCannister.tag = tag;
        smokeCloudDodge = GenerateSmokeCloud();
        smokeCloudDodge.tag = tag;

        cannister = Instantiate(cannisterPrefab);
        cannister.SetActive(false);
        cannister.tag = tag;
        hasCannister = true;
    }

    public GameObject GenerateSmokeCloud()
    {
        GameObject smoke = Instantiate(smokeCloudPrefab);
        smoke.SetActive(false);
        return smoke;
    }

    public override void XAction()
    {
        if (xTimer <= 0)
        {
            base.XAction();

            anim.SetTrigger("XAttack");
            weapon.GainInfo(baseXDamage, baseXKnockback, visuals.transform.forward, pvp, 0, this, true);
            xTimer = xCooldown;
            PlaySound(xSound);
        }
    }

    public override void YAction()
    {
        if (yTimer <= 0)
        {
            base.YAction();

            anim.SetTrigger("YAction");
            yTimer = yCooldown;
            PlaySound(ySound);
        }
    }

    public override void BAction()
    {
        if (hasCannister)
        {
            if (bTimer <= 0)
            {
                base.BAction();

                cannister.transform.position = transform.position;
                cannister.SetActive(true);
                hasCannister = false;
                anim.SetTrigger("BAction");
                bTimer = bCooldown;
                PlaySound(bSound);
            }
        }
        else
        {
            smokeCloudCannister.transform.localScale = Vector3.one;
            cannister.GetComponent<Cannister>().TriggerBurst(smokeCloudCannister, cannisterBurstDamage, cannisterCloudSize, cannisterSmokeKnockback, cannisterPoisonTime, this, cannisterImpactDur, cannisterInterrupt, playerColour);
            hasCannister = true;
            PlaySound(bSoundBonus);
        }
    }

    public override void AAction(bool playAnim)
    {
        if (aTimer <= 0 && dir != Vector3.zero)
        {
            base.AAction(false);

            anim.SetTrigger("AAction");
            state = State.dodging;

            Invoke("EndDodge", dodgeDur);

            smokeCloudDodge.transform.position = transform.position;
            smokeCloudDodge.transform.localScale = Vector3.zero;
            smokeCloudDodge.transform.rotation = new Quaternion(0, 0, 180, 0);
            smokeCloudDodge.SetActive(true);
            smokeCloudDodge.GetComponent<SmokeBase>().Begin(dodgeBurstDamage, dodgeSmokeKnockback, dodgeCloudSize, dodgePoisonTime, this, tag, dodgeImpactdur, dodgeInterrupt, playerColour);

            for (int i = 0; i < dodgeCloudSize; i++)
            {
                StartCoroutine(smokeGrowth(i * 0.01f, smokeCloudDodge));
            }
            PlaySound(aSound);
        }
    }

    private IEnumerator smokeGrowth(float time, GameObject smokecloud)
    {
        yield return new WaitForSeconds(time);
        smokecloud.transform.localScale += Vector3.one;
    }

    public void ThrowVial()
    {
        smokeCloud.transform.position = transform.position;
        smokeCloud.transform.localScale = Vector3.zero;
        smokeCloud.transform.rotation = new Quaternion(0, 0, 180, 0);
        smokeCloud.SetActive(true);
        smokeCloud.GetComponent<SmokeBase>().Begin(thrownBurstDamage, thrownSmokeKnockback, thrownCloudSize, thrownCloudTime, this, tag, thrownImpactDur, thrownInterrupt, playerColour);

        for (int i = 0; i < thrownCloudSize; i++)
        {
            StartCoroutine(SmokeMove(smokeCloud, visuals.transform.forward, i * 0.01f, false));
        }
    }

    private IEnumerator SmokeMove(GameObject smokeCloud, Vector3 dir, float time, bool willShrink)
    {
        yield return new WaitForSeconds(time);
        smokeCloud.transform.position += dir * 2;
        smokeCloud.transform.localScale += Vector3.one;
        if (willShrink) StartCoroutine(ShrinkCloud(smokeCloud));
    }

    IEnumerator ShrinkCloud(GameObject smokeCloud)
    {
        yield return new WaitForSeconds(1);
        smokeCloud.transform.localScale -= Vector3.one;
    }

};