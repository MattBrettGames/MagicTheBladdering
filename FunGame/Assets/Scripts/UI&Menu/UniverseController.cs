using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;
using Rewired;

public class UniverseController : BlankMono
{
    [Header("Level Counts")]
    public int levelCount;
    public int firstArenaID;

    [Header("GameObjects")]
    public CharacterSelector charSelector1;
    public CharacterSelector charSelector2;
    public CharacterSelector charSelector3;
    public CharacterSelector charSelector4;
    public AnalyticsController analytics;
    public Player player;
    public AreaGen generator;
    public ScoreTracker tracker;
    DualObjectiveCamera camCode;

    [Header("Character Info")]
    public GameObject[] selectedChars = new GameObject[4];
    public int numOfPlayers;
    private int lockedInPlayers;
    public int numOfRespawns;
    public int respawnTimer;
    private List<GameObject> playersAlive = new List<GameObject>();
    private int[] finalScore = new int[2];

    [Header("Instantiation Info")]
    public List<spawnPositions> allSpawnPositions = new List<spawnPositions>();
    private int currentLevel;
    private string gameMode;

    [Header("Analytics")]
    private string[] characters = new string[2] { "", "" };
    private string[] skins = new string[2] { "", "" };

    [Header("Determining Victory")]
    private string winner;
    private Text victoryText;

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
        if (SceneManager.GetActiveScene().name == "Bios")
        {
            if (Input.GetButtonDown("AllBButton")) { SceneManager.LoadScene("MainMenu"); }
        }
        else if (SceneManager.GetActiveScene().name.Contains("GameOver"))
        {
            if (Input.GetButtonDown("AllBButton"))
            {
                SceneManager.LoadScene("MainMenu");
                selectedChars[0] = null;
                selectedChars[1] = null;
            }
        }
        else if (SceneManager.GetActiveScene().name.Contains("ArenaSel"))
        {
            if (Input.GetButtonDown("AllBButton"))
            {
                Unlock(0);
                Unlock(1);
                ReturnToMenu();
            }
        }
    }

    public void SelectedPlay() { SceneManager.LoadScene("2CharacterSelectorPVP"); numOfPlayers = 2; }
    public void SelectedBios() { SceneManager.LoadScene("Bios"); }
    public void SelectedAdventure() { SceneManager.LoadScene("2CharacterSelectorPvE"); numOfPlayers = 2; playersAlive.Add(GameObject.FindGameObjectWithTag("Player1")); playersAlive.Add(GameObject.FindGameObjectWithTag("Player2")); }
    public void Restart() { SceneManager.LoadScene("MainMenu"); }

    private void OnLevelWasLoaded(int level)
    {

        print("The level is " + level);
        currentLevel = level;
        if (level == 0)
        {
            print(lockedInPlayers);
            //lockedInPlayers = 0;
        }
        if (level == 2)
        {
            gameMode = "PvP";
            charSelector1 = GameObject.FindGameObjectWithTag("P1Selector").GetComponent<CharacterSelector>();
            charSelector1.SetUniverse(this);
            charSelector2 = GameObject.FindGameObjectWithTag("P2Selector").GetComponent<CharacterSelector>();
            charSelector2.SetUniverse(this);
        }
        else if (level == 3)
        {
            victoryText = GameObject.Find("VictoryText").GetComponent<Text>();
            victoryText.text = winner + " Won!";
            print(winner + "|");// + GameObject.Find(winner));

            GameObject[] gams = GameObject.FindGameObjectsWithTag("Boss");

            for (int i = 0; i < gams.Length; i++)
            {
                if (gams[i].name != winner) { gams[i].SetActive(false); }
            }
        }
        else if (level == 3)
        {
            Text p1Text = GameObject.Find("ScoreInt1").GetComponent<Text>();
            p1Text.text = finalScore[0].ToString();
            Text p2Text = GameObject.Find("ScoreInt2").GetComponent<Text>();
            p2Text.text = finalScore[1].ToString();
        }
        else if (level >= firstArenaID)
        {
            Vector3 targetScale = new Vector3(1, 1, 1);
            Quaternion targetLook = new Quaternion(0, 0, 0, 0);
            Vector3 targetPos = new Vector3(0, 5, 0);
            int[] charInts = new int[4];

            #region Player 1
            GameObject p1 = selectedChars[0];
            p1.SetActive(true);
            PlayerBase playerCode = p1.GetComponent<PlayerBase>();
            playerCode.enabled = true;
            playerCode.thisPlayer = "P1";
            p1.tag = "Player1";
            p1.transform.SetParent(GameObject.Find("CentreBase").transform);
            p1.GetComponent<CapsuleCollider>().isTrigger = false;

            GameObject parent1 = GameObject.Find("Player1Base");
            parent1.transform.SetParent(p1.transform);
            parent1.transform.localPosition = targetPos;
            p1.transform.position = new Vector3(-15, 0.4f, 0);
            p1.transform.localScale = Vector3.one;
            p1.transform.rotation = targetLook;
            if (p1.name.Contains("Valderheim")) { charInts[0] = 0; }
            else if (p1.name.Contains("Songbird")) { charInts[0] = 1; }
            playerCode.SetInfo();

            #endregion

            #region Player 2
            GameObject p2 = selectedChars[1];
            p2.SetActive(true);
            playerCode = p2.GetComponent<PlayerBase>();
            playerCode.enabled = true;
            playerCode.thisPlayer = "P2";
            p2.tag = "Player2";
            p2.transform.parent = GameObject.Find("CentreBase").transform;
            p2.GetComponent<CapsuleCollider>().isTrigger = false;

            GameObject parent2 = GameObject.Find("Player2Base");
            parent2.transform.SetParent(p2.transform);
            parent2.transform.localPosition = targetPos;
            p2.transform.position = new Vector3(15, 0.4f, 0);
            p2.transform.rotation = targetLook;
            if (p1.name.Contains("Valderheim")) { charInts[1] = 0; }
            else if (p1.name.Contains("Songbird")) { charInts[1] = 1; }
            p2.transform.localScale = targetScale;
            playerCode.SetInfo();
            #endregion

            for (int i = 0; i < 2; i++)
            {
                GameObject.Find("HUDController").GetComponents<HUDController>()[i].SetStats(charInts[i]);
            }
        }
    }

    public void CheckReady(int arrayIndex, GameObject gobject, GameObject character, string skin)
    {
        selectedChars[arrayIndex] = gobject;
        characters[arrayIndex] = character.name;
        skins[arrayIndex] = skin;

        lockedInPlayers++;
        gobject.transform.parent = gameObject.transform;

        if (lockedInPlayers == numOfPlayers)
        {
            selectedChars[0].SetActive(false);
            selectedChars[1].SetActive(false);
            SceneManager.LoadScene("ArenaSelectorPvP");
            //lockedInPlayers = 0;
        }
    }

    public void Unlock(int player)
    {
        lockedInPlayers--;
        if (SceneManager.GetActiveScene().name == "ArenaSelector")
        {
            selectedChars[0].transform.SetParent(Camera.main.transform);
            selectedChars[0] = null;
            selectedChars[1].transform.SetParent(Camera.main.transform);
            selectedChars[1] = null;
        }
        else
        {
            selectedChars[player].transform.SetParent(GameObject.Find("Player" + (player + 1) + "GameObjectStore").transform);
        }
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
        camCode.Death();

        if (gameMode == "PvP")
        {
            PlayerBase playerCode = player.GetComponent<PlayerBase>();

            playerCode.numOfDeaths++;

            if (playerCode.numOfDeaths != numOfRespawns)
            {
                StartCoroutine(StartSpawn(playerCode, playerCode.playerID));
                playerCode.enabled = true;
            }
            else
            {
                if (playerCode.playerID == 1)
                {
                    player = GameObject.FindGameObjectWithTag("Player1");
                }
                else
                {
                    player = GameObject.FindGameObjectWithTag("Player2");

                }

                winner = player.name;
                SceneManager.LoadScene("GameOver");
            }
        }
        else
        {
            playersAlive.Remove(player);
            if (playersAlive.Count >= 0)
            {
                finalScore[0] = tracker.ReturnScores()[0];
                finalScore[1] = tracker.ReturnScores()[1];

                SceneManager.LoadScene("PvEGameOver");
            }
        }
    }

    private IEnumerator StartSpawn(PlayerBase player, int playerInt)
    {
        yield return new WaitForSeconds(respawnTimer);
        camCode.RespawnedAPlayer();
        player.Respawn();
        player.gameObject.transform.position = new Vector3(15, 0, 0);
    }
    public void ReturnToMenu()
    {
        charSelector1.locked = false;
        charSelector2.locked = false;
        selectedChars[0] = null;
        selectedChars[1] = null;
        if (transform.childCount > 3) transform.GetChild(3).SetParent(Camera.main.transform);
        SceneManager.LoadScene("MainMenu");
    }

    public void BossDeath()
    {
        generator.rowsToSpawn += 1;
        generator.columnsToSpawn += 1;
        generator.DestroyZones();
        generator.CreateZone(currentLevel);
        tracker.EnemyDeath("both", 0);
        playersAlive[0].transform.position = allSpawnPositions[currentLevel - levelCount - 1].spawnPos[0];
        playersAlive[1].transform.position = allSpawnPositions[currentLevel - levelCount - 1].spawnPos[1];
    }

    public void GetCam(DualObjectiveCamera newCam) { camCode = newCam; }

}