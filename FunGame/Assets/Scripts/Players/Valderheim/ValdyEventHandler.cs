using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ValdyEventHandler : BlankMono
{
    public Valderheim valdy;
    public Weapons hammer;
    public Weapons chestBox;

    public void ChestBoxOn() { chestBox.GetComponent<BoxCollider>().enabled = true; }
    public void ChestBoxOff() { chestBox.GetComponent<BoxCollider>().enabled = true; }

    public void HammerBoxOn() { hammer.GetComponent<BoxCollider>().enabled = true; }
    public void HammerBoxOff() { hammer.GetComponent<BoxCollider>().enabled = false; }

    public void OpenKickCombo() { valdy.OpenComboKick(); }
    public void CloseKickCombo() { valdy.CloseComboKick(); }
         
}