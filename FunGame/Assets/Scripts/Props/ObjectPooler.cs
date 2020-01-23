using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPooler : BlankMono
{
    #region Songbird
    [Header("Songbird Inputs")]
    public GameObject poisonSmoke;
    public GameObject cannister;

    [HideInInspector] public List<GameObject> poisonSmokeList = new List<GameObject>();
    [HideInInspector] public List<GameObject> cannisters = new List<GameObject>();

    public void ReturnToPoisonSmokePool(GameObject gameobject) { poisonSmokeList.Add(gameobject); gameobject.transform.position = transform.position; gameobject.SetActive(false); }

    #endregion

    #region Carman
    [Header("Carmen")]
    public GameObject grappler;
    [HideInInspector] public List<GameObject> grapplerList = new List<GameObject>();
    #endregion

    #region Skjegg
    [Header("Skjegg Input")]
    public GameObject ghost;

    [HideInInspector] public List<GameObject> ghostList = new List<GameObject>();
    public void ReturnToGhostList(GameObject gameobject) { ghostList.Add(gameobject); gameobject.transform.position = transform.position; gameobject.SetActive(false); }
    #endregion

    #region Wiosna
    [Header("Wisona")]
    public GameObject flamingClone;
    [HideInInspector] public List<GameObject> cloneList = new List<GameObject>();

    public GameObject explosion;
    [HideInInspector] public List<GameObject> blastList = new List<GameObject>();

    public GameObject cloneExplosion;
    [HideInInspector] public List<GameObject> cloneExplosionList = new List<GameObject>();

    #endregion

    #region Valderheim
    [Header("Valderheim")]
    [SerializeField] GameObject cracks;
    [HideInInspector] public List<GameObject> crackList = new List<GameObject>();

    #endregion

    private void Start()
    {
        for (int i = 0; i < 16; i++)
        {
            #region Songbird Props
            poisonSmokeList.Add(Instantiate<GameObject>(poisonSmoke, transform.position, Quaternion.identity, transform));
            poisonSmokeList[i].SetActive(false);

            cannisters.Add(Instantiate<GameObject>(cannister, transform.position, Quaternion.identity, transform));
            cannisters[i].SetActive(false);
            #endregion

            #region Carman Props
            grapplerList.Add(Instantiate<GameObject>(grappler, transform.position, Quaternion.identity, transform));
            grapplerList[i].SetActive(false);

            #endregion

            #region Wiosna Props
            cloneList.Add(Instantiate(flamingClone, Vector3.zero, Quaternion.identity));
            cloneList[i].SetActive(false);
            cloneList[i].name = "FlamingClone" + i;

            blastList.Add(Instantiate(explosion, Vector3.zero, Quaternion.identity));
            blastList[i].SetActive(false);
            blastList[i].GetComponent<WiosnaExplosions>().Setup();

            cloneExplosionList.Add(Instantiate(cloneExplosion, Vector3.zero, Quaternion.identity));
            cloneExplosionList[i].SetActive(false);

            #endregion

            #region Valderheim Props
            crackList.Add(Instantiate(cracks, Vector3.zero, Quaternion.identity));
            crackList[i].SetActive(false);
            #endregion

        }

        for (int i = 0; i < 15; i++)
        {
            #region Skjegg & WarBanner Props
            ghostList.Add(Instantiate<GameObject>(ghost, transform.position, Quaternion.identity));
            ghostList[i].SetActive(false);
            #endregion
        }
    }

    public GameObject ReturnSmokeCloud(int listIndex)
    {
        GameObject poisonSmoke = poisonSmokeList[listIndex];
        poisonSmokeList.RemoveAt(listIndex);
        return poisonSmoke;

    }
}