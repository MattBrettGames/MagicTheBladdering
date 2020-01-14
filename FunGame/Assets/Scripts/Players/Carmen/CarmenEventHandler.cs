using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarmenEventHandler : MonoBehaviour
{

    public Carmen carmen;
    public Weapons leftDagger;
    public Weapons spinSphere;

    public void BothBeginAttack() { leftDagger.StartAttack(); }
    public void BothEndAttack() { leftDagger.EndAttack(); }

    public void spinSphereOn() { spinSphere.StartAttack(); }
    public void spinSphereOff() { spinSphere.EndAttack(); }
    public void Vibration(float intensity, float dur) { carmen.ControllerRumble(intensity, dur); }
    public void GainIFrames() { carmen.GainIFrames(); }
    public void LoseIFrames() { carmen.LoseIFrames(); }

    public void BeginActing() { carmen.BeginActing(); }
    public void EndActing() { carmen.EndActing(); }

    public void EndDodge() { carmen.StopKnockback(); }


}
