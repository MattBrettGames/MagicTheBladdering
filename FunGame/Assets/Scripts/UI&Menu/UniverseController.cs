using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using Rewired;

public class UniverseController : BlankMono
{
    [Header("GameObjects")]
    public CharacterSelector charSelector1;
    public CharacterSelector charSelector2;
    public CharacterSelector charSelector3;
    public CharacterSelector charSelector4;

    [Header("Character Info")]
    public GameObject[] selectedChars = new GameObject[4];
    public int numOfPlayers;
    private int lockedInPlayers;
    public int numOfRespawns;
    public int respawnTimer;

    [Header("Instantiation Info")]
    public List<spawnPositions> allSpawnPositions = new List<spawnPositions>();
    private int currentLevel;

    void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

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
            //if (Input.GetButtonDown("AllAButton")) { SceneManager.LoadScene("3CharacterSelector"); numOfPlayers = 3; }
            //if (Input.GetButtonDown("AllBButton")) { SceneManager.LoadScene("4CharacterSelector"); numOfPlayers = 4; }
            if (Input.GetButtonDown("AllXButton")) { SceneManager.LoadScene("2CharacterSelector"); numOfPlayers = 2; }
            if (Input.GetButtonDown("AllYButton")) { SceneManager.LoadScene("Bio"); }
        }
        else if (SceneManager.GetActiveScene().name == "Bio")
        {
            if (Input.GetButtonDown("AllBButton")) { SceneManager.LoadScene("MainMenu"); }
        }
        else if (SceneManager.GetActiveScene().name == "GameOver")
        {
            if (Input.GetButtonDown("AllBButton"))
            {
                SceneManager.LoadScene("MainMenu");
            }
        }
    }
    
    private void OnLevelWasLoaded(int level)
    {
        currentLevel = level;
        if (level == 2)
        {
            print("Loaded 2CharacterSelector");
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
        else if (level >= 7)
        {
            Vector3 targetScale = new Vector3(1, 1, 1);
            Quaternion targetLook = new Quaternion(0, 0, 0, 0);
            Vector3 targetPos = new Vector3(0, 5, 0);
            int[] charInts = new int[4];

            #region Player 1
            GameObject p1 = selectedChars[0];
            p1.GetComponent<PlayerBase>().enabled = true;
            p1.GetComponent<PlayerBase>().thisPlayer = "P1";
            p1.tag = "Player1";
            p1.transform.parent = GameObject.Find("CentreBase").transform;

            GameObject parent1 = GameObject.Find("Player1Base");
            parent1.transform.SetParent(p1.transform);
            parent1.transform.localPosition = targetPos;
            p1.transform.position = allSpawnPositions[level - 7].spawnPos[0];
            p1.transform.localScale = targetScale;
            p1.transform.rotation = targetLook;
            if (p1.name.Contains("Valderheim")) { charInts[0] = 0; }
            else if (p1.name.Contains("Songbird")) { charInts[0] = 1; }

            #endregion

            #region Player 2
            GameObject p2 = selectedChars[1];
            p2.GetComponent<PlayerBase>().enabled = true;
            p2.GetComponent<PlayerBase>().thisPlayer = "P2";
            p2.tag = "Player2";
            p2.transform.parent = GameObject.Find("CentreBase").transform;

            GameObject parent2 = GameObject.Find("Player2Base");
            parent2.transform.SetParent(p2.transform);
            parent2.transform.localPosition = targetPos;
            p2.transform.position = allSpawnPositions[level - 7].spawnPos[1];
            p2.transform.localScale = targetScale;
            p2.transform.rotation = targetLook;
            if (p1.name.Contains("Valderheim")) { charInts[1] = 0; }
            else if (p1.name.Contains("Songbird")) { charInts[1] = 1; }
            #endregion

            #region Player 3
            if (selectedChars[2] != null)
            {
                GameObject p3 = selectedChars[2];
                p3.GetComponent<PlayerBase>().enabled = true;
                p3.GetComponent<PlayerBase>().thisPlayer = "P3";
                p3.tag = "Player3";
                //p3.GetComponent<Rigidbody>().isKinematic = false;
                p3.transform.parent = null;
                p3.GetComponent<PlayerController>().playerId = 2;

                GameObject parent3 = GameObject.Find("Player3Base");
                parent3.transform.SetParent(p3.transform);
                parent3.transform.position = allSpawnPositions[level - 7].spawnPos[2];
                p3.transform.localPosition = Vector3.zero;
                p3.transform.localScale = targetScale;
                p3.transform.rotation = targetLook;
            }
            #endregion

            #region Player 4
            if (selectedChars[3] != null)
            {
                GameObject p4 = selectedChars[3];
                p4.GetComponent<PlayerBase>().enabled = true;
                p4.GetComponent<PlayerBase>().thisPlayer = "P4";
                p4.tag = "Player4";
                //p4.GetComponent<Rigidbody>().isKinematic = false;
                p4.transform.parent = null;
                p4.GetComponent<PlayerController>().playerId = 3;

                GameObject parent4 = GameObject.Find("Player4Base");
                parent4.transform.SetParent(p4.transform);
                parent4.transform.position = allSpawnPositions[level - 7].spawnPos[3];
                p4.transform.localPosition = Vector3.zero;
                p4.transform.localScale = targetScale;
                p4.transform.rotation = targetLook;

            }
            #endregion

            for (int i = 0; i < 2; i++)
            {
                GameObject.Find("HUDController").GetComponents<HUDController>()[i].SetStats(charInts[i]);
            }
        }
    }

    public void CheckReady(int arrayIndex, GameObject gobject)
    {
        selectedChars[arrayIndex] = gobject;
        gobject.transform.parent = gameObject.transform;
        lockedInPlayers++;

        if (lockedInPlayers == numOfPlayers)
        {
            SceneManager.LoadScene("ArenaSelector");
        }
    }

    public void ChooseArena(string arena)
    {
        SceneManager.LoadScene(arena);
    }

    [Serializable] public struct spawnPositions { public List<Vector3> spawnPos; }

    public void PlayerDeath(GameObject player)
    {
        player.SetActive(false);
        PlayerBase playerCode = player.GetComponent<PlayerBase>();

        playerCode.numOfDeaths++;

        if (playerCode.numOfDeaths != numOfRespawns)
        {
            StartCoroutine(StartSpawn(playerCode, playerCode.playerID));
        }
        else
        {
            SceneManager.LoadScene("GameOver");
        }
    }

    private IEnumerator StartSpawn(PlayerBase player, int playerInt)
    {
        yield return new WaitForSeconds(respawnTimer);
        player.Respawn();
        player.gameObject.transform.position = allSpawnPositions[currentLevel - 7].spawnPos[playerInt];
    }
}