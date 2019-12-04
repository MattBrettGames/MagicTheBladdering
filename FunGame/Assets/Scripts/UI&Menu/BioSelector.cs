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

    void LateUpdate()
    {
        if (Input.GetButtonDown(inputToSwitch))
        {
            onMovesets = !onMovesets;
        }
        print(currentDisplay);
        print(onMovesets);

        if (Input.GetAxis("AllVertical") >= 0.4f && !inputCooldown)
        {
            onMovesets = false;
            displays[currentDisplay].SetActive(false);

            if (currentDisplay < displays.Count - 1) currentDisplay++;
            else currentDisplay = 0;

            displays[currentDisplay].SetActive(true);
            inputCooldown = true;
            Invoke("EndCooldown", 0.3f);
        }
        if (Input.GetAxis("AllVertical") <= -0.4f && !inputCooldown)
        {
            onMovesets = false;
            displays[currentDisplay].SetActive(false);

            if (currentDisplay != 0) currentDisplay--;
            else currentDisplay = displays.Count - 1;

            displays[currentDisplay].SetActive(true);
            inputCooldown = true;
            Invoke("EndCooldown", 0.3f);
        }
    }

    private void Update()
    {
        for(int i = 0; i < movesetDisplays.Count; i++)
        {
            movesetDisplays[i].SetActive(false);
        }
        movesetDisplays[currentDisplay].SetActive(onMovesets);
    }

    void EndCooldownButton() { inputCooldownButton = false; }

    void EndCooldown()
    {
        inputCooldown = false;
    }
}
