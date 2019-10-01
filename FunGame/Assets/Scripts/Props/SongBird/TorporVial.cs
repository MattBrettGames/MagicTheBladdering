using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TorporVial : Throwables
{
    public float curseDuration;
    public override void CollisionEvent(GameObject collision)
    {
        collision.GetComponent<PlayerBase>().GainCurse(curseDuration);
    }
}
