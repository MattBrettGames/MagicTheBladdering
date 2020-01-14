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

    public void CreateCrack() { valdy.LeaveCrack(hammer.head.position - new Vector3(0, 1.5f, 0)); }

    public void GainHA() { valdy.GainHA(); }
    public void LoseHA() { valdy.LoseHA(); }

    public void GainIFrames() { valdy.GainIFrames(); }
    public void LoseIFrames() { valdy.LoseIFrames(); }

    public void BeginActing() { valdy.BeginActing(); }
    public void EndActing() { valdy.EndActing(); }

}