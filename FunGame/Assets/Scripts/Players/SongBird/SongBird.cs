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
    public int smokePoisonTicks;
    public int thrownCloudSize;
    public int dodgeCloudSize;
    public int cannisterCloudSize;
    public int smokeKnockback;
    private bool hasCannister;

    public override void SetInfo()
    {
        base.SetInfo();
        pooler = GameObject.FindGameObjectWithTag("ObjectPooler").GetComponent<ObjectPooler>();
        Invoke("GainSmokes", 0.1f);
    }

    void GainSmokes()
    {
        smokeCloud = pooler.ReturnSmokeCloud(playerID);
        smokeCloud.tag = tag;

        print(pooler.poisonSmoke.Count - (playerID + 1) + " is the thing you need");
        smokeCloudCannister = pooler.ReturnSmokeCloud(pooler.poisonSmoke.Count - (playerID + 1));
        smokeCloudCannister.tag = tag;

        cannister = pooler.cannisters[playerID];
        cannister.tag = tag;
        hasCannister = true;
    }

    public override void XAction()
    {
        anim.SetTrigger("XAttack");
        weapon.GainInfo(baseXDamage, baseXKnockback, visuals.transform.forward, pvp, 0);
    }

    public override void YAction() { anim.SetTrigger("YAction"); }

    public override void BAction()
    {
        if (hasCannister)
        {
            cannister.transform.position = transform.position;
            cannister.SetActive(true);
            hasCannister = false;
            anim.SetTrigger("BAction");
        }
        else
        {
            smokeCloudCannister.transform.localScale = Vector3.one;
            cannister.GetComponent<Cannister>().TriggerBurst(smokeCloudCannister, smokeburstDamage, smokePoisonTicks, cannisterCloudSize, smokeKnockback);
            hasCannister = true;
        }
    }

    public override void AAction()
    {
        if (dodgeTimer < 0)
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
        }
    }

    public void EndDodge()
    {
        state = State.normal;
        dodgeTimer = dodgeCooldown;
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
        smokeCloud.GetComponent<SmokeBase>().Begin(smokeburstDamage, smokePoisonTicks, smokeKnockback);

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