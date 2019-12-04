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
    private bool onMovesets;
    [SerializeField] string inputButtonCaps = "A";
    string inputToSwitch;
    private void Start()
    {
        displays[currentDisplay].SetActive(true);
        inputToSwitch = "All" + inputButtonCaps + "Button";
    }

    void Update()
    {
        if (Input.GetButtonDown(inputToSwitch) && !inputCooldown)
        {
            displays[currentDisplay].SetActive(!onMovesets);
            movesetDisplays[currentDisplay].SetActive(onMovesets);
            onMovesets = !onMovesets;
        }

        if (Input.GetAxis("AllVertical") >= 0.4f && !inputCooldown)
        {
            if (currentDisplay < displays.Count - 1)
            {
                movesetDisplays[currentDisplay].SetActive(false);
                displays[currentDisplay].SetActive(false);
                currentDisplay++;
                displays[currentDisplay].SetActive(true);
            }
            else
            {
                movesetDisplays[currentDisplay].SetActive(false);
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
                movesetDisplays[currentDisplay].SetActive(false);
                displays[currentDisplay].SetActive(false);
                currentDisplay--;
                displays[currentDisplay].SetActive(true);
            }
            else
            {
                movesetDisplays[currentDisplay].SetActive(false);
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
