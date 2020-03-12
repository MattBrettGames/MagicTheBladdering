using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarmenEventHandler : MonoBehaviour
{

    public Carmen carmen;
    public Weapons yAttackHitBox;
    public Weapons spinSphere;
    [SerializeField] ParticleSystem leftWeaponParticles;
    [SerializeField] ParticleSystem rightWeaponParticles;

    public void NewStart()
    {
        leftWeaponParticles.gameObject.SetActive(true);
        rightWeaponParticles.gameObject.SetActive(true);
        EndBladeTrails();
    }

    void Start()
    {
        leftWeaponParticles.gameObject.SetActive(true);
        rightWeaponParticles.gameObject.SetActive(true);
        EndBladeTrails();
    }

    public void BeginYAttack()
    {
        yAttackHitBox.StartAttack();
        StartBladeTrails();

    }
    public void EndYAttack()
    {
        yAttackHitBox.EndAttack();
        EndBladeTrails();
    }

    #region Sound
    public void PlaySound(AudioClip clipToPlay)
    {
        carmen.PlaySound(clipToPlay, null);
    }
    public void PlaySoundFromArray(AudioClip[] clipsToPlay)
    {
        carmen.PlaySound(clipsToPlay);
    }
    #endregion

    public void StartBladeTrails()
    {
        leftWeaponParticles.Clear();
        rightWeaponParticles.Clear();
        leftWeaponParticles.Play();
        rightWeaponParticles.Play();
    }

    public void EndBladeTrails()
    {
        leftWeaponParticles.Stop();
        rightWeaponParticles.Stop();
    }

    public void spinSphereOn() { spinSphere.StartAttack(); }
    public void spinSphereOff() { spinSphere.EndAttack(); }
    public void Vibration(float intensity, float dur) { carmen.ControllerRumble(intensity, dur, false, null); }

    public void GainIFrames() { carmen.GainIFrames(); }
    public void LoseIFrames() { carmen.LoseIFrames(); }

    public void BeginActing() { carmen.BeginActing(); }
    public void EndActing() { carmen.EndActing(); }

    public void EndDodge() { carmen.StopKnockback(); }


}
