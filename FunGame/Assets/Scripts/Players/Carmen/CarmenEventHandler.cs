using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarmenEventHandler : MonoBehaviour
{

    public Carmen carmen;
    public Weapons leftDagger;
    public Weapons rightDagger;

    public void BothBeginAttack() { leftDagger.StartAttack(); rightDagger.StartAttack(); }
    public void BothEndAttack() { leftDagger.EndAttack(); rightDagger.EndAttack(); }

    public void DigTravel() { carmen.DigTravel(); }

    public void GainIFrames() { carmen.GainIFrames(); }
    public void LoseIFrames() { carmen.LoseIFrames(); }

    public void BeginActing() { carmen.BeginActing(); }
    public void EndActing() { carmen.EndActing(); }

    public void EndDodge() { carmen.StopKnockback(); }


}
