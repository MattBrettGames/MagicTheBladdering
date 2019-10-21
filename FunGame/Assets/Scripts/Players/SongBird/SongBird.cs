using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SongBird : PlayerBase
{
    [Header("Props")]
    private GameObject vial;
    public CorvidDagger weapon;
    private ObjectPooler pooler;

    [Header("Dagger Swipe")]
    public int baseXDamage;
    public int adrenXDamage;
    public int boomXKnockback;

    [Header("Vial Stats")]
    public Color[] vialColours = new Color[3];
    private int currentVial;
    private string[] types = new string[] { "Poison", "Adrenaline", "Boom" };
    private int smokeCount;

    [Header("Dodge Stats")]
    public int dodgeForce;

    public override void Start()
    {
        base.Start();
        pooler = GameObject.FindGameObjectWithTag("ObjectPooler").GetComponent<ObjectPooler>();
    }

    public override void XAction()
    {
        print("X Action");
        if (currentVial == 0) { weapon.poisonActive = true; weapon.GainInfo(baseXDamage, 0, visuals.transform.forward); }
        else { weapon.poisonActive = false; }

        if (currentVial == 1) { weapon.GainInfo(adrenXDamage, 0, visuals.transform.forward); }
        if (currentVial == 2) { weapon.GainInfo(baseXDamage, boomXKnockback, visuals.transform.forward); }
    }

    public override void YAction() { ThrowVial(); } // anim.SetTrigger("YAction"); }
    //ThrowVial(); in the animation

    public override void BAction() { if (currentVial != 2) { currentVial++; } else { currentVial = 0; } }

    public override void AAction()
    {
        GameObject smokeCloud = null;

        if (currentVial == 0)
        {
            smokeCloud = pooler.poisonSmoke[smokeCount];
            pooler.poisonSmoke.Remove(smokeCloud);
        }
        if (currentVial == 1)
        {
            smokeCloud = pooler.adrenalineSmoke[smokeCount];
            pooler.adrenalineSmoke.Remove(smokeCloud);
        }
        if (currentVial == 2)
        {
            smokeCloud = pooler.boomSmoke[smokeCount];
            pooler.boomSmoke.Remove(smokeCloud);
        }

        smokeCloud.transform.position = transform.position;
        smokeCloud.transform.localScale = Vector3.zero;
        smokeCloud.SetActive(true);
        for (int i = 0; i < 5; i++) { StartCoroutine(WaitForSmoke(smokeCloud, i)); }

        Knockback(dodgeForce, visuals.transform.forward);
        //anim.SetTrigger("AAction");
        //Uncomment above and replace below with an animation event;
        Invoke("StopKnockback", 0.2f);
    }

    public void ThrowVial()
    {
        //print(pooler.vials[0]);
        //vial.GetComponent<MeshRenderer>().material.color = vialColours[currentVial];

        vial = pooler.vials[playerID];
        SongbirdVial vials = vial.GetComponent<SongbirdVial>();
        vials.vialType = types[currentVial];
        vials.VialThrown(vialColours[currentVial],
            rangeTarget.position
            , playerID, gameObject.tag);
        print(visuals.transform.forward * 4);
        vial.transform.position = transform.position;

        //vial.GetComponent<Rigidbody>().AddForce((visuals.transform.forward*throwDistance), ForceMode.Impulse);

        vial.SetActive(true);
        vial = null;
    }

    private IEnumerator WaitForSmoke(GameObject smoke, int time)
    {
        print(0.1f + time * 0.5f);
        yield return new WaitForSeconds(0.1f + (time * 0.5f));
        ScaleUp(smoke);
    }

    private void ScaleUp(GameObject smoke) { smoke.transform.localScale += Vector3.one; }

}