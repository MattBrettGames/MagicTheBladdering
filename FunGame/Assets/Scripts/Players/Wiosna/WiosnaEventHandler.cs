using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WiosnaEventHandler : BlankMono
{

    public Wiosna wiosna;
    public Weapons flameJet;
    MeshRenderer jetMesh;
    public Weapons shotgun;
    MeshRenderer shotgunMesh;
    public Weapons explosion;
    MeshRenderer explosionMesh;

    void Start()
    {
        jetMesh = flameJet.gameObject.GetComponent<MeshRenderer>();
        shotgunMesh = shotgun.gameObject.GetComponent<MeshRenderer>();
        explosionMesh = explosion.gameObject.GetComponent<MeshRenderer>();
    }

    public void FlameJetBeginAttack() { jetMesh.enabled = true; flameJet.StartAttack(); wiosna.FlameJetOn(); }
    public void FlameJetEndAttack() { jetMesh.enabled = false; flameJet.EndAttack(); wiosna.FlameJetOff(); }

    public void ShotgunBeginAttack() { shotgunMesh.enabled = true; shotgun.StartAttack(); wiosna.ShotgunOn(); }
    public void ShotgunEndAttack() { shotgunMesh.enabled = false; shotgun.EndAttack(); wiosna.ShotgunOff(); }

    public void ExplosionBeginAttack() { explosionMesh.enabled = true; explosion.StartAttack(); }
    public void ExplosionEndAttack() { explosionMesh.enabled = false; explosion.EndAttack(); }

    public void BeginActing() { wiosna.BeginActing(); }
    public void EndActing() { wiosna.EndActing(); }

    public void GainIFrmaes() { wiosna.GainIFrames(); }
    public void LoseIFrmaes() { wiosna.LoseIFrames(); }

    public void DoTheDash() { wiosna.DoTheTeleport(); }

}
