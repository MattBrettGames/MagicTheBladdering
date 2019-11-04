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
    public AnalyticsController analytics;
    public Player player;
    public WaveController waves;

    [Header("Character Info")]
    public GameObject[] selectedChars = new GameObject[4];
    public int numOfPlayers;
    private int lockedInPlayers;
    public int numOfRespawns;
    public int respawnTimer;

    [Header("Instantiation Info")]
    public List<spawnPositions> allSpawnPositions = new List<spawnPositions>();
    private int currentLevel;
    private string gameMode;

    [Header("Analytics")]
    private string[] characters = new string[2] { "", "" };
    private string[] skins = new string[2] { "", "" };

    void Start()
    {
        player = ReInput.players.GetPlayer("System");

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
        if (SceneManager.GetActiveScene().name == "Bio")
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

    public void SelectedPlay() { SceneManager.LoadScene("2CharacterSelectorPVP"); numOfPlayers = 2; }
    public void SelectedBios() { SceneManager.LoadScene("Bios"); }
    public void SelectedAdventure() { SceneManager.LoadScene("2CharacterSelectorPvE"); numOfPlayers = 2; }

    private void OnLevelWasLoaded(int level)
    {
        currentLevel = level;
        if (level == 2)
        {
            gameMode = "PvE";
            charSelector1 = GameObject.FindGameObjectWithTag("P1Selector").GetComponent<CharacterSelector>();
            charSelector1.SetUniverse(this);
            charSelector2 = GameObject.FindGameObjectWithTag("P2Selector").GetComponent<CharacterSelector>();
            charSelector2.SetUniverse(this);
        }
        else if (level == 3)
        {
            gameMode = "PvP";
            charSelector1 = GameObject.FindGameObjectWithTag("P1Selector").GetComponent<CharacterSelector>();
            charSelector1.SetUniverse(this);
            charSelector2 = GameObject.FindGameObjectWithTag("P2Selector").GetComponent<CharacterSelector>();
            charSelector2.SetUniverse(this);
        }
        else if (level >= 6)
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
            
            waves.BeginWaves(level);

            for (int i = 0; i < 2; i++)
            {
                GameObject.Find("HUDController").GetComponents<HUDController>()[i].SetStats(charInts[i]);
                print(string.Format("Set {0}, HUD parsed {1}.", i, charInts[i]));
            }
        }
    }

    public void CheckReady(int arrayIndex, GameObject gobject, string character, string skin)
    {
        selectedChars[arrayIndex] = gobject;
        characters[arrayIndex] = character;
        skins[arrayIndex] = skin;

        lockedInPlayers++;
        gobject.transform.parent = gameObject.transform;

        if (lockedInPlayers == numOfPlayers)
        {
            SceneManager.LoadScene("ArenaSelector"+gameMode);
        }
    }

    public void Unlock()
    {
        lockedInPlayers--;
    }

    public void ChooseArena(string arena)
    {
        analytics.map = arena;
        analytics.character1 = characters[0];
        analytics.character2 = characters[1];
        analytics.skin1 = skins[0];
        analytics.skin2 = skins[1];
        analytics.CreateCSV();
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
    public void ReturnToMenu()
    {
        charSelector1.locked = false;
        charSelector2.locked = false;
        SceneManager.LoadScene("MainMenu");

    }

}