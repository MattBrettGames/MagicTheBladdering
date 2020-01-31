using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireJetHandler : MonoBehaviour
{
    [SerializeField]
    private List<GameObject> JetObj = new List<GameObject>(); //create class for these

    [SerializeField]
    private float maxTime = 15f;
    [SerializeField]  //time between outbursts
    private float minTime = 8f;
    [SerializeField]
    private float maxDuration = 5f;
    [SerializeField]     //random between these two values. length of outburst
    private float minDuration = 3f;

    [SerializeField]
    private int maxActive = 3;   //max no active at once.

    [SerializeField]
    private int maxToActivate = 5;
    public int currentActive = 0;

    void Start()
    {
        StartCoroutine(outburstTimer()); //get going.
    }


    public void SetInactive()
    {
        currentActive -= 1;
    }

    IEnumerator outburstTimer()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(minTime, maxTime));
            int currentactivating = 0;
            int toActivate = Random.Range(1, maxToActivate + 1);

            toActivate -= currentActive;
            if (currentActive != maxActive)
            {
                while (currentactivating < toActivate)
                {
                    GameObject go = JetObj[Random.Range(0, JetObj.Count)];
                    FireJet fj = go.GetComponent<FireJet>();
                    if (!fj.isActive && currentActive < maxActive)
                    {
                        currentactivating += 1;
                        fj.setActive(Random.Range(minDuration, maxDuration));
                        currentActive += 1;
                    }
                }
            }
            currentactivating = 0;
        }
            
        }
            
}
