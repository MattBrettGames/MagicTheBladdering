using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WiosnaEventHandler : BlankMono
{

    public Wiosna wiosna;
    public Weapons beam;
    public Weapons explosion;
    public Weapons melee;

    void Start()
    {
        print(melee + " is Wiosna's melee");
        beam.gameObject.SetActive(false);
        melee.gameObject.SetActive(false);
        explosion.gameObject.SetActive(false);
    }


    public void ExplosionBeginAttack() { wiosna.BeginExplosion(); }
    public void BeamBeginAttack() { wiosna.BeginBeam(); }

    public void BeginMelee() { melee.gameObject.SetActive(true); melee.StartAttack(); }
    public void EndMelee() { melee.EndAttack(); melee.gameObject.SetActive(false); }

    public void BeginActing() { wiosna.BeginActing(); }
    public void EndActing() { wiosna.EndActing(); }

    public void GainIFrmaes() { wiosna.GainIFrames(); }
    public void LoseIFrmaes() { wiosna.LoseIFrames(); }

    public void DoTheDash() { wiosna.DoTheTeleport(); }
}