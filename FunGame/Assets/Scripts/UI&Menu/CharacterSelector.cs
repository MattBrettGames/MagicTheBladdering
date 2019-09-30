using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class CharacterSelector : BlankMono
{
    public string thisPlayer;

    private string horiPlayerInput;
    private string vertPlayerInput;

    public List<ModelList> characters = new List<ModelList>();

    int currentChar;
    int currentSkin;

    private bool inputCooldown;

    [Header("Readying Up")]
    public bool locked;
    private UniverseController universe;

    [Header("UI Elements")]
    public Text characterText;
    public Text skinText;
    public GameObject displayChar;

    void Start()
    {
        transform.tag = thisPlayer + "Selector";
        horiPlayerInput = thisPlayer + "Horizontal";
        vertPlayerInput = thisPlayer + "Vertical";
        UpdateDisplay();
        displayChar.SetActive(true);
    }

    void Update()
    {
        if (!locked)
        {
            if (Input.GetAxis(horiPlayerInput) >= 0.4f && !inputCooldown)
            {
                inputCooldown = true;
                if (currentChar < characters.Count - 1)
                {
                    currentSkin = 0;
                    currentChar++;
                    UpdateDisplay();
                }
                else
                {
                    currentSkin = 0;
                    currentChar = 0;
                    UpdateDisplay();
                }
                Invoke("EndCooldown", 0.3f);
                currentSkin = 0;
            }
            if (Input.GetAxis(horiPlayerInput) <= -0.4f && !inputCooldown)
            {
                inputCooldown = true;
                if (currentChar != 0)
                {
                    currentChar--;
                    currentSkin = 0;
                    UpdateDisplay();
                }
                else
                {
                    currentChar = characters.Count - 1;
                    currentSkin = 0;
                    UpdateDisplay();
                }
                Invoke("EndCooldown", 0.3f);
            }

            if (Input.GetAxis(vertPlayerInput) >= 0.4f && !inputCooldown)
            {
                inputCooldown = true;
                if (currentSkin < characters[currentChar].skins.Count - 1)
                {
                    currentSkin++;
                    UpdateDisplay();
                }
                else
                {
                    currentSkin = 0;
                    UpdateDisplay();
                }
                Invoke("EndCooldown", 0.3f);
            }
            if (Input.GetAxis(vertPlayerInput) <= -0.4f && !inputCooldown)
            {
                inputCooldown = true;
                if (currentSkin != 0)
                {
                    currentSkin--;
                    UpdateDisplay();
                }
                else
                {
                    currentSkin = characters[currentChar].skins.Count - 1;
                    UpdateDisplay();
                }
                Invoke("EndCooldown", 0.3f);
            }

            if (Input.GetButtonDown(thisPlayer + "XButton"))
            {
                locked = true;
                UpdateDisplay();
                universe.CheckReady();
            }
        }
    }

    public void UpdateDisplay()
    {
        skinText.text = characters[currentChar].skins[currentSkin].name.ToString();
        characterText.text = characters[currentChar].name.ToString();
        displayChar.SetActive(false);
        displayChar = characters[currentChar].skins[currentSkin].Skin;
        displayChar.SetActive(true);
    }

    public void SetUniverse(UniverseController universeTemp)
    {
        universe = universeTemp;
    }

    void EndCooldown()
    {
        inputCooldown = false;
    }

    [Serializable]
    public struct ModelList
    {
        public string name;
        public List<SkinFo> skins;
    }

    [Serializable]
    public struct SkinFo
    {
        public string name;
        public GameObject Skin;
    }

}
