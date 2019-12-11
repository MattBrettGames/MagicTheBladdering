using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ArenaSelector : BlankMono
{
    private bool inputCooldown;
    public List<GameObject> displays;
    public List<Vector3> camPos;
    public List<Vector3> camAnglesOffset;
    [Space]
    [SerializeField] float targetWidth;


    [Header("UI")]
    public Text arenaName;


    private int currentDisplay;
    List<Outline> displayOutlines = new List<Outline>();
    private UniverseController universe;

    void Start()
    {
        for (int i = 0; i < displays.Count; i++)
        {
            print(displays[i].GetComponent<Outline>());
            displayOutlines.Add(displays[i].GetComponent<Outline>());
            displayOutlines[i].OutlineWidth = 0;
        }
        universe = GameObject.FindGameObjectWithTag("UniverseController").GetComponent<UniverseController>();
    }


    void Update()
    {
        Camera.main.transform.LookAt(displays[currentDisplay].transform.position + camAnglesOffset[currentDisplay]);
        Camera.main.transform.position = Vector3.Lerp(Camera.main.transform.position, camPos[currentDisplay], 0.16f);

        if (Input.GetAxis("AllHorizontal") >= 0.4f && !inputCooldown)
        {
            if (currentDisplay < displays.Count - 1)
            {
                displayOutlines[currentDisplay].enabled = false;
                currentDisplay++;
                displayOutlines[currentDisplay].enabled = true;
                arenaName.text = displays[currentDisplay].name;
            }
            else
            {
                displayOutlines[currentDisplay].enabled = false;
                currentDisplay = 0;
                displayOutlines[currentDisplay].enabled = true;
                arenaName.text = displays[currentDisplay].name;
            }
            inputCooldown = true;
            Invoke("EndCooldown", 0.3f);
        }
        if (Input.GetAxis("AllVertical") <= -0.4f && !inputCooldown)
        {
            if (currentDisplay != 0)
            {
                displayOutlines[currentDisplay].enabled = false;
                currentDisplay--;
                displayOutlines[currentDisplay].enabled = true;
                arenaName.text = displays[currentDisplay].name;
            }
            else
            {
                displayOutlines[currentDisplay].enabled = false;
                currentDisplay = displays.Count - 1;
                displayOutlines[currentDisplay].enabled = true;
                arenaName.text = displays[currentDisplay].name;
            }
            inputCooldown = true;
            Invoke("EndCooldown", 0.3f);
        }

        if (Input.GetButtonDown("AllAButton"))
        {
            universe.ChooseArena(displays[currentDisplay].name);
        }

        if (Input.GetKeyDown(KeyCode.G))
        {
            universe.ChooseArena(displays[currentDisplay].name);
        }

    }

    void EndCooldown()
    {
        inputCooldown = false;
    }

}
