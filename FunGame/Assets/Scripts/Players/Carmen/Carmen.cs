using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Carmen : PlayerBase
{

    [Header("Unique Components")]
    public Weapons leftDagger;
    public Weapons rightDagger;

    [Header("Dagger Stab")]
    public int stabDamage;
    public int stabKnockback;

    [Header("Dash-Slash")]
    public int slashDamage;
    public int slashKnockback;

    [Header("Dig")]
    public int digDistance;

    [Header("Backstab")]
    public int backstabAngle;

    public override void XAction() { }

    public override void YAction() { }

    public override void BAction() { }

}
