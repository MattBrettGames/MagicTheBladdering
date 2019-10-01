using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPooler : BlankMono
{
    [Header("Songbird")]
    public List<GameObject> vials = new List<GameObject>();
    public List<GameObject> curseSmoke = new List<GameObject>();
    public void ReturnToVialPool(GameObject gameobject) { vials.Add(gameobject); }
    public void ReturnToCurseSmokePool(GameObject gameobject) { curseSmoke.Add(gameobject); }


}
