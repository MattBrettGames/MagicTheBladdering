using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WiosnaEventHandler : BlankMono
{

    public Wiosna wiosna;
    public Weapons melee;

    void Start()
    {
        melee.gameObject.SetActive(false);
    }

    public void BeginMelee() { melee.gameObject.SetActive(true); melee.StartAttack(); Invoke("EndMelee", 0.4f); }
    public void EndMelee() { melee.EndAttack(); melee.gameObject.SetActive(false); }

    public void BeginActing() { wiosna.BeginActing(); }
    public void EndActing() { wiosna.EndActing(); }

    public void SummonClone() 
    {
        wiosna.SummonClone(); 
    }

    public void Vibration(float intensity, float dur) { wiosna.ControllerRumble(intensity, dur); }
    public void GainIFrmaes() { wiosna.GainIFrames(); }
    public void LoseIFrmaes() { wiosna.LoseIFrames(); }

    #region Sound
    public void PlaySound(AudioClip clipToPlay)
    {
        wiosna.PlaySound(clipToPlay);
    }
    public void PlaySoundFromArray(AudioClip[] clipsToPlay)
    {
        wiosna.PlaySound(clipsToPlay);
    }
    #endregion

}