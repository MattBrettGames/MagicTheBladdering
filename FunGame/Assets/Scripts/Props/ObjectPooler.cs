using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPooler : BlankMono
{

    #region Carman
    [Header("Carmen")]
    public GameObject grappler;
    [HideInInspector] public List<GameObject> grapplerList = new List<GameObject>();
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

            #region Carman Props
            grapplerList.Add(Instantiate<GameObject>(grappler, transform.position, Quaternion.identity, transform));
            grapplerList[i].SetActive(false);

            #endregion

            #region Wiosna Props
            cloneList.Add(Instantiate(flamingClone, Vector3.zero, Quaternion.identity));
            cloneList[i].SetActive(false);
            cloneList[i].name = "FlamingClone" + i;       

            cloneExplosionList.Add(Instantiate(cloneExplosion, Vector3.zero, Quaternion.identity));
            cloneExplosionList[i].SetActive(false);

            #endregion

            #region Valderheim Props
            crackList.Add(Instantiate(cracks, Vector3.zero, Quaternion.identity));
            crackList[i].SetActive(false);
            #endregion

        }
    }
}