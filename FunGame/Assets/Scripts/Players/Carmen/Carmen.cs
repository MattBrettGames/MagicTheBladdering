using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Carmen : PlayerBase
{

    [Header("Unique Components")]
    public Weapons leftDagger;
    public Weapons rightDagger;

    [Header("Dagger Slash")]
    public int slashDamage;
    public int slashKnockback;

    

    public override void XAction() { }

    public override void YAction() { }

    public override void BAction() { }

}
