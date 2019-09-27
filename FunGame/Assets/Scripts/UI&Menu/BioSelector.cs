using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BioSelector : BlankMono
{

    public List<GameObject> displays = new List<GameObject>();
    private int currentDisplay;
    private bool inputCooldown;

    private void Start()
    {
        displays[currentDisplay].SetActive(true);
    }

    void Update()
    {
        if (Input.GetAxis("AllHorizontal") >= 0.4f && !inputCooldown)
        {
            if (currentDisplay < displays.Count-1)
            {
                displays[currentDisplay].SetActive(false);
                currentDisplay++;
                displays[currentDisplay].SetActive(true);
            }
            else
            {
                displays[currentDisplay].SetActive(false);
                currentDisplay = 0;
                displays[currentDisplay].SetActive(true);
            }
            inputCooldown = true;
            Invoke("EndCooldown", 0.3f);
        }
        if (Input.GetAxis("AllVertical") <= -0.4f && !inputCooldown)
        {
            if (currentDisplay != 0)
            {
                displays[currentDisplay].SetActive(false);
                currentDisplay--;
                displays[currentDisplay].SetActive(true);
            }
            else
            {
                displays[currentDisplay].SetActive(false);
                currentDisplay = displays.Count - 1;
                displays[currentDisplay].SetActive(true);
            }
            inputCooldown = true;
            Invoke("EndCooldown", 0.3f);
        }

    }

    void EndCooldown()
    {
        inputCooldown = false;
    }
}
