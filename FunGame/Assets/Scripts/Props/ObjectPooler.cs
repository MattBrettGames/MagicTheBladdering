using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPooler : BlankMono
{
    #region Songbird
    [Header("Songbird Inputs")]
    public GameObject vialModel;
    public GameObject poisonSmokeModel;
    public GameObject adrenalineSmokeModel;
    public GameObject boomSmokeModel;

    [Header("Non Editable Songbird")]
    public List<GameObject> vials = new List<GameObject>();
    public List<GameObject> poisonSmoke = new List<GameObject>();
    public List<GameObject> adrenalineSmoke = new List<GameObject>();
    public List<GameObject> boomSmoke = new List<GameObject>();
    public void ReturnToVialPool(GameObject gameobject) { vials.Add(gameobject); gameobject.transform.position = transform.position; gameobject.SetActive(false); }
    public void ReturnToPoisonSmokePool(GameObject gameobject) { poisonSmoke.Add(gameobject); gameobject.transform.position = transform.position; gameobject.SetActive(false); }
    public void ReturnToAdrenalineSmokePool(GameObject gameobject) { adrenalineSmoke.Add(gameobject); gameobject.transform.position = transform.position; gameobject.SetActive(false); }
    public void ReturnToBoomSmokePool(GameObject gameobject) { boomSmoke.Add(gameobject); gameobject.transform.position = transform.position; gameobject.SetActive(false); }

    #endregion

    #region Carman
    [Header("Carman Editables")]
    public GameObject curseCircle;
    public GameObject curseTrap;
    [Header("Carman DONOTTOUCH")]
    public List<GameObject> curseCircleList = new List<GameObject>();
    public List<GameObject> curseTrapList = new List<GameObject>();
    #endregion

    private void Start()
    {
        for (int i = 0; i < 5; i++)
        {
            #region Songbird Props
            poisonSmoke.Add(Instantiate<GameObject>(poisonSmokeModel, transform.position, Quaternion.identity));
            poisonSmoke[i].SetActive(false);

            adrenalineSmoke.Add(Instantiate<GameObject>(adrenalineSmokeModel, transform.position, Quaternion.identity));
            adrenalineSmoke[i].SetActive(false);

            boomSmoke.Add(Instantiate<GameObject>(boomSmokeModel, transform.position, Quaternion.identity));
            boomSmoke[i].SetActive(false);

            vials.Add(Instantiate<GameObject>(vialModel, transform.position, Quaternion.identity));
            vials[i].SetActive(false);
            #endregion

            #region Carman Props
            curseCircleList.Add(Instantiate<GameObject>(curseCircle, transform.position, Quaternion.identity));
            curseCircleList[i].SetActive(false);

            curseTrapList.Add(Instantiate<GameObject>(curseTrap, transform.position, Quaternion.identity));
            curseTrapList[i].SetActive(false);
            #endregion
        }
    }

}
