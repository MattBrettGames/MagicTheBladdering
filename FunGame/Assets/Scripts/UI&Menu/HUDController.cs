using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class HUDController : BlankMono
{
    [Header("Healthbar")]
    public GameObject healthBar;
    public GameObject barBorder;
    public Text characterName;
    [SerializeField] Text healthText;

    [Header("Cooldowns")]
    public Image aBanner;
    Text aBannerText;
    public Image bBanner;
    Text bBannerText;
    public Image xBanner;
    Text xBannerText;
    public Image yBanner;
    Text yBannerText;
    [Space]
    [SerializeField] GameObject aParticle;
    bool aReady;
    [SerializeField] GameObject bParticle;
    bool bReady;
    [SerializeField] GameObject xParticle;
    bool xReady;
    [SerializeField] GameObject yParticle;
    bool yReady;

    [Header("Misc Stuff")]
    public GameObject playerBase;
    public string thisPlayerInt;
    public GameObject[] images = new GameObject[4];
    public GameObject image;
    [SerializeField] GameObject myHud;
    [SerializeField] GameObject[] skulls = new GameObject[3];
    private PlayerBase targetPlayer;
    [HideInInspector] bool isUsed;

    [Header("Ability Symbols")]
    [SerializeField] SkillImages[] abilitySymbols = new SkillImages[4];
    Vector3 usedAbilitySize = new Vector3(0.371f, 0.371f, 0.371f);
    Vector3 readyAbilitySize = new Vector3(0.4f, 0.4f, 0.4f);

    public void SetStats(int imageInt, string charName, bool shouldBeActive)
    {
        myHud.SetActive(shouldBeActive);
        gameObject.SetActive(shouldBeActive);

        aBannerText = aBanner.GetComponentInChildren<Text>();
        bBannerText = bBanner.GetComponentInChildren<Text>();
        xBannerText = xBanner.GetComponentInChildren<Text>();
        yBannerText = yBanner.GetComponentInChildren<Text>();

        characterName.text = charName;

        targetPlayer = playerBase.GetComponentInParent<PlayerBase>();
        if (targetPlayer == null)
        {
            myHud.SetActive(false);
            return;
        }

        healthBar.transform.localScale = new Vector3(targetPlayer.currentHealth / 50f, 0.2f, 1);
        barBorder.transform.localScale = new Vector3(targetPlayer.currentHealth / 50f, 0.2f, 1);

        image.SetActive(false);
        image = images[imageInt];
        image.SetActive(true);

        aBanner.sprite = abilitySymbols[imageInt].aSymbol;
        bBanner.sprite = abilitySymbols[imageInt].bSymbol;
        xBanner.sprite = abilitySymbols[imageInt].xSymbol;
        yBanner.sprite = abilitySymbols[imageInt].ySymbol;

        for (int i = 0; i < skulls.Length; i++)
        {
            skulls[i].SetActive(false);
        }

        Invoke("ProperStartup", 3.3f);
    }

    void ProperStartup()
    {
        aReady = true;
        bReady = true;
        xReady = true;
        yReady = true;
    }

    public void PlayerDeath()
    {
        skulls[targetPlayer.numOfDeaths].SetActive(true);
    }

    public void UsedA() { aReady = true; }
    public void UsedB() { bReady = true; }
    public void UsedX() { xReady = true; }
    public void UsedY() { yReady = true; }
    
    public void Update()
    {
        healthBar.transform.localScale = Vector3.Lerp(healthBar.transform.localScale, new Vector3(targetPlayer.currentHealth / 50f, 0.2f, 1), 0.3f);
        healthText.text = targetPlayer.currentHealth + "/" + targetPlayer.healthMax;

        if (targetPlayer.aTimer <= 0)
        {
            aBannerText.text = "";
            aBanner.color = Color.white;
            aBanner.transform.localScale = readyAbilitySize;
            TriggerABurst();
        }
        else
        {
            aBanner.color = Color.grey;
            aBannerText.text = Mathf.RoundToInt(targetPlayer.aTimer) + "";
            aBanner.transform.localScale = usedAbilitySize;
        }

        if (targetPlayer.bTimer <= 0)
        {
            bBannerText.text = "";
            bBanner.color = Color.white;
            bBanner.transform.localScale = readyAbilitySize;
            TriggerBBurst();
        }
        else
        {
            bBanner.color = Color.grey;
            bBannerText.text = Mathf.RoundToInt(targetPlayer.bTimer) + "";
            bBanner.transform.localScale = usedAbilitySize;
        }

        if (targetPlayer.xTimer <= 0)
        {
            xBannerText.text = "";
            xBanner.color = Color.white;
            xBanner.transform.localScale = readyAbilitySize;
            TriggerXBurst();
        }
        else
        {
            xBanner.color = Color.grey;
            xBannerText.text = Mathf.RoundToInt(targetPlayer.xTimer) + "";
            xBanner.transform.localScale = usedAbilitySize;
        }

        if (targetPlayer.yTimer <= 0)
        {
            yBannerText.text = "";
            yBanner.color = Color.white;
            yBanner.transform.localScale = readyAbilitySize;
            TriggerYBurst();
        }
        else
        {
            yBanner.color = Color.grey;
            yBannerText.text = Mathf.RoundToInt(targetPlayer.yTimer) + "";
            yBanner.transform.localScale = usedAbilitySize;
        }

        if (targetPlayer.currentHealth < 0) targetPlayer.currentHealth = 0;
    }

    #region Particle Bursts
    void TriggerABurst()
    {
        if (aReady)
        {
            aParticle.SetActive(false);
            aParticle.transform.parent = targetPlayer.transform;
            aParticle.transform.localPosition = Vector3.zero;
            aParticle.SetActive(true);
            aReady = false;
        }
    }

    void TriggerBBurst()
    {
        if (bReady)
        {
            bParticle.SetActive(false);
            bParticle.transform.parent = targetPlayer.transform;
            bParticle.transform.localPosition = Vector3.zero;
            bParticle.SetActive(true);
            bReady = false;
        }
    }

    void TriggerXBurst()
    {
        if (xReady)
        {
            xParticle.SetActive(false);
            xParticle.transform.parent = targetPlayer.transform;
            xParticle.transform.localPosition = Vector3.zero;
            xParticle.SetActive(true);
            xReady = false;
        }
    }
    void TriggerYBurst()
    {
        if (yReady)
        {
            yParticle.SetActive(false);
            yParticle.transform.parent = targetPlayer.transform;
            yParticle.transform.localPosition = Vector3.zero;
            yParticle.SetActive(true);
            yReady = false;
        }
    }
    #endregion

    [Serializable]
    public struct SkillImages
    {
        public Sprite aSymbol;
        public Sprite bSymbol;
        public Sprite xSymbol;
        public Sprite ySymbol;
    }

}