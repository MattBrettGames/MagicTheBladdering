using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SongBird : PlayerBase
{
    [Header("Props")]
    private GameObject vial;
    public Weapons weapon;
    private ObjectPooler pooler;

    [Header("Dagger Swipe")]
    public int baseXDamage;
    public int adrenXDamage;
    public int boomXKnockback;

    [Header("Vial Stats")]
    private int currentVial;
    private string[] types = new string[] { "Poison", "Adrenaline", "Boom" };
    public Color[] vialColours = new Color[3];

    public override void Start()
    {
        base.Start();
        pooler = GameObject.FindGameObjectWithTag("ObjectPooler").GetComponent<ObjectPooler>();
    }

    public override void XAction()
    {
        if (currentVial == 0)
        {
            
        }
        if (currentVial == 1) { weapon.GainInfo(adrenXDamage, 0, visuals.transform.forward); }
        if (currentVial == 2) { weapon.GainInfo(baseXDamage, boomXKnockback, visuals.transform.forward); }
    }

    public override void YAction() { ThrowVial(); }

    public override void BAction() { if (currentVial != 2) { currentVial++; } else { currentVial = 0; } }

    public override void AAction() { }

    private void ThrowVial()
    {
        vial = pooler.vials[0];
        vial.GetComponent<MeshRenderer>().material.color = vialColours[currentVial];
        vial.transform.position = transform.position;
        vial.GetComponent<SongbirdVial>().vialType = types[currentVial];
        vial.GetComponent<Rigidbody>().AddForce(visuals.transform.forward + new Vector3(0, 3, 0), ForceMode.Impulse);
        vial.SetActive(true);
        vial = null;
    }

}