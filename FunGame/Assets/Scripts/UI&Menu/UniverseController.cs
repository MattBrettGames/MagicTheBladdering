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
        currentLevel = level;
        if (level == 0)
        {
            lockedInPlayers = 0;
            Time.timeScale = 1;
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
            Time.timeScale = 1;
            print("Loaded the end screen");
            victoryText = GameObject.Find("VictoryText").GetComponent<Text>();
            victoryText.text = winner + " Won!";
            print("1 Yup " + winner);

            GameObject gam = GameObject.Find(winner);
            gam.transform.SetParent(Camera.main.transform);
            GameObject.Find("CharacterStore").SetActive(false);
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
            else if (p1.name.Contains("Carmen")) { charInts[0] = 2; }
            else if (p1.name.Contains("Wiosna")) { charInts[0] = 3; }
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
            if (p2.name.Contains("Valderheim")) { charInts[1] = 0; }
            else if (p2.name.Contains("Songbird")) { charInts[1] = 1; }
            else if (p2.name.Contains("Camren")) { charInts[1] = 2; }
            else if (p2.name.Contains("Wiosna")) { charInts[1] = 3; }
            p2.transform.localScale = targetScale;
            playerCode.SetInfo();
            #endregion

            for (int i = 0; i < 2; i++)
            {
                GameObject.Find("HUDController").GetComponents<HUDController>()[i].SetStats(charInts[i], selectedChars[i].name);
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

    public void PlayerDeath(GameObject player, GameObject otherPlayer)
    {

        if (gameMode == "PvP")
        {
            PlayerBase otherCode = otherPlayer.GetComponentInParent<PlayerBase>();
            otherCode.dir = Vector3.zero;
            otherCode.GetComponent<Rigidbody>().velocity = Vector3.zero;
            otherCode.GetComponent<Animator>().SetFloat("Movement", 0);
            otherCode.enabled = false;

            PlayerBase playerCode = player.GetComponent<PlayerBase>();

            camCode.Death(playerCode.playerID);

            playerCode.numOfDeaths++;

            if (playerCode.numOfDeaths != numOfRespawns)
            {
                StartCoroutine(StartSpawn(playerCode, playerCode.playerID, otherCode));
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
    }

    private IEnumerator StartSpawn(PlayerBase player, int playerInt, PlayerBase otherPlayer)
    {
        yield return new WaitForSeconds(respawnTimer);
        player.enabled = true;
        camCode.RespawnedAPlayer();
        player.Respawn();
        otherPlayer.enabled = true;
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

    public void GetCam(DualObjectiveCamera newCam) { camCode = newCam; }

    IEnumerator DelayedVictory()
    {
        yield return new WaitForSeconds(0.1f);

        GameObject[] gams = GameObject.FindGameObjectsWithTag("Boss");
        for (int i = 0; i < gams.Length; i++)
        {
            print(winner + "|" + gams[i].name + "|" + gams[i].activeInHierarchy);
            if (gams[i].name == winner) { gams[i].SetActive(true); }
        }
    }

    public void CameraRumbleCall()
    {
        camCode.CamShake(0.1f);
    }



}