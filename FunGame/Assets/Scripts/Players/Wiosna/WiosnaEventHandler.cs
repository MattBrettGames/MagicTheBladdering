using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WiosnaEventHandler : BlankMono
{

    public Wiosna wiosna;
    public Weapons flameJet;
    public Weapons shotgun;
    public Weapons explosion;

    public void FlameJetBeginAttack() { flameJet.StartAttack(); }
    public void FlameJetEndAttack() { flameJet.EndAttack(); }

    public void ShotgunBeginAttack() { shotgun.StartAttack(); }
    public void ShotgunEndAttack() { shotgun.EndAttack(); }

    public void ExplosionBeginAttack() { explosion.StartAttack(); }
    public void ExplosionEndAttack() { explosion.EndAttack(); }

    public void BeginActing() { wiosna.BeginActing(); }
    public void EndActing() { wiosna.EndActing(); }

    public void GainIFrmaes() { wiosna.GainIFrames(); }
    public void LoseIFrmaes() { wiosna.LoseIFrames(); }


}
