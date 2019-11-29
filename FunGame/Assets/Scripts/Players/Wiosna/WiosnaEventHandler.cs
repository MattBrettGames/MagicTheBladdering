using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WiosnaEventHandler : BlankMono
{

    public Wiosna wiosna;
    public Weapons flameJet;
    public Weapons shotgun;
    public Weapons explosion;

    public void FlameJetBeginAttack() { flameJet.StartAttack(); wiosna.FlameJetOn(); }
    public void FlameJetEndAttack() { flameJet.EndAttack(); wiosna.FlameJetOff(); }

    public void ShotgunBeginAttack() { shotgun.StartAttack(); wiosna.ShotgunOn(); }
    public void ShotgunEndAttack() { shotgun.EndAttack(); wiosna.ShotgunOff(); }

    public void ExplosionBeginAttack() { explosion.StartAttack(); }
    public void ExplosionEndAttack() { explosion.EndAttack(); }

    public void BeginActing() { wiosna.BeginActing(); }
    public void EndActing() { wiosna.EndActing(); }

    public void GainIFrmaes() { wiosna.GainIFrames(); }
    public void LoseIFrmaes() { wiosna.LoseIFrames(); }


}
