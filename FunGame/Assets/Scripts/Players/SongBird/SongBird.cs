using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SongBird : PlayerBase
{
    [Header("More Components")]
    public CorvidDagger weapon;
    private ObjectPooler pooler;
    private GameObject smokeCloud;
    private GameObject smokeCloudCannister;
    private GameObject cannister;

    [Header("Dagger Swipe")]
    public int baseXDamage;
    public int baseXKnockback;

    [Header("Vial Stats")]
    public int smokeburstDamage;
    [SerializeField] float smokePoisonTicks;
    [Space]
    public int thrownCloudSize;
    public int dodgeCloudSize;
    public int cannisterCloudSize;
    [Space]
    public int smokeKnockback;

    private bool hasCannister;

    [Header("Unique Sounds")]
    [SerializeField] string bSoundBonus;


    public override void SetInfo(UniverseController uni)
    {
        base.SetInfo(uni);
        pooler = GameObject.FindGameObjectWithTag("ObjectPooler").GetComponent<ObjectPooler>();
        Invoke("GainSmokes", 0.1f);
    }

    void GainSmokes()
    {
        smokeCloud = pooler.ReturnSmokeCloud(playerID);
        smokeCloud.tag = tag;
        smokeCloudCannister = pooler.ReturnSmokeCloud(pooler.poisonSmokeList.Count - (playerID + 1));
        smokeCloudCannister.tag = tag;

        cannister = pooler.cannisters[playerID];
        cannister.tag = tag;
        hasCannister = true;
    }

    public override void XAction()
    {
        if (xTimer <= 0)
        {
            anim.SetTrigger("XAttack");
            weapon.GainInfo(baseXDamage, baseXKnockback, visuals.transform.forward, pvp, 0, this);
            xTimer = xCooldown;
            universe.PlaySound(xSound);
        }
    }

    public override void YAction()
    {
        if (yTimer <= 0)
        {
            anim.SetTrigger("YAction");
            yTimer = yCooldown;
            universe.PlaySound(ySound);
        }
    }

    public override void BAction()
    {
        if (hasCannister)
        {
            if (bTimer <= 0)
            {
                cannister.transform.position = transform.position;
                cannister.SetActive(true);
                hasCannister = false;
                anim.SetTrigger("BAction");
                bTimer = bCooldown;
            universe.PlaySound(bSound);
            }
        }
        else
        {
            smokeCloudCannister.transform.localScale = Vector3.one;
            cannister.GetComponent<Cannister>().TriggerBurst(smokeCloudCannister, smokeburstDamage, smokePoisonTicks, cannisterCloudSize, smokeKnockback, lookAtTarget.gameObject);
            hasCannister = true;
            universe.PlaySound(bSoundBonus);
        }
    }

    public override void AAction()
    {
        if (aTimer <= 0)
        {

            smokeCloud.transform.position = transform.position;
            smokeCloud.transform.localScale = Vector3.zero;
            smokeCloud.transform.rotation = new Quaternion(0, 0, 180, 0);
            smokeCloud.SetActive(true);

            anim.SetTrigger("AAction");
            state = State.dodging;

            Invoke("EndDodge", dodgeDur);

            for (int i = 0; i < dodgeCloudSize; i++)
            {
                StartCoroutine(smokeGrowth(i * 0.01f, smokeCloud));
            }

            universe.PlaySound(aSound);
            aTimer = aCooldown;
        }
    }

    public void EndDodge()
    {
        state = State.normal;
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
        smokeCloud.GetComponent<SmokeBase>().Begin(smokeburstDamage, smokePoisonTicks, smokeKnockback, lookAtTarget.gameObject, thrownCloudSize);

        for (int i = 0; i < thrownCloudSize; i++)
        {
            StartCoroutine(SmokeMove(smokeCloud, dir, i * 0.01f));
        }
    }

    private IEnumerator SmokeMove(GameObject smokeCloud, Vector3 dir, float time)
    {
        yield return new WaitForSeconds(time);
        smokeCloud.transform.position += dir * 2;
        smokeCloud.transform.localScale += Vector3.one;
    }
}