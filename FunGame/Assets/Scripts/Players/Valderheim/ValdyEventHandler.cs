using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ValdyEventHandler : BlankMono
{
    public Valderheim valdy;
    public Weapons hammer;
    public Weapons chestBox;

    public void ChestBoxOn() { chestBox.gameObject.GetComponent<BoxCollider>().enabled = true; }
    public void ChestBoxOff() { chestBox.gameObject.GetComponent<BoxCollider>().enabled = true; }

    public void HammerBoxOn() { hammer.gameObject.GetComponent<BoxCollider>().enabled = true; }
    public void HammerBoxOff() { hammer.gameObject.GetComponent<BoxCollider>().enabled = false; }

    public void OpenKickCombo() { valdy.OpenComboKick(); }
    public void CloseKickCombo() { valdy.CloseComboKick(); }

    public void GainHA() { valdy.GainHA(); }
    public void LoseHA() { valdy.LoseHA(); }
         
}