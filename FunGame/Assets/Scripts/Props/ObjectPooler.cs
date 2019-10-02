using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPooler : BlankMono
{
    [Header("Songbird")]
    public List<GameObject> vials = new List<GameObject>();
    public List<GameObject> poisonSmoke = new List<GameObject>();
    public List<GameObject> adrenalineSmoke = new List<GameObject>();
    public List<GameObject> boomSmoke = new List<GameObject>();
    public void ReturnToVialPool(GameObject gameobject) { vials.Add(gameobject); gameobject.transform.position = transform.position; gameobject.SetActive(false); }
    public void ReturnToPoisonSmokePool(GameObject gameobject) { poisonSmoke.Add(gameobject); gameobject.transform.position = transform.position; gameobject.SetActive(false); }
    public void ReturnToAdrenalineSmokePool(GameObject gameobject) { adrenalineSmoke.Add(gameobject); gameobject.transform.position = transform.position; gameobject.SetActive(false); }
    public void ReturnToBoomSmokePool(GameObject gameobject) { boomSmoke.Add(gameobject); gameobject.transform.position = transform.position; gameobject.SetActive(false); }



}
