using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WiosnaEventHandler : BlankMono
{

    public Wiosna wiosna;
    public Weapons melee;
    [SerializeField] GameObject fireEffect;
    [SerializeField] Transform targetBone;
    [SerializeField] Vector3 offset;

    void Start()
    {
        fireEffect.SetActive(false);
    }

    void Update()
    {
        fireEffect.transform.position = targetBone.position + offset;
    }

    public void BeginMelee() { melee.gameObject.SetActive(true); melee.StartAttack(); Invoke("EndMelee", 0.4f); }
    public void EndMelee() { melee.EndAttack(); melee.gameObject.SetActive(false); }

    public void BeginActing() { wiosna.BeginActing(); }
    public void EndActing() { wiosna.EndActing(); }

    public void TurnFireOn() 
    {
        if (SceneManager.GetActiveScene().name.Contains("Selector") || SceneManager.GetActiveScene().name.Contains("Menu") || SceneManager.GetActiveScene().name.Contains("Game"))
        {
            fireEffect.transform.localScale = new Vector3(15, 15, 15);
        }
        else
        {
            fireEffect.transform.localScale = new Vector3(1, 1, 1);
        }
        fireEffect.SetActive(true); 
    }
    public void TurnFireOff() 
    {
        fireEffect.SetActive(false); 
    }


    public void DoTheDodge() { wiosna.DotheDodge(); }

    public void SummonClone() { wiosna.SummonClone(); }

    public void Vibration(float intensity, float dur) { wiosna.ControllerRumble(intensity, dur, false, null); }
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