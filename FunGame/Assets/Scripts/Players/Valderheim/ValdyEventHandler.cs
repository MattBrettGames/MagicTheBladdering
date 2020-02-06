using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ValdyEventHandler : BlankMono
{
    public Valderheim valdy;
    public Weapons hammer;

    public void HammerBoxOn() { hammer.StartAttack(); }
    public void HammerBoxOff() { hammer.EndAttack(); }

    public void OpenKickCombo() { valdy.OpenComboKick(); }
    public void Vibration(float intensity, float dur) { valdy.ControllerRumble(intensity, dur); }

    public void BeginSlow() { valdy.BeginSlow(); }
    public void EndSlow() { }// valdy.EndSlow(); }

    public void CreateCrack() { valdy.LeaveCrack(hammer.head.position); }

    public void GainHA() { valdy.GainHA(); }
    public void LoseHA() { valdy.LoseHA(); }

    public void GainIFrames() { valdy.GainIFrames(); }
    public void LoseIFrames() { valdy.LoseIFrames(); }

    public void BeginActing() { valdy.BeginActing(); }
    public void EndActing() { valdy.EndActing(); }


    #region Sound
    public void PlaySound(AudioClip clipToPlay)
    {
        valdy.PlaySound(clipToPlay);
    }
    public void PlaySoundFromArray(AudioClip[] clipsToPlay)
    {
        valdy.PlaySound(clipsToPlay);
    }
    #endregion


}