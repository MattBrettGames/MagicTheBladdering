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

    [Header("Cooldowns")]
    public Image aBanner;
    Text aBannerText;
    public Image bBanner;
    Text bBannerText;
    public Image xBanner;
    Text xBannerText;
    public Image yBanner;
    Text yBannerText;

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

    }

    public void PlayerDeath()
    {
        skulls[targetPlayer.numOfDeaths].SetActive(true);
    }

    public void LateUpdate()
    {
        healthBar.transform.localScale = Vector3.Lerp(healthBar.transform.localScale, new Vector3(targetPlayer.currentHealth / 50f, 0.2f, 1), 0.3f);

        if (targetPlayer.aTimer <= 0)
        {
            aBannerText.text = "";
            aBanner.color = Color.white;
        }
        else
        {
            aBanner.color = Color.grey;
            aBannerText.text = Mathf.RoundToInt(targetPlayer.aTimer) + "";
        }

        if (targetPlayer.bTimer <= 0)
        {
            bBannerText.text = "";
            bBanner.color = Color.white;
        }
        else
        {
            bBanner.color = Color.grey;
            bBannerText.text = Mathf.RoundToInt(targetPlayer.bTimer) + "";
        }

        if (targetPlayer.xTimer <= 0)
        {
            xBannerText.text = "";
            xBanner.color = Color.white;
        }
        else
        {
            xBanner.color = Color.grey;
            xBannerText.text = Mathf.RoundToInt(targetPlayer.xTimer) + "";
        }

        if (targetPlayer.yTimer <= 0)
        {
            yBannerText.text = "";
            yBanner.color = Color.white;
        }
        else
        {
            yBanner.color = Color.grey;
            yBannerText.text = Mathf.RoundToInt(targetPlayer.yTimer) + "";
        }

        if (targetPlayer.currentHealth < 0) targetPlayer.currentHealth = 0;
    }

    [Serializable]
    public struct SkillImages
    {
        public Sprite aSymbol;
        public Sprite bSymbol;
        public Sprite xSymbol;
        public Sprite ySymbol;
    }

}