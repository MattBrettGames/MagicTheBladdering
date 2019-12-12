using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ArenaSelector : BlankMono
{
    private bool inputCooldown;
    [SerializeField] float speed;
    public List<GameObject> displays;
    public List<Vector3> camPos;
    public List<Vector3> camAnglesOffset;

    [Header("UI")]
    public Text arenaName;

    GameObject cam;
    private int currentDisplay;
    List<Outline> displayOutlines = new List<Outline>();
    private UniverseController universe;

    void Start()
    {
        cam = Camera.main.gameObject;
        for (int i = 0; i < displays.Count; i++)
        {
            displayOutlines.Add(displays[i].GetComponent<Outline>());
            displayOutlines[i].enabled = false;
        }
        displayOutlines[currentDisplay].enabled = true;
        arenaName.text = displays[currentDisplay].name;

        universe = GameObject.FindGameObjectWithTag("UniverseController").GetComponent<UniverseController>();
    }

    void FixedUpdate()
    {
        cam.transform.LookAt(Vector3.Lerp(Camera.main.transform.forward, displays[currentDisplay].transform.position + camAnglesOffset[currentDisplay],Time.deltaTime * speed));
        cam.transform.position = Vector3.Slerp(cam.transform.position, camPos[currentDisplay], Time.deltaTime * speed) ;

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
        if (Input.GetAxis("AllHorizontal") <= -0.4f && !inputCooldown)
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
