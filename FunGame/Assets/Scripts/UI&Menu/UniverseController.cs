using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;

public class UniverseController : BlankMono
{

    [Header("GameObjects")]
    public CharacterSelector charSelector1;
    public CharacterSelector charSelector2;
    public CharacterSelector charSelector3;
    public CharacterSelector charSelector4;

    [Header("Character Info")]
    public GameObject lockedCharacter1;
    public GameObject lockedCharacter2;
    public GameObject lockedCharacter3;
    public GameObject lockedCharacter4;

    [Header("Instantiation Info")]
    public List<spawnPositions> allSpawnPositions = new List<spawnPositions>();

    void Start()
    {
        if (GameObject.FindGameObjectsWithTag("UniverseController").Length == 1)
        {
            DontDestroyOnLoad(gameObject);
        }
        else { Destroy(gameObject); }
    }

    private void Update()
    {
        if (SceneManager.GetActiveScene().name == "MainMenu")
        {
            if (Input.GetButtonDown("AllAButton")) { SceneManager.LoadScene("3CharacterSelector"); }
            if (Input.GetButtonDown("AllBButton")) { SceneManager.LoadScene("4CharacterSelector"); }
            if (Input.GetButtonDown("AllXButton")) { SceneManager.LoadScene("2CharacterSelector"); }
            if (Input.GetButtonDown("AllYButton")) { SceneManager.LoadScene("Bio"); }
        }
        else if (SceneManager.GetActiveScene().name == "Bio")
        {
            if (Input.GetButtonDown("AllBButton")) { SceneManager.LoadScene("MainMenu"); }
        }
    }

    private void OnLevelWasLoaded(int level)
    {
        if (level == 2)
        {
            charSelector1 = GameObject.FindGameObjectWithTag("P1Selector").GetComponent<CharacterSelector>();
            charSelector1.SetUniverse(this);
            charSelector2 = GameObject.FindGameObjectWithTag("P2Selector").GetComponent<CharacterSelector>();
            charSelector2.SetUniverse(this);
        }
        else if (level == 3)
        {
            charSelector1 = GameObject.FindGameObjectWithTag("P1Selector").GetComponent<CharacterSelector>();
            charSelector1.SetUniverse(this);
            charSelector2 = GameObject.FindGameObjectWithTag("P2Selector").GetComponent<CharacterSelector>();
            charSelector2.SetUniverse(this);
            charSelector3 = GameObject.FindGameObjectWithTag("P3Selector").GetComponent<CharacterSelector>();
            charSelector3.SetUniverse(this);
        }
        else if (level == 4)
        {
            charSelector1 = GameObject.FindGameObjectWithTag("P1Selector").GetComponent<CharacterSelector>();
            charSelector1.SetUniverse(this);
            charSelector2 = GameObject.FindGameObjectWithTag("P2Selector").GetComponent<CharacterSelector>();
            charSelector2.SetUniverse(this);
            charSelector3 = GameObject.FindGameObjectWithTag("P3Selector").GetComponent<CharacterSelector>();
            charSelector3.SetUniverse(this);
            charSelector4 = GameObject.FindGameObjectWithTag("P4Selector").GetComponent<CharacterSelector>();
            charSelector4.SetUniverse(this);
        }
        else if (level >= 6)
        {
            PlayerBase p1 = Instantiate<GameObject>(lockedCharacter1, allSpawnPositions[level - 6].spawnPos[0], Quaternion.identity).GetComponent<PlayerBase>();
            p1.enabled = true;
            PlayerBase p2 = Instantiate<GameObject>(lockedCharacter2, allSpawnPositions[level - 6].spawnPos[1], Quaternion.identity).GetComponent<PlayerBase>();
            p2.enabled = true;
            if (lockedCharacter3 != null)
            {
                PlayerBase p3 = Instantiate<GameObject>(lockedCharacter3, allSpawnPositions[level - 6].spawnPos[2], Quaternion.identity).GetComponent<PlayerBase>();
                p3.enabled = true;
            }
            if (lockedCharacter4 != null)
            {
                PlayerBase p4 = Instantiate<GameObject>(lockedCharacter4, allSpawnPositions[level - 6].spawnPos[3], Quaternion.identity).GetComponent<PlayerBase>();
                p4.enabled = true;
            }
        }
    }

    public void CheckReady()
    {
        if (SceneManager.GetActiveScene().name == "2CharacterSelector")
        {
            if (charSelector1.locked && charSelector2.locked)
            {
                lockedCharacter1 = charSelector1.displayChar;
                lockedCharacter2 = charSelector2.displayChar;
                SceneManager.LoadScene("arenaSelector");
            }
        }
        if (SceneManager.GetActiveScene().name == "3CharacterSelector")
        {
            if (charSelector1.locked && charSelector2.locked && charSelector3.locked)
            {
                lockedCharacter1 = charSelector1.displayChar;
                lockedCharacter2 = charSelector2.displayChar;
                lockedCharacter3 = charSelector3.displayChar;
                SceneManager.LoadScene("arenaSelector");
            }
        }
        if (SceneManager.GetActiveScene().name == "4CharacterSelector")
        {
            if (charSelector1.locked && charSelector2.locked && charSelector3.locked && charSelector4.locked)
            {
                lockedCharacter1 = charSelector1.displayChar;
                lockedCharacter2 = charSelector2.displayChar;
                lockedCharacter3 = charSelector3.displayChar;
                lockedCharacter4 = charSelector4.displayChar;
                SceneManager.LoadScene("arenaSelector");
            }
        }
    }

    public void ChooseArena(string arena)
    {
        SceneManager.LoadScene(arena);
    }

    [Serializable] public struct spawnPositions { public List<Vector3> spawnPos; }

}
