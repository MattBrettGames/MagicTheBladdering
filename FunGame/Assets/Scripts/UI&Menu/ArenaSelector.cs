using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArenaSelector : BlankMono
{
    private bool inputCooldown;
    public List<GameObject> displays;
    private int currentDisplay;

    void Update()
    {
        if (Input.GetAxis("AllHorizontal") >= 0.4f && !inputCooldown)
        {
            if (currentDisplay < displays.Count - 1)
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

        if (Input.GetButtonDown("AllXButton"))
        {
            UniverseController universe = GameObject.FindGameObjectWithTag("UniverseController").GetComponent<UniverseController>();
            universe.ChooseArena(displays[currentDisplay].name);
        }
    }

    void EndCooldown()
    {
        inputCooldown = false;
    }

}
