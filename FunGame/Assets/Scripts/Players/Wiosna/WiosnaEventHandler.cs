using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WiosnaEventHandler : BlankMono
{

    public Wiosna wiosna;
    public Weapons beam;
    public Weapons explosion;
    public Weapons melee;

    public void ExplosionBeginAttack() { wiosna.BeginExplosion(); }
    public void BeamBeginAttack() { wiosna.BeginBeam(); }

    public void BeginMelee() { melee.StartAttack(); }
    public void EndMelee() { melee.EndAttack(); }

    public void BeginActing() { wiosna.BeginActing(); }
    public void EndActing() { wiosna.EndActing(); }

    public void GainIFrmaes() { wiosna.GainIFrames(); }
    public void LoseIFrmaes() { wiosna.LoseIFrames(); }

    public void DoTheDash() { wiosna.DoTheTeleport(); }
}