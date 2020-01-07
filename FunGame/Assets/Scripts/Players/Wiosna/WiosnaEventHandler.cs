using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WiosnaEventHandler : BlankMono
{

    public Wiosna wiosna;
    public Weapons melee;
    public Weapons explosion;

    void Start()
    {
        melee.gameObject.SetActive(false);
        explosion.gameObject.SetActive(false);
    }


    public void BeginMelee() { melee.gameObject.SetActive(true); melee.StartAttack(); Invoke("EndMelee", 0.4f); }
    public void EndMelee() { melee.EndAttack(); melee.gameObject.SetActive(false); }

    public void BeginExplosion() { explosion.gameObject.SetActive(true); melee.StartAttack(); Invoke("EndExplosion", 0.4f); }
    public void EndExplosion() { explosion.gameObject.SetActive(false); melee.EndAttack(); }

    public void BeginActing() { wiosna.BeginActing(); }
    public void EndActing() { wiosna.EndActing(); }

    public void Vibration(float intensity, float dur) { wiosna.ControllerRumble(intensity, dur); }
    public void GainIFrmaes() { wiosna.GainIFrames(); }
    public void LoseIFrmaes() { wiosna.LoseIFrames(); }
}