using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using Rewired;

public class CharacterSelector : BlankMono
{
    public string thisPlayer;
    public int thisPInt;

    private string horiPlayerInput;
    private string vertPlayerInput;
    private Player player;
    int currentChar;
    int currentSkin;

    private bool inputCooldown;

    [Header("Skin Inputter")]
    public List<ModelList> characters = new List<ModelList>();

    [Header("Readying Up")]
    public bool locked;
    [HideInInspector] public UniverseController universe;
    public Camera cam;
    public float camFOVLocked;
    private float camFOVBase;
    Vector3 camOffsetBase;
    [SerializeField] Vector3 camOffsetLocked;
    GameObject store;
    GameObject backStore;

    [Header("UI Elements")]
    public GameObject characterText;
    public Text skinText;
    public GameObject displayChar;
    public GameObject characterCover;

    [Header("Background Images")]
    public GameObject[] backImages = new GameObject[2];
    public GameObject displayImage;

    void Start()
    {
        store = transform.GetChild(0).gameObject;
        backStore = transform.parent.GetChild(1).gameObject;
        camOffsetBase = store.transform.position;
        camFOVBase = cam.fieldOfView;
        transform.tag = thisPlayer + "Selector";
        horiPlayerInput = thisPlayer + "Horizontal";
        vertPlayerInput = thisPlayer + "Vertical";
        displayChar.SetActive(true);
        displayImage.SetActive(true);
        skinText.text = characters[currentChar].skins[currentSkin].name.ToString();
        characterText = characters[currentChar].name;
        player = ReInput.players.GetPlayer(thisPInt);
    }

    void Update()
    {
        if (!locked)
        {
            if (player.GetAxis("HoriMove") >= 0.4f && !inputCooldown)
            {
                currentSkin = 0;
                inputCooldown = true;
                if (currentChar < characters.Count - 1)
                {
                    for (int i = 0; i < 72; i++) { StartCoroutine(SpinTrigger(5, i)); }
                    currentChar++;
                    UpdateDisplay();
                }
                else
                {
                    for (int i = 0; i < 72; i++) { StartCoroutine(SpinTrigger(5, i)); }
                    currentChar = 0;
                    UpdateDisplay();
                }
                Invoke("EndCooldown", 0.3f);
                currentSkin = 0;
            }
            if (player.GetAxis("HoriMove") <= -0.4f && !inputCooldown)
            {
                inputCooldown = true;
                if (currentChar != 0)
                {
                    for (int i = 0; i < 72; i++) { StartCoroutine(SpinTrigger(-5, i)); }
                    currentChar--;
                    currentSkin = 0;
                    UpdateDisplay();
                }
                else
                {
                    for (int i = 0; i < 72; i++) { StartCoroutine(SpinTrigger(-5, i)); }
                    currentChar = characters.Count - 1;
                    currentSkin = 0;
                    UpdateDisplay();
                }
                Invoke("EndCooldown", 0.3f);
            }

            if (player.GetAxis("VertMove") >= 0.4f && !inputCooldown || Input.GetKeyDown(KeyCode.UpArrow))
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
            if (player.GetAxis("VertMove") <= -0.4f && !inputCooldown || Input.GetKeyDown(KeyCode.DownArrow))
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

            if (player.GetButtonDown("AAction")|| Input.GetKeyDown(KeyCode.H))
            {
                if (!characters[currentChar].skins[currentSkin].lockedChar)
                {
                    backStore.transform.position += camOffsetLocked;
                    //store.transform.position += camOffsetLocked;
                    cam.fieldOfView = camFOVLocked;

                    displayChar.transform.rotation = new Quaternion(0, 0, 0, 0);
                    universe.CheckReady(thisPInt, displayChar, characters[currentChar].name, characters[currentChar].skins[currentSkin].name);

                    locked = true;
                }
            }
        }
        if (player.GetButtonDown("BAttack"))
        {
            if (!locked)
            {
                universe.ReturnToMenu();
            }
            else
            {
                locked = false;
                universe.Unlock(thisPInt);
                Unlock();
            }
        }
    }

    private void Unlock()
    {
        store.transform.position = camOffsetBase;
        //backStore.transform.position = camOffsetBase;
        cam.fieldOfView = camFOVBase;
    }

    private IEnumerator SpinTrigger(float angle, float time)
    {
        yield return new WaitForSeconds(time / 200);
        SpinJuice(angle);
    }
    private void SpinJuice(float angle)
    {
        gameObject.transform.Rotate(new Vector3(0, angle, 0));
    }

    public void UpdateDisplay()
    {
        skinText.text = characters[currentChar].skins[currentSkin].name.ToString();
        characterText.SetActive(false);
        characterText = characters[currentChar].name;
        characterText.SetActive(true);

        displayImage.SetActive(false);
        displayImage = backImages[currentChar];
        displayImage.SetActive(true);

        characterCover.SetActive(characters[currentChar].skins[currentSkin].lockedChar);

        displayChar.SetActive(false);
        displayChar = characters[currentChar].skins[currentSkin].Skin;
        if (displayChar.GetComponent<CapsuleCollider>() != null)
        {
            displayChar.GetComponent<CapsuleCollider>().isTrigger = true;
        }

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
        public GameObject name;
        public List<SkinFo> skins;
    }

    [Serializable]
    public struct SkinFo
    {
        public string name;
        public GameObject Skin;
        public bool lockedChar;
    }

}
