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

    [SerializeField] float fovLerpSpeed;

    private bool inputCooldown;

    [Header("Skin Inputter")]
    public List<ModelList> characters = new List<ModelList>();

    [Header("Readying Up")]
    public bool locked;
    [HideInInspector] public UniverseController universe;
    [SerializeField] Vector3 targetLookatForChar;
    public Camera cam;
    public float camFOVLocked;
    private float camFOVBase;
    Vector3 camOffsetBase;
    [SerializeField] Vector3 camOffsetLocked;
    GameObject store;
    GameObject backStore;
    [SerializeField] CharacterSelector otherChar1;
    [SerializeField] CharacterSelector otherChar2;
    [SerializeField] CharacterSelector otherChar3;

    [Header("Sounds")]
    [SerializeField] AudioClip[] chosenSounds = new AudioClip[0];
    private AudioSource audioSource;

    [Header("UI Elements")]
    public GameObject characterText;
    public Text skinText;
    public GameObject displayChar;
    public GameObject characterCover;
    public GameObject[] characterAblityArray = new GameObject[4];

    [Header("Background Images")]
    public GameObject[] backImages = new GameObject[2];
    public GameObject displayImage;

    Vector3 lockedForward;

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

        universe = GameObject.Find("UniverseController").GetComponent<UniverseController>();
        audioSource = gameObject.AddComponent<AudioSource>();

        currentChar = UnityEngine.Random.Range(0, characters.Count);
        while (currentChar == otherChar1.currentChar && thisPInt != 0)
        {
            currentChar = UnityEngine.Random.Range(0, characters.Count);
        }

        UpdateDisplay();
    }

    void Update()
    {
        if (!locked)
        {
            if (player.GetAxis("HoriMove") >= 0.4f && !inputCooldown)
            {
                characterAblityArray[currentChar].SetActive(false);
                currentSkin = 0;
                inputCooldown = true;
                if (currentChar < characters.Count - 1)
                {
                    for (int i = 0; i < 72; i++) { StartCoroutine(SpinTrigger(5, i)); }
                    currentChar++;
                    UpdateDisplay(0.2f);
                }
                else
                {
                    for (int i = 0; i < 72; i++) { StartCoroutine(SpinTrigger(5, i)); }
                    currentChar = 0;
                    UpdateDisplay(0.2f);
                }
                Invoke("EndCooldown", 0.3f);
                currentSkin = 0;
            }
            if (player.GetAxis("HoriMove") <= -0.4f && !inputCooldown)
            {
                characterAblityArray[currentChar].SetActive(false);
                currentSkin = 0;
                inputCooldown = true;
                if (currentChar != 0)
                {
                    for (int i = 0; i < 72; i++) { StartCoroutine(SpinTrigger(-5, i)); }
                    currentChar--;
                    UpdateDisplay(0.2f);
                }
                else
                {
                    for (int i = 0; i < 72; i++) { StartCoroutine(SpinTrigger(-5, i)); }
                    currentChar = characters.Count - 1;
                    UpdateDisplay(0.2f);
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

            if (player.GetButtonDown("AAction") || Input.GetKeyDown(KeyCode.H))
            {
                if (!characters[currentChar].skins[currentSkin].lockedChar)
                {
                    audioSource.PlayOneShot(chosenSounds[currentChar]);
                    characterAblityArray[currentChar].SetActive(false);
                    LockInCharacter();
                }
            }

            if (player.GetButtonDown("YAttack"))
            {
                characterAblityArray[currentChar].SetActive(false);

                currentChar = UnityEngine.Random.Range(0, characters.Count);
                currentSkin = UnityEngine.Random.Range(0, characters[currentChar].skins.Count);

                UpdateDisplay();
            }
            if (player.GetButtonDown("XAttack"))
            {
                characterAblityArray[currentChar].SetActive(!characterAblityArray[currentChar].activeSelf);
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
                characters[otherChar1.currentChar].skins[otherChar1.currentSkin].Skin.GetComponent<PlayerBase>().isAI = false;
                locked = false;
                Unlock();
                universe.Unlock(thisPInt);
            }
        }
        if (player.GetAxis("LockOn") >= 0.4f && !inputCooldown)
        {
            PlayerBase target = otherChar1.characters[otherChar1.currentChar].skins[otherChar1.currentSkin].Skin.GetComponent<PlayerBase>();
            target.isAI = true;
            otherChar1.LockInCharacter();
            inputCooldown = true;
            Invoke("EndCooldown", 0.3f);
        }
    }

    private void LockInCharacter()
    {
        print(name + " has had LockIn called");
        backStore.transform.position += camOffsetLocked;
        cam.fieldOfView = camFOVLocked;
        lockedForward = displayChar.transform.forward;
        store.transform.eulerAngles = targetLookatForChar;

        StartCoroutine(StartLoad());

        locked = true;
    }

    IEnumerator StartLoad()
    {
        otherChar1.characters[currentChar].skins[currentSkin].lockedChar = true;
        otherChar1.UpdateDisplay();

        if (otherChar2 != null)
        {
            otherChar2.characters[currentChar].skins[currentSkin].lockedChar = true;
            otherChar2.UpdateDisplay();
        }
        if (otherChar3 != null)
        {
            otherChar3.characters[currentChar].skins[currentSkin].lockedChar = true;
            otherChar3.UpdateDisplay();
        }

        yield return new WaitForSeconds(0);
        displayChar.transform.SetParent(GameObject.FindGameObjectWithTag("UniverseController").transform);
        universe.CheckReady(thisPInt, displayChar, characters[currentChar].name, characters[currentChar].skins[currentSkin].name);
    }

    private void Unlock()
    {
        otherChar1.characters[currentChar].skins[currentSkin].lockedChar = false;
        otherChar1.UpdateDisplay();

        if (otherChar2 != null)
        {
            otherChar2.characters[currentChar].skins[currentSkin].lockedChar = true;
            otherChar2.UpdateDisplay();
        }
        if (otherChar3 != null)
        {
            otherChar3.characters[currentChar].skins[currentSkin].lockedChar = true;
            otherChar3.UpdateDisplay();
        }

        displayChar.transform.SetParent(store.transform);
        store.transform.forward = lockedForward;
        displayChar.transform.position = camOffsetBase;
        cam.fieldOfView = camFOVBase;
        locked = false;
    }

    private IEnumerator SpinTrigger(float angle, float time)
    {
        yield return new WaitForSecondsRealtime(time * 0.005f);
        SpinJuice(angle);
    }
    private void SpinJuice(float angle)
    {
        gameObject.transform.Rotate(new Vector3(0, angle, 0));
    }

    public void UpdateDisplay(float time)
    {
        StartCoroutine(DelayedDisplay(time));
    }
    public void UpdateDisplay()
    {
        StartCoroutine(DelayedDisplay(0));
    }

    IEnumerator DelayedDisplay(float time)
    {
        yield return new WaitForSeconds(time);
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



    public void SetUniverse(UniverseController universeTemp) { universe = universeTemp; }

    void EndCooldown() { inputCooldown = false; }

    [Serializable]
    public struct ModelList
    {
        public GameObject name;
        public List<SkinFo> skins;
    }
}

[Serializable]
public class SkinFo
{
    public CharacterSelector charSel;
    public string name;
    public GameObject Skin;
    public bool lockedChar;
}
