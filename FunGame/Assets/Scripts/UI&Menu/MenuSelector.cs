using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuSelector : MonoBehaviour
{
    [Header("Options")]
    public GameObject[] options = new GameObject[2];
    public GameObject cursor;

    [Header("Components")]
    public UniverseController universe;

    private bool inputOnCooldown;
    int currentSel;

    void Start() { cursor.transform.position = options[currentSel].transform.position; }

    void Update()
    {
        if (Input.GetAxis("AllVertical") >= 0.4 && !inputOnCooldown)
        {
            inputOnCooldown = true;
            Invoke("EndCooldown", 0.2f);
            if (currentSel >= options.Length - 1) { currentSel = 0; }
            else { currentSel++; }
            cursor.transform.position = options[currentSel].transform.position;
        }
        if (Input.GetAxis("AllVertical") <= -0.4 && !inputOnCooldown)
        {
            inputOnCooldown = true;
            Invoke("EndCooldown", 0.2f);
            if (currentSel <= 0) { currentSel = options.Length-1; }
            else { currentSel--; }
            cursor.transform.position = options[currentSel].transform.position;
        }
        if (Input.GetButtonDown("AllAButton"))
        {
            Invoke(options[currentSel].name, 0);
        }
    }

    void Duel() { universe.SelectedPlay(); }
    void Bios() { universe.SelectedBios(); }
    void Adventure() { universe.SelectedAdventure(); }
    void EndCooldown() { inputOnCooldown = false; }
}