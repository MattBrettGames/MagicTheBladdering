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

    public override void YAction() { ThrowVial(); }

    public override void BAction() { if (currentVial != 2) { currentVial++; } else { currentVial = 0; } }

    public override void AAction()
    {
        GameObject smokeCloud = null;

        if (currentVial == 0)
        {
            smokeCloud = pooler.poisonSmoke[1];
        }
        if (currentVial == 1)
        {
            smokeCloud = pooler.adrenalineSmoke[1];
        }
        if (currentVial == 2)
        {
            smokeCloud = pooler.boomSmoke[1];
        }

        smokeCloud.transform.position = transform.position;
        smokeCloud.transform.localScale = Vector3.zero;
        smokeCloud.SetActive(true);
        for (int i = 0; i < 5; i++) { StartCoroutine(WaitForSmoke(smokeCloud)); }
    }

    private void ThrowVial()
    {
        vial = pooler.vials[0];
        vial.GetComponent<MeshRenderer>().material.color = vialColours[currentVial];
        vial.GetComponent<SongbirdVial>().vialType = types[currentVial];
        vial.transform.position = transform.position;
        vial.GetComponent<Rigidbody>().AddForce(visuals.transform.forward + new Vector3(0, 3, 0), ForceMode.Impulse);
        vial.SetActive(true);
        vial = null;
    }

    private IEnumerator WaitForSmoke(GameObject smoke)
    {
        yield return new WaitForSeconds(0.1f);
        ScaleUp(smoke);
    }

    private void ScaleUp(GameObject smoke) { smoke.transform.localScale += Vector3.one; }

}