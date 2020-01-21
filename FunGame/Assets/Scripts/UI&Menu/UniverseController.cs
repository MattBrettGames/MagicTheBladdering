using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PostProcessing;
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
    //DualObjectiveCamera dualCamCode;
    TriObjectiveCamera triCamCode;
    AudioManager audioManager;

    [Header("Character Info")]
    public GameObject[] selectedChars = new GameObject[4];
    [HideInInspector] public int numOfPlayers;
    private int lockedInPlayers;
    public int numOfRespawns;
    public int respawnTimer;

    private int livingPlayers;
    private int currentLevel;
    private string gameMode;

    [Header("Analytics")]
    private string[] characters = new string[2] { "", "" };
    private string[] skins = new string[2] { "", "" };

    [Header("Determining Victory")]
    [SerializeField] string gameOverText;
    private string winner;
    private Text victoryText;

    [Header("Settings")]

    [HideInInspector] public bool isPostProcessing = true;



    void Start()
    {
        player = ReInput.players.GetPlayer("System");

        audioManager = GetComponentInChildren<AudioManager>();
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
        if (SceneManager.GetActiveScene().name == "OptionsMenu")
        {
            if (Input.GetButtonDown("AllBButton")) { GameObject.Find("Cover").GetComponent<FadeController>().FadeToBlack("MainMenu"); }
        }
        if (SceneManager.GetActiveScene().name == "Bios")
        {
            if (Input.GetButtonDown("AllBButton")) { GameObject.Find("Cover").GetComponent<FadeController>().FadeToBlack("MainMenu"); ; }
        }
        else if (SceneManager.GetActiveScene().name.Contains("GameOver"))
        {
            if (Input.GetButtonDown("AllBButton"))
            {
                GameObject.Find("Cover").GetComponent<FadeController>().FadeToBlack("MainMenu");
                selectedChars[0] = null;
                selectedChars[1] = null;
                selectedChars[2] = null;
                selectedChars[3] = null;
            }
        }
        else if (SceneManager.GetActiveScene().name.Contains("ArenaSel"))
        {
            if (Input.GetButtonDown("AllBButton"))
            {
                Unlock(0);
                Unlock(1);
                Unlock(2);
                Unlock(3);
                ReturnToMenu();
            }
        }
    }

    public void SelectedPlay()
    {
        numOfPlayers = ReInput.controllers.controllerCount - 2;
        if (numOfPlayers < 2) { numOfPlayers = 2; }
        if (numOfPlayers > 4) { numOfPlayers = 4; }

        GameObject.Find("Cover").GetComponent<FadeController>().FadeToBlack(numOfPlayers + "CharacterSelectorPVP");
    }

    public void SelectedBios() { GameObject.Find("Cover").GetComponent<FadeController>().FadeToBlack("Bios"); }
    public void SelectedOptions() { GameObject.Find("Cover").GetComponent<FadeController>().FadeToBlack("OptionsMenu"); }
    // public void SelectedAdventure() { SceneManager.LoadScene("2CharacterSelectorPvE"); numOfPlayers = 2; playersAlive.Add(GameObject.FindGameObjectWithTag("Player1")); playersAlive.Add(GameObject.FindGameObjectWithTag("Player2")); }
    public void Restart() { GameObject.Find("Cover").GetComponent<FadeController>().FadeToBlack("MainMenu"); }

    private void OnLevelWasLoaded(int level)
    {

        StartCoroutine(NewLevelLoad(level));
    }
    IEnumerator NewLevelLoad(int level)
    {

        yield return new WaitForEndOfFrame();

        livingPlayers = numOfPlayers;
        GameObject.Find("Cover").GetComponent<FadeController>().FadeFromBlack();
        currentLevel = level;

        if (level == 0)
        {
            lockedInPlayers = 0;
            Time.timeScale = 1;
        }
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
        else if (level == 5)
        {
            Time.timeScale = 1;
            victoryText = GameObject.Find("VictoryText").GetComponent<Text>();
            victoryText.text = gameOverText.Replace("<winner>", winner);
            GameObject gam = GameObject.Find(winner);
            gam.transform.SetParent(Camera.main.transform);
            GameObject.Find("CharacterStore").SetActive(false);
        }
        //Game-Level Setup
        else if (level >= firstArenaID)
        {
            Vector3 targetScale = new Vector3(1, 1, 1);
            Vector3 targetLook = new Vector3(0, 90, 0);
            Vector3 targetPos = new Vector3(0, 5, 0);
            int[] charInts = new int[4];
            livingPlayers = numOfPlayers;

            /*if (numOfPlayers == 2)
            {
                dualCamCode.enabled = true;
                triCamCode.enabled = false;
            }
            else
            {*/
            // dualCamCode.enabled = false;
            // }

            triCamCode.enabled = true;

            if (numOfPlayers == 2)
            {
                triCamCode.RemoveTarget(GameObject.Find("Player3Base").transform);
                triCamCode.RemoveTarget(GameObject.Find("Player4Base").transform);
            }

            if (numOfPlayers == 3)
            {
                triCamCode.RemoveTarget(GameObject.Find("Player4Base").transform);
            }

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
            p1.transform.position = GameObject.Find("Player1Spawn").transform.position;
            p1.transform.localScale = Vector3.one;
            p1.transform.eulerAngles = targetLook;
            if (p1.name.Contains("Valderheim")) { charInts[0] = 0; }
            else if (p1.name.Contains("Songbird")) { charInts[0] = 1; }
            else if (p1.name.Contains("Carmen")) { charInts[0] = 2; }
            else if (p1.name.Contains("Wiosna")) { charInts[0] = 3; }
            playerCode.SetInfo(this, 13);
            #endregion

            targetLook = new Vector3(0, -90, 0);

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
            p2.transform.position = GameObject.Find("Player2Spawn").transform.position;
            p2.transform.eulerAngles = targetLook;
            if (p2.name.Contains("Valderheim")) { charInts[1] = 0; }
            else if (p2.name.Contains("Songbird")) { charInts[1] = 1; }
            else if (p2.name.Contains("Carmen")) { charInts[1] = 2; }
            else if (p2.name.Contains("Wiosna")) { charInts[1] = 3; }
            p2.transform.localScale = targetScale;
            playerCode.SetInfo(this, 14);
            #endregion

            targetLook = new Vector3(0, 0, 0);

            #region Player 3
            if (selectedChars[2] != null)
            {
                GameObject p3 = selectedChars[2];
                p3.SetActive(true);
                playerCode = p3.GetComponent<PlayerBase>();
                playerCode.enabled = true;
                playerCode.thisPlayer = "P3";
                p3.tag = "Player3";
                p3.transform.parent = GameObject.Find("CentreBase").transform;
                p3.GetComponent<CapsuleCollider>().isTrigger = false;

                GameObject parent3 = GameObject.Find("Player3Base");
                parent3.transform.SetParent(p3.transform);
                parent3.transform.localPosition = targetPos;
                p3.transform.position = GameObject.Find("Player3Spawn").transform.position;
                p3.transform.eulerAngles = targetLook;
                if (p3.name.Contains("Valderheim")) { charInts[2] = 0; }
                else if (p3.name.Contains("Songbird")) { charInts[2] = 1; }
                else if (p3.name.Contains("Carmen")) { charInts[2] = 2; }
                else if (p3.name.Contains("Wiosna")) { charInts[2] = 3; }
                p3.transform.localScale = targetScale;
                playerCode.SetInfo(this, 15);
            }
            else { selectedChars[2] = new GameObject("NullObjectException"); }
            #endregion

            targetLook = new Vector3(0, 180, 0);

            #region Player 4
            if (selectedChars[3] != null)
            {
                GameObject p4 = selectedChars[3];
                p4.SetActive(true);
                playerCode = p4.GetComponent<PlayerBase>();
                playerCode.enabled = true;
                playerCode.thisPlayer = "P4";
                p4.tag = "Player4";
                p4.transform.parent = GameObject.Find("CentreBase").transform;
                p4.GetComponent<CapsuleCollider>().isTrigger = false;

                GameObject parent4 = GameObject.Find("Player4Base");
                parent4.transform.SetParent(p4.transform);
                parent4.transform.localPosition = targetPos;
                p4.transform.position = GameObject.Find("Player4Spawn").transform.position;
                p4.transform.eulerAngles = targetLook;
                if (p4.name.Contains("Valderheim")) { charInts[3] = 0; }
                else if (p4.name.Contains("Songbird")) { charInts[3] = 1; }
                else if (p4.name.Contains("Carmen")) { charInts[3] = 2; }
                else if (p4.name.Contains("Wiosna")) { charInts[3] = 3; }
                p4.transform.localScale = targetScale;
                playerCode.SetInfo(this, 16);
            }
            else { selectedChars[3] = new GameObject("NullObjectException"); }
            #endregion

            for (int i = 0; i < 4; i++)
            {
                bool temp;
                if (!selectedChars[i].name.Contains("Null"))
                {
                    temp = true;
                }
                else
                {
                    temp = false;
                    RemovePlayerFromTargets(i + 1);
                }
                GameObject.Find("P" + (i + 1) + "HUDController").GetComponent<HUDController>().SetStats(charInts[i], selectedChars[i].name, temp);

            }
        }
    }

    public void CheckReady(int arrayIndex, GameObject gobject, GameObject character, string skin)
    {
        StartCoroutine(DelayedCheckReady(arrayIndex, gobject, character, skin));
    }
    IEnumerator DelayedCheckReady(int arrayIndex, GameObject gobject, GameObject character, string skin)
    {
        yield return new WaitForSecondsRealtime(1);

        selectedChars[arrayIndex] = gobject;

        lockedInPlayers++;
        //gobject.transform.parent = gameObject.transform;

        if (lockedInPlayers == numOfPlayers)
        {
            GameObject.Find("Cover").GetComponent<FadeController>().FadeToBlack("ArenaSelectorPVP");
            Invoke("DisableChars", 0.5f);
            charSelector1.enabled = false;
            charSelector2.enabled = false;
            charSelector3.enabled = false;
            charSelector4.enabled = false;
        }
    }

    void DisableChars()
    {
        selectedChars[0].SetActive(false);
        selectedChars[1].SetActive(false);
        selectedChars[2].SetActive(false);
        selectedChars[3].SetActive(false);
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

            selectedChars[2].transform.SetParent(Camera.main.transform);
            selectedChars[2] = null;
            selectedChars[3].transform.SetParent(Camera.main.transform);
            selectedChars[3] = null;
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
        //analytics.CreateCSV();
        GameObject.Find("Cover").GetComponent<FadeController>().FadeToBlack(arena);
    }

    void RemovePlayerFromTargets(int deadPlayer)
    {
        for (int i = 0; i < livingPlayers; i++)
        {
            //        print("Removing " + "Player" + deadPlayer + "Base" + " from " + "Player" + (i + 1) + "Base");
            PlayerBase playerI = GameObject.Find("Player" + (i + 1) + "Base").GetComponentInParent<PlayerBase>();
            playerI.RemoveTarget(GameObject.Find("Player" + deadPlayer + "Base").transform);
        }
    }

    public void PlayerDeath(GameObject player, GameObject otherPlayer)
    {
        if (numOfPlayers == 2)
        {

            PlayerBase playerCode = player.GetComponent<PlayerBase>();
            player.GetComponent<Rigidbody>().isKinematic = true;

            // dualCamCode.Death(playerCode.playerID);

            playerCode.numOfDeaths++;
            //   print(player.name + " has died " + playerCode.numOfDeaths + " times");

            if (playerCode.numOfDeaths != numOfRespawns)
            {
                StartCoroutine(StartSpawn(playerCode, playerCode.playerID));
            }
            else
            {
                livingPlayers--;
                if (livingPlayers <= 1)
                {
                    if (playerCode.playerID == 1)
                    {
                        player = GameObject.FindGameObjectWithTag("Player1");
                    }
                    else if (playerCode.playerID == 2)
                    {
                        player = GameObject.FindGameObjectWithTag("Player2");
                    }
                    else if (playerCode.playerID == 3)
                    {
                        player = GameObject.FindGameObjectWithTag("Player3");
                    }
                    else
                    {
                        player = GameObject.FindGameObjectWithTag("Player4");
                    }

                    winner = player.name;
                    Invoke("EndGame", 4);
                }

                triCamCode.enabled = true;
                triCamCode.RemoveTarget(GameObject.Find("Player" + (playerCode.playerID + 1) + "Base").transform);

                RemovePlayerFromTargets(playerCode.playerID - 1);

            }
        }
        else
        {

            PlayerBase playerCode = player.GetComponent<PlayerBase>();

            playerCode.numOfDeaths++;

            if (playerCode.numOfDeaths != numOfRespawns)
            {
                StartCoroutine(StartSpawn(playerCode, playerCode.playerID));
            }
            else
            {
                livingPlayers--;
                if (livingPlayers <= 1)
                {
                    if (playerCode.playerID == 1)
                    {
                        player = GameObject.FindGameObjectWithTag("Player1");
                    }
                    else if (playerCode.playerID == 2)
                    {
                        player = GameObject.FindGameObjectWithTag("Player2");
                    }
                    else if (playerCode.playerID == 3)
                    {
                        player = GameObject.FindGameObjectWithTag("Player3");
                    }
                    else
                    {
                        player = GameObject.FindGameObjectWithTag("Player4");
                    }

                    winner = player.name;
                    Invoke("EndGame", 4);
                }

                triCamCode.enabled = true;
                print("Player" + (playerCode.playerID + 1) + "Base");
                triCamCode.RemoveTarget(GameObject.Find("Player" + (playerCode.playerID + 1) + "Base").transform);

                RemovePlayerFromTargets(playerCode.playerID + 1);
            }
        }
    }
    void EndGame() { GameObject.Find("Cover").GetComponent<FadeController>().FadeToBlack("GameOver"); }

    private IEnumerator StartSpawn(PlayerBase player, int playerInt)
    {
        yield return new WaitForSeconds(respawnTimer);
        player.enabled = true;        
        triCamCode.AddTarget(playerInt + 1);
        player.Respawn();
        player.gameObject.transform.position = new Vector3(0, 0.4f, 0);
    }

    private IEnumerator StartSpawn(PlayerBase player, int playerInt, PlayerBase otherPlayer)
    {
        yield return new WaitForSeconds(respawnTimer);
        if (numOfPlayers == 2)
        {
            player.enabled = true;
            //dualCamCode.RespawnedAPlayer();
            player.Respawn();
            otherPlayer.enabled = true;
            player.gameObject.transform.position = new Vector3(0, 0.4f, 0);
        }
    }
    public void ReturnToMenu()
    {
        charSelector1.locked = false;
        charSelector2.locked = false;
        charSelector3.locked = false;
        charSelector4.locked = false;
        selectedChars[0] = null;
        selectedChars[1] = null;
        selectedChars[2] = null;
        selectedChars[3] = null;
        if (transform.childCount > 2) transform.GetChild(2).SetParent(Camera.main.transform);
        GameObject.Find("Cover").GetComponent<FadeController>().FadeToBlack("MainMenu");
    }

    public void GetCam(DualObjectiveCamera duoCam, TriObjectiveCamera triCam)
    {
        // dualCamCode = duoCam;
        triCamCode = triCam;
    }

    IEnumerator DelayedVictory()
    {
        yield return new WaitForSeconds(0.1f);

        GameObject[] gams = GameObject.FindGameObjectsWithTag("Boss");
        for (int i = 0; i < gams.Length; i++)
        {
            if (gams[i].name == winner) { gams[i].SetActive(true); }
        }
    }

    public void CameraRumbleCall(float degree) { triCamCode.CamShake(degree); }

    public void PlaySound(string clip) { audioManager.Play(clip); }

}
