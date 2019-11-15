using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SongBird : PlayerBase
{
    [Header("More Components")]
    public CorvidDagger weapon;
    private ObjectPooler pooler;

    [Header("Dagger Swipe")]
    public int baseXDamage;
    public int adrenXDamage;
    public int boomXKnockback;

    [Header("Vial Stats")]
    public int poisonTickDist;
    public int adrenalineTickDist;
    public int boomTickDist;

    private int currentVial;
    private string[] types = new string[] { "Poison", "Adrenaline", "Boom" };
    public int smokeCount = 3;

    [Header("Dodge Stats")]
    public int dodgeForce;

    public override void Start()
    {
        base.Start();
        pooler = GameObject.FindGameObjectWithTag("ObjectPooler").GetComponent<ObjectPooler>();
        InvokeRepeating("RegainSmoke", 6, 6);
    }

    public override void XAction()
    {
        anim.SetTrigger("XAttack");
        if (currentVial == 0) { weapon.poisonActive = true; weapon.GainInfo(baseXDamage, 0, visuals.transform.forward, pvp); }
        else { weapon.poisonActive = false; }

        if (currentVial == 1) { weapon.GainInfo(adrenXDamage, 0, visuals.transform.forward, pvp); }
        if (currentVial == 2) { weapon.GainInfo(baseXDamage, boomXKnockback, visuals.transform.forward, pvp); }
    }

    public override void YAction() { anim.SetTrigger("YAction"); }

    public override void BAction()
    {
        anim.SetTrigger("BAction");
        if (currentVial != 2) { currentVial++; } else { currentVial = 0; }
        //GetComponentInChildren<Renderer>().material = typeMaterials[currentVial];
    }

    public override void AAction()
    {
        if (smokeCount > 0)
        {
            GameObject smokeCloud = null;
            int smokeTicks = 0;

            if (currentVial == 0)
            {
                smokeCloud = pooler.poisonSmoke[pooler.poisonSmoke.Count - 1];
                pooler.poisonSmoke.Remove(smokeCloud);
                smokeTicks = Mathf.RoundToInt(poisonTickDist * 0.5f);
            }
            if (currentVial == 1)
            {
                smokeCloud = pooler.adrenalineSmoke[pooler.adrenalineSmoke.Count - 1];
                pooler.adrenalineSmoke.Remove(smokeCloud);
                smokeTicks = Mathf.RoundToInt(adrenalineTickDist * 0.5f);
            }
            if (currentVial == 2)
            {
                smokeCloud = pooler.boomSmoke[pooler.boomSmoke.Count - 1];
                pooler.boomSmoke.Remove(smokeCloud);
                smokeTicks = Mathf.RoundToInt(boomTickDist * 0.5f);
            }

            smokeCloud.transform.position = transform.position;
            smokeCloud.transform.localScale = Vector3.zero;
            smokeCloud.SetActive(true);

            smokeCount++;

            anim.SetTrigger("AAction");
            state = State.dodging;

            Invoke("EndDodge", dodgeDur);

            for (int i = 0; i < smokeTicks; i++)
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

    private void RegainSmoke()
    {
        if (smokeCount < 3)
        {
            smokeCount++;
        }
    }

    public void ThrowVial()
    {
        if (smokeCount > 0)
        {
            GameObject smokeCloud = null;
            Vector3 dir = visuals.transform.forward;
            int smokeTicks = 0;
            print("Vars set");

            if (currentVial == 0)
            {
                smokeCloud = pooler.poisonSmoke[pooler.poisonSmoke.Count - 1];
                pooler.poisonSmoke.Remove(smokeCloud);
                smokeTicks = poisonTickDist;
            }
            if (currentVial == 1)
            {
                smokeCloud = pooler.adrenalineSmoke[pooler.adrenalineSmoke.Count - 1];
                pooler.adrenalineSmoke.Remove(smokeCloud);
                smokeTicks = adrenalineTickDist;
            }
            if (currentVial == 2)
            {
                smokeCloud = pooler.boomSmoke[pooler.boomSmoke.Count - 1];
                pooler.boomSmoke.Remove(smokeCloud);
                smokeTicks = boomTickDist;
            }

            smokeCloud.transform.position = transform.position;
            smokeCloud.transform.localScale = Vector3.zero;
            smokeCloud.SetActive(true);
            smokeCloud.GetComponent<SmokeBase>().Begin(tag);

            for (int i = 0; i < smokeTicks; i++)
            {
                StartCoroutine(SmokeMove(smokeCloud, dir, i * 0.01f));
            }
        }
    }

    private IEnumerator SmokeMove(GameObject smokeCloud, Vector3 dir, float time)
    {
        yield return new WaitForSeconds(time);
        smokeCloud.transform.position += dir * 2;
        smokeCloud.transform.localScale += Vector3.one;
    }

    public override int AccessUniqueFeature(int v)
    {
        return smokeCount;
    }


}