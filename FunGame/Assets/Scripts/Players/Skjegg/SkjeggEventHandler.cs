using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkjeggEventHandler : MonoBehaviour
{
    [SerializeField] Skjegg skjegg;
    [SerializeField] Weapons leftFist;
    [SerializeField] Weapons rightFist;

    public void BeginActing() { skjegg.BeginActing(); }
    public void EndActing() { skjegg.EndActing(); }
    
    public void LeftFistOn() { leftFist.StartAttack(); }
    public void LeftFistOff() { leftFist.EndAttack(); }

    public void RightFistOff() { rightFist.EndAttack(); }
    public void RightFistOn() { rightFist.StartAttack(); }

}
