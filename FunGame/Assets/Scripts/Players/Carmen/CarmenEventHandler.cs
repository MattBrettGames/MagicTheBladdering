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

    public void BeginYAttack()
    {
        yAttackHitBox.StartAttack();
        leftWeaponParticles.Clear();
        rightWeaponParticles.Clear();
        leftWeaponParticles.Play();
        rightWeaponParticles.Play();

    }
    public void EndYAttack()
    {
        yAttackHitBox.EndAttack();
        leftWeaponParticles.Stop();
        rightWeaponParticles.Stop();
    }

    public void spinSphereOn() { spinSphere.StartAttack(); }
    public void spinSphereOff() { spinSphere.EndAttack(); }
    public void Vibration(float intensity, float dur) { carmen.ControllerRumble(intensity, dur); }
    public void GainIFrames() { carmen.GainIFrames(); }
    public void LoseIFrames() { carmen.LoseIFrames(); }

    public void BeginActing() { carmen.BeginActing(); }
    public void EndActing() { carmen.EndActing(); }

    public void EndDodge() { carmen.StopKnockback(); }


}
