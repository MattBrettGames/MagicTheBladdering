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
    DualObjectiveCamera dualCamCode;
    TriObjectiveCamera triCamCode;
    //    AudioManager audioManager;

    [Header("Character Info")]
    public GameObject[] selectedChars = new GameObject[4];
    [HideInInspector] public int numOfPlayers;
    private int lockedInPlayers;
    public int numOfRespawns;
    public int respawnTimer;
    List<PlayerBase> playerBases;

    private int livingPlayers;
    private int currentLevel;
    private string gameMode;

    [Header("Analytics")]
    private string[] characters = new string[2] { "", "" };
    private string[] skins = new string[2] { "", "" };

    [Header("Determining Victory")]
    [SerializeField] string gameOverText;
    private GameObject winner;
    private Text victoryText;
    private string victoryScene;

    [Header("Settings")]
    [HideInInspector] public bool isPostProcessing = true;

    void Start()
    {
        player = ReInput.players.GetPlayer("System");

        //audioManager = GetComponentInChildren<AudioManager>();
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        if (GameObject.FindGameObjectsWithTag("UniverseController").Length == 1) DontDestroyOnLoad(gameObject);
        else Destroy(gameObject);
    }

    private void Update()
    {
        if (SceneManager.GetActiveScene().name == "OptionsMenu")
        {
            if (Input.GetButtonDown("AllBButton")) { GameObject.Find("Cover").GetComponent<FadeController>().FadeToBlack("MainMenu"); }
        }
        if (SceneManager.GetActiveScene().name.Contains("Bios"))
        {
            if (Input.GetButtonDown("AllBButton")) { GameObject.Find("Cover").GetComponent<FadeController>().FadeToBlack("MainMenu"); ; }
        }
        else if (SceneManager.GetActiveScene().name.Contains("GameOver"))
        {
            if (Input.GetButtonDown("AllBButton") || Input.GetKeyDown(KeyCode.H))
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
                ReturnToMenu();
                Unlock(0);
                Unlock(1);
                Unlock(2);
                Unlock(3);
            }
        }
    }

    public void SelectedPlay()
    {
        numOfPlayers = 0;
        numOfPlayers = ReInput.controllers.controllerCount - 2;
        if (numOfPlayers < 2) { numOfPlayers = 2; }
        if (numOfPlayers > 4) { numOfPlayers = 4; }

        if (Input.GetKey(KeyCode.Alpha3)) { numOfPlayers = 3; }
        if (Input.GetKey(KeyCode.Alpha4)) { numOfPlayers = 4; }

        GameObject.Find("Cover").GetComponent<FadeController>().FadeToBlack(numOfPlayers + "CharacterSelectorPVP");
    }

    public void SelectedBios() { GameObject.Find("Cover").GetComponent<FadeController>().FadeToBlack("Bios"); }
    public void SelectedOptions() { GameObject.Find("Cover").GetComponent<FadeController>().FadeToBlack("OptionsMenu"); }
    public void Restart() { GameObject.Find("Cover").GetComponent<FadeController>().FadeToBlack("MainMenu"); }

    private void OnLevelWasLoaded(int level)
    {
        StartCoroutine(NewLevelLoad(level));
    }
    IEnumerator NewLevelLoad(int level)
    {
        yield return new WaitForEndOfFrame();

        //        print(livingPlayers + "|" + numOfPlayers + "Before");
        numOfPlayers = ReInput.controllers.controllerCount - 2;
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
            if (winner != null)
            {
                victoryText.text = gameOverText.Replace("<winner>", winner.name);

                winner.transform.SetParent(GameObject.Find("CharacterPlaceHolder").transform);
                winner.transform.localPosition = Vector3.zero;
                winner.transform.localScale = Vector3.one;
                PlayerBase winnerCode = winner.GetComponent<PlayerBase>();
                winnerCode.OnVictory();
                winnerCode.enabled = false;
                Rigidbody winnerRB = winnerCode.GetComponent<Rigidbody>();
                winnerRB.isKinematic = true;
                winnerRB.velocity = Vector3.zero;

                winnerCode.visuals.transform.LookAt(Camera.main.transform);
                winnerCode.visuals.transform.eulerAngles = new Vector3(0, winnerCode.visuals.transform.eulerAngles.y, 0);
            }
            else
                victoryText.text = "A Draw!";

            GameObject.Find("BackgroundController").GetComponent<GameOverBackgroundController>().SetBackground(victoryScene);
        }
        //Game-Level Setup
        else if (level >= firstArenaID)
        {
            Vector3 targetScale = new Vector3(1, 1, 1);
            Vector3 targetLook = new Vector3(0, 90, 0);
            Vector3 targetPos = new Vector3(0, 5, 0);
            int[] charInts = new int[4];
            livingPlayers = numOfPlayers;

            playerBases = new List<PlayerBase>();

            #region Player 1
            GameObject p1 = selectedChars[0];
            p1.SetActive(true);
            PlayerBase playerCode = p1.GetComponent<PlayerBase>();
            playerCode.thisPlayer = "P1";
            p1.tag = "Player1";
            p1.transform.SetParent(GameObject.Find("CentreBase").transform);
            p1.GetComponent<CapsuleCollider>().isTrigger = false;

            GameObject parent1 = GameObject.Find("Player1Base");
            parent1.transform.SetParent(p1.transform);
            parent1.transform.localPosition = targetPos;
            playerCode.TeleportPlayer(GameObject.Find("Player1Spawn").transform.position);
            p1.transform.localScale = Vector3.one;
            playerCode.visuals.transform.eulerAngles = targetLook;
            if (p1.name.Contains("Valderheim")) { charInts[0] = 0; }
            else if (p1.name.Contains("Songbird")) { charInts[0] = 1; }
            else if (p1.name.Contains("Carmen")) { charInts[0] = 2; }
            else if (p1.name.Contains("Wiosna")) { charInts[0] = 3; }
            else if (p1.name.Contains("Skjegg")) { charInts[0] = 4; }
            playerCode.SetInfo(this, 13);
            playerBases.Add(playerCode);
            #endregion

            targetLook = new Vector3(0, -90, 0);

            #region Player 2
            GameObject p2 = selectedChars[1];
            p2.SetActive(true);
            PlayerBase playerCode2 = p2.GetComponent<PlayerBase>();
            playerCode2.thisPlayer = "P2";
            p2.tag = "Player2";
            p2.transform.parent = GameObject.Find("CentreBase").transform;
            p2.GetComponent<CapsuleCollider>().isTrigger = false;

            GameObject parent2 = GameObject.Find("Player2Base");
            parent2.transform.SetParent(p2.transform);
            parent2.transform.localPosition = targetPos;
            playerCode2.TeleportPlayer(GameObject.Find("Player2Spawn").transform.position);
            playerCode2.visuals.transform.eulerAngles = targetLook;
            if (p2.name.Contains("Valderheim")) { charInts[1] = 0; }
            else if (p2.name.Contains("Songbird")) { charInts[1] = 1; }
            else if (p2.name.Contains("Carmen")) { charInts[1] = 2; }
            else if (p2.name.Contains("Wiosna")) { charInts[1] = 3; }
            else if (p2.name.Contains("Skjegg")) { charInts[1] = 4; }
            p2.transform.localScale = targetScale;
            playerCode2.SetInfo(this, 14);
            playerBases.Add(playerCode2);
            #endregion

            targetLook = new Vector3(0, 180, 0);

            #region Player 3
            PlayerBase playerCode3;
            if (selectedChars[2] != null)
            {
                GameObject p3 = selectedChars[2];
                p3.SetActive(true);
                playerCode3 = p3.GetComponent<PlayerBase>();
                playerCode3.thisPlayer = "P3";
                p3.tag = "Player3";
                p3.transform.parent = GameObject.Find("CentreBase").transform;
                p3.GetComponent<CapsuleCollider>().isTrigger = false;

                GameObject parent3 = GameObject.Find("Player3Base");
                parent3.transform.SetParent(p3.transform);
                parent3.transform.localPosition = targetPos;
                playerCode3.TeleportPlayer(GameObject.Find("Player3Spawn").transform.position);
                playerCode3.visuals.transform.eulerAngles = targetLook;
                if (p3.name.Contains("Valderheim")) { charInts[2] = 0; }
                else if (p3.name.Contains("Songbird")) { charInts[2] = 1; }
                else if (p3.name.Contains("Carmen")) { charInts[2] = 2; }
                else if (p3.name.Contains("Wiosna")) { charInts[2] = 3; }
                else if (p3.name.Contains("Skjegg")) { charInts[2] = 4; }
                p3.transform.localScale = targetScale;
                playerCode3.SetInfo(this, 15);
                playerBases.Add(playerCode3);
            }
            else { selectedChars[2] = new GameObject("NullObjectException"); }
            #endregion

            targetLook = new Vector3(0, 0, 0);

            #region Player 4
            PlayerBase playerCode4;
            if (selectedChars[3] != null)
            {
                GameObject p4 = selectedChars[3];
                p4.SetActive(true);
                playerCode4 = p4.GetComponent<PlayerBase>();
                playerCode4.thisPlayer = "P4";
                p4.tag = "Player4";
                p4.transform.parent = GameObject.Find("CentreBase").transform;
                p4.GetComponent<CapsuleCollider>().isTrigger = false;

                GameObject parent4 = GameObject.Find("Player4Base");
                parent4.transform.SetParent(p4.transform);
                parent4.transform.localPosition = targetPos;
                playerCode4.TeleportPlayer(GameObject.Find("Player4Spawn").transform.position);
                playerCode4.visuals.transform.eulerAngles = targetLook;
                if (p4.name.Contains("Valderheim")) { charInts[3] = 0; }
                else if (p4.name.Contains("Songbird")) { charInts[3] = 1; }
                else if (p4.name.Contains("Carmen")) { charInts[3] = 2; }
                else if (p4.name.Contains("Wiosna")) { charInts[3] = 3; }
                else if (p4.name.Contains("Skjegg")) { charInts[3] = 4; }
                p4.transform.localScale = targetScale;
                playerCode4.SetInfo(this, 16);
                playerBases.Add(playerCode4);
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

            CheckForDualCamera();
            StartCoroutine(DelayedStart(playerBases));
        }
    }

    IEnumerator DelayedStart(List<PlayerBase> p1)
    {
        yield return new WaitForSecondsRealtime(3.3f);
        for (int i = 0; i < p1.Count; i++)
            p1[i].enabled = true;
        //p2.enabled = true;
        //p3.enabled = true;
        //p4.enabled = true;
    }

    public void CheckReady(int arrayIndex, GameObject gobject, GameObject character, string skin)
    {
        StartCoroutine(DelayedCheckReady(arrayIndex, gobject, character, skin));
    }
    IEnumerator DelayedCheckReady(int arrayIndex, GameObject gobject, GameObject character, string skin)
    {
        yield return new WaitForSecondsRealtime(1);

        selectedChars[arrayIndex] = gobject;
        gobject.GetComponent<PlayerBase>().OnSelected();

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
            //print("Removing " + "Player" + deadPlayer + "Base" + " from " + "Player" + (i + 1) + "Base");
            PlayerBase playerI = GameObject.Find("Player" + (i + 1) + "Base").GetComponentInParent<PlayerBase>();
            playerI.RemoveTarget(GameObject.Find("Player" + deadPlayer + "Base").transform);
        }
    }

    public void PlayerDeath(GameObject player, GameObject otherPlayer)
    {
        PlayerBase playerCode = player.GetComponent<PlayerBase>();
        playerCode.numOfDeaths++;

        if (playerCode.numOfDeaths < numOfRespawns)
        {
            StartCoroutine(StartSpawn(playerCode, playerCode.playerID));
        }
        else
        {
            playerBases.RemoveAt(Mathf.Clamp(playerCode.playerID, 0, playerBases.Count));
            if (triCamCode != null && triCamCode.enabled)
                triCamCode.RemoveTarget(GameObject.Find("Player" + (playerCode.playerID + 1) + "Base").transform);
            RemovePlayerFromTargets(playerCode.playerID + 1);

            livingPlayers--;

            if (livingPlayers <= 1)
            {
                if (playerBases[0] != null) player = playerBases[0].gameObject;
                else if (playerBases[1] != null) player = playerBases[1].gameObject;
                else if (playerBases[2] != null) player = playerBases[2].gameObject;
                else if (playerBases[3] != null) player = playerBases[3].gameObject;

                winner = player;
                winner.transform.parent = transform;
                Invoke("EndGame", 2);
                Time.timeScale = 0.5f;
            }
            CheckForDualCamera();
        }
    }

    private void CheckForDualCamera()
    {
        triCamCode = Camera.main.GetComponent<TriObjectiveCamera>();
        DualObjectiveCamera dualCam = Camera.main.GetComponent<DualObjectiveCamera>();

        if (livingPlayers <= 2)
        {
            triCamCode.enabled = false;
            dualCam.enabled = true;
            List<PlayerBase> tempPlayerList = new List<PlayerBase>();
            tempPlayerList.AddRange(playerBases);

            dualCam.firstTarget = tempPlayerList[0].transform;
            dualCam.secondTarget = tempPlayerList[1].transform;
        }
        else
        {
            triCamCode.enabled = true;
            dualCam.enabled = false;
            List<PlayerBase> tempPlayerList = new List<PlayerBase>();
            tempPlayerList.AddRange(playerBases);
            triCamCode.targets = new List<Transform>();

            for (int i = 0; i < tempPlayerList.Count; i++)
            {
                if (tempPlayerList[i] != null)
                    triCamCode.targets.Add(tempPlayerList[i].transform);
            }

        }
    }

    void EndGame()
    {
        victoryScene = SceneManager.GetActiveScene().name;
        GameObject.Find("Cover").GetComponent<FadeController>().FadeToBlack("NewGameOver");
    }

    private IEnumerator StartSpawn(PlayerBase player, int playerInt)
    {
        yield return new WaitForSeconds(respawnTimer);
        player.enabled = true;
        // triCamCode.AddTarget(playerInt + 1);
        player.Respawn();
        player.TeleportPlayer(GameObject.Find("PlayerRespawnPoint").transform.position);
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
        if (SceneManager.GetActiveScene().name.Contains("3Character"))
        {
            charSelector3.locked = false;
        }
        if (SceneManager.GetActiveScene().name.Contains("4Character"))
        {
            charSelector3.locked = false;
            charSelector4.locked = false;
        }
        selectedChars[0] = null;
        selectedChars[1] = null;
        selectedChars[2] = null;
        selectedChars[3] = null;
        if (transform.childCount > 2) transform.GetChild(2).SetParent(Camera.main.transform);
        GameObject.Find("Cover").GetComponent<FadeController>().FadeToBlack("MainMenu");
    }

    public void GetCam(DualObjectiveCamera duoCam, TriObjectiveCamera triCam)
    {
        dualCamCode = duoCam;
        triCamCode = triCam;
    }

    IEnumerator DelayedVictory()
    {
        yield return new WaitForSeconds(0.1f);

        GameObject[] gams = GameObject.FindGameObjectsWithTag("Boss");
        for (int i = 0; i < gams.Length; i++)
        {
            if (gams[i].name == winner.name) { gams[i].SetActive(true); }
        }
    }

    public void CameraRumbleCall(float degree) { triCamCode.CamShake(degree); }

    // public void PlaySound(string clip) { audioManager.Play(clip); }

}
