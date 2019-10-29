using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SongBird : PlayerBase
{
    [Header("More Components")]
    public CorvidDagger weapon;
    private GameObject vial;
    private ObjectPooler pooler;

    [Header("Dagger Swipe")]
    public int baseXDamage;
    public int adrenXDamage;
    public int boomXKnockback;

    [Header("Vial Stats")]
    public MeshRenderer meshRenderer;
    public Material[] typeMaterials = new Material[3];
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
        anim.SetTrigger("XAttack");
        if (currentVial == 0) { weapon.poisonActive = true; weapon.GainInfo(baseXDamage, 0, visuals.transform.forward); }
        else { weapon.poisonActive = false; }

        if (currentVial == 1) { weapon.GainInfo(adrenXDamage, 0, visuals.transform.forward); }
        if (currentVial == 2) { weapon.GainInfo(baseXDamage, boomXKnockback, visuals.transform.forward); }
    }

    public override void YAction() { ThrowVial(); anim.SetTrigger("YAction"); }
    //ThrowVial(); in the animation

    public override void BAction()
    {
        anim.SetTrigger("BAction");
        if (currentVial != 2) { currentVial++; } else { currentVial = 0; }
        //GetComponentInChildren<Renderer>().material = typeMaterials[currentVial];
    }

    public override void AAction()
    {
        if (smokeCount <= 2)
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

            smokeCount++;
            Invoke("RegainSmoke", 6);
        }

        anim.SetTrigger("AAction");
        transform.position = Vector3.Slerp(transform.position, dodgeTarget.position, dodgeForce);
        Invoke("StopKnockback", 0.2f);
    }

    private void RegainSmoke()
    {
        smokeCount--;
    }

    public void ThrowVial()
    {
        vial = pooler.vials[playerID];
        SongbirdVial vials = vial.GetComponent<SongbirdVial>();
        vials.vialType = types[currentVial];
        vials.VialThrown(vialColours[currentVial], rangeTarget.position, playerID, gameObject.tag);
        vial.SetActive(true);
        vial = null;
    }
}