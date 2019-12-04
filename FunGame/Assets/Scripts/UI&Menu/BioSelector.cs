using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BioSelector : BlankMono
{

    public List<GameObject> displays = new List<GameObject>();
    public List<GameObject> movesetDisplays = new List<GameObject>();
    private int currentDisplay;
    private bool inputCooldown;
    private bool inputCooldownButton;
    private bool onMovesets;
    [SerializeField] string inputButtonCaps = "A";
    string inputToSwitch;

    private void Start()
    {
        displays[currentDisplay].SetActive(true);
        onMovesets = false;
        inputToSwitch = "All" + inputButtonCaps + "Button";
    }

    void Update()
    {
        if (Input.GetButtonDown(inputToSwitch))
        {
            onMovesets = !onMovesets;
            //UpdateMoveset();
        }

        if (Input.GetAxis("AllVertical") >= 0.4f && !inputCooldown)
        {
            if (currentDisplay < displays.Count - 1)
            {
                if (onMovesets) onMovesets = false;
                displays[currentDisplay].SetActive(false);
                currentDisplay++;
                displays[currentDisplay].SetActive(true);
            }
            else
            {
                if (onMovesets) onMovesets = false;
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
                if (onMovesets) onMovesets = false;
                displays[currentDisplay].SetActive(false);
                currentDisplay--;
                displays[currentDisplay].SetActive(true);
            }
            else
            {
                if (onMovesets) onMovesets = false;
                displays[currentDisplay].SetActive(false);
                currentDisplay = displays.Count - 1;
                displays[currentDisplay].SetActive(true);
            }
            inputCooldown = true;
            Invoke("EndCooldown", 0.3f);
        }
        if (onMovesets)
        {
            movesetDisplays[currentDisplay].SetActive(true);
        }
        else
        {
            movesetDisplays[currentDisplay].SetActive(false);
        }
    }

    private void UpdateMoveset()
    {
        if (!inputCooldownButton)
        {
            displays[currentDisplay].SetActive(!onMovesets);
            movesetDisplays[currentDisplay].SetActive(onMovesets);
            //print(onMovesets);
            onMovesets = !onMovesets;
            inputCooldownButton = true;
            Invoke("EndCooldownButton", 0.3f);
        }
    }

    void EndCooldownButton() { inputCooldownButton = false; }

    void EndCooldown()
    {
        inputCooldown = false;
    }
}
