using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SongBirdEventHandler : BlankMono
{

    public SongBird songBird;
    public CorvidDagger weapon;

    public void ThrowVial() { songBird.ThrowVial(); }

    public void BeginAttack() { weapon.StartAttack(); }
    public void EndAttack() { weapon.EndAttack(); }

    public void DeathVial() { songBird.DeathVial(); }

    //Common
    public void GainHA() { songBird.GainHA(); }
    public void LoseHA() { songBird.LoseHA(); }

    public void Vibration(float intensity, float dur) { songBird.ControllerRumble(intensity, dur, false, null); }
    public void GainIFrames() { songBird.GainIFrames(); }
    public void LoseIFrames() { songBird.LoseIFrames(); }

    public void BeginActing() { songBird.BeginActing(); }
    public void EndActing() { songBird.EndActing(); }

    public void EndDodge() { songBird.StopKnockback(); }


    #region Sound
    public void PlaySound(AudioClip clipToPlay)
    {
        songBird.PlaySound(clipToPlay);
    }
    public void PlaySoundFromArray(AudioClip[] clipsToPlay)
    {
        songBird.PlaySound(clipsToPlay);
    }
    #endregion


}
