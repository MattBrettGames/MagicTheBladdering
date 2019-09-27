using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UniverseController : BlankMono
{

    [Header("GameObjects")]
    public CharacterSelector charSelector1;
    public CharacterSelector charSelector2;
    public CharacterSelector charSelector3;
    public CharacterSelector charSelector4;

    [Header("Scenes")]
    public string chosenScene;

    [Header("Character Info")]
    public GameObject lockedCharacter1;
    public GameObject lockedCharacter2;
    public GameObject lockedCharacter3;
    public GameObject lockedCharacter4;

    [Header("Instantiation Info")]
    public List<Vector3> spawnPosList = new List<Vector3>();

    void Start()
    {
        DontDestroyOnLoad(gameObject);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {

        }
        if (SceneManager.GetActiveScene().name == "MainMenu")
        {
            if (Input.GetButtonDown("AllAButton")) { SceneManager.LoadScene("3CharacterSelector"); }
            if (Input.GetButtonDown("AllBButton")) { SceneManager.LoadScene("4CharacterSelector"); }
            if (Input.GetButtonDown("AllXButton")) { SceneManager.LoadScene("2CharacterSelector"); }
            if (Input.GetButtonDown("AllYButton")) { SceneManager.LoadScene("Bio"); }
        }
        if(SceneManager.GetActiveScene().name == "ArenaSelector")
        {
            

        }

    }

    private void OnLevelWasLoaded(int level)
    {
        if(level == 1)
        {
            charSelector1 = GameObject.FindGameObjectWithTag("P1Selector").GetComponent<CharacterSelector>();
            charSelector2 = GameObject.FindGameObjectWithTag("P2Selector").GetComponent<CharacterSelector>();
        }
        else if(level == 2)
        {
            charSelector1 = GameObject.FindGameObjectWithTag("P1Selector").GetComponent<CharacterSelector>();
            charSelector2 = GameObject.FindGameObjectWithTag("P2Selector").GetComponent<CharacterSelector>();
            charSelector3 = GameObject.FindGameObjectWithTag("P3Selector").GetComponent<CharacterSelector>();
        }
        else if (level == 3)
        {
            charSelector1 = GameObject.FindGameObjectWithTag("P1Selector").GetComponent<CharacterSelector>();
            charSelector2 = GameObject.FindGameObjectWithTag("P2Selector").GetComponent<CharacterSelector>();
            charSelector3 = GameObject.FindGameObjectWithTag("P3Selector").GetComponent<CharacterSelector>();
            charSelector4 = GameObject.FindGameObjectWithTag("P4Selector").GetComponent<CharacterSelector>();
        }
    }

    public void CheckReady()
    {
        if (charSelector1.locked && charSelector2.locked)
        {
            lockedCharacter1 = charSelector1.displayChar;
            lockedCharacter2 = charSelector2.displayChar;
            SceneManager.LoadScene("arenaSelector");
        }
    }

    public void ChooseArena(string arena)
    {
        SceneManager.LoadScene(arena);
    }

}
