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

    [Header("Vial Stats")]
    public int smokeburstDamage;
    public int smokePoisonTicks;
    public int cannisterCloudSize;
    private bool hasCannister;

    public override void SetInfo()
    {
        pooler = GameObject.FindGameObjectWithTag("ObjectPooler").GetComponent<ObjectPooler>();

        base.SetInfo();

        Invoke("GainSmokes", 0.1f);                
    }

    void GainSmokes()
    {
        smokeCloud = pooler.ReturnSmokeCloud(playerID);
        smokeCloud.tag = tag;

        smokeCloudCannister = pooler.ReturnSmokeCloud(pooler.poisonSmoke.Count - (1 - playerID));
        smokeCloudCannister.tag = tag;

        cannister = pooler.cannisters[playerID];
        cannister.tag = tag;
        hasCannister = true;
    }
    
    public override void XAction()
    {
        anim.SetTrigger("XAttack");
        weapon.GainInfo(baseXDamage, 0, visuals.transform.forward, pvp);
    }

    public override void YAction() { anim.SetTrigger("YAction"); }

    public override void BAction()
    {
        if (hasCannister)
        {
            cannister.transform.position = transform.position;
            cannister.SetActive(true);
            hasCannister = false;
            anim.SetTrigger("BAttack");
        }
        else
        {
            smokeCloudCannister.transform.localScale = Vector3.one;
            cannister.GetComponent<Cannister>().TriggerBurst(smokeCloudCannister, smokeburstDamage, smokePoisonTicks, cannisterCloudSize);
            hasCannister = true;
        }
    }

    public override void AAction()
    {
        smokeCloud.transform.position = transform.position;
        smokeCloud.transform.localScale = Vector3.zero;
        smokeCloud.SetActive(true);

        anim.SetTrigger("AAction");
        state = State.dodging;

        Invoke("EndDodge", dodgeDur);

        for (int i = 0; i < 5; i++)
        {
            StartCoroutine(smokeGrowth(i * 0.01f, smokeCloud));
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
        smokeCloud.SetActive(true);
        smokeCloud.GetComponent<SmokeBase>().Begin(smokeburstDamage, smokePoisonTicks);

        for (int i = 0; i < 10; i++)
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