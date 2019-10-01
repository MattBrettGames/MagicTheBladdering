using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SongBird : PlayerBase
{
    [Header("Props")]
    public GameObject vial;
    public Weapons weapon;

    [Header("Dagger Swipe")]
    public int baseXDamage;
    public int boostedXDamage;

    public override void XAction() { }

    public override void YAction() { }

    public override void BAction() { }

    public override void AAction() { }


    private void ThrowVial(GameObject vial)
    {
        vial.GetComponent<Rigidbody>().AddForce(visuals.transform.forward + new Vector3(0,3,0), ForceMode.Impulse);
    }

}
