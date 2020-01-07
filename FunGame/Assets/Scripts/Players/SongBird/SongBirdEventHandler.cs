using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SongBirdEventHandler : BlankMono
{

    public SongBird songBird;
    public CorvidDagger weapon;

    public void ThrowVial() { songBird.ThrowVial(); }
    public void DeathVial() { songBird.DeathCloud(); }

    public void BeginAttack() { weapon.StartAttack(); }
    public void EndAttack() { weapon.EndAttack(); }

    //Common
    public void GainHA() { songBird.GainHA(); }
    public void LoseHA() { songBird.LoseHA(); }

    public void Vibration(float intensity, float dur) { songBird.ControllerRumble(intensity, dur); }
    public void GainIFrames() { songBird.GainIFrames(); }
    public void LoseIFrames() { songBird.LoseIFrames(); }

    public void BeginActing() { songBird.BeginActing(); }
    public void EndActing() { songBird.EndActing(); }

    public void EndDodge() { songBird.StopKnockback(); }



}