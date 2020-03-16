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
    private int maxToActivate = 6;
    [SerializeField]
    private int currentActive;

    void Start()
    {
        StartCoroutine(outburstTimer()); //get going.
        currentActive = 0;
    }


    public void SetInactive()
    {
        Debug.Log("Set inactive");
        currentActive--;      
    }
    public int toActivate;
    public int currentActivating;

    IEnumerator outburstTimer()
    {
        while (true)
        {
            if (currentActive < maxActive)
            {
                yield return new WaitForSeconds(Random.Range(minTime, maxTime));
                currentActivating = 0;
                toActivate = Random.Range(1, (maxToActivate + 1));

                if (currentActive < maxActive && toActivate > 0)
                {
                    if (toActivate + currentActive > maxActive) toActivate = maxActive - currentActive;
                    while (currentActivating < toActivate)
                    {
                        GameObject go = JetObj[Random.Range(0, JetObj.Count)];
                        FireJet fj = go.GetComponent<FireJet>();
                        if (!fj.isActive)
                        {
                            currentActivating += 1;
                            fj.setActive(Random.Range(minDuration, maxDuration));
                            currentActive += 1;
                        }
                    }
                    toActivate = 0;
                }
            }
            else
            {
                yield return new WaitForSeconds(maxDuration);
                currentActive = 0;
            }
            yield return null;
           // currentactivating = 0;
        }

    }

}
