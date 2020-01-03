using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUDController : BlankMono
{
    [Header("Healthbar")]
    public GameObject healthBar;
    public GameObject barBorder;
    public Text characterName;

    [Header("Cooldowns")]
    public GameObject aBanner;
    RectTransform aBannerRect;
    public GameObject bBanner;
    RectTransform bBannerRect;
    public GameObject xBanner;
    RectTransform xBannerRect;
    public GameObject yBanner;
    RectTransform yBannerRect;

    [Header("Misc Stuff")]
    public GameObject playerBase;
    public string thisPlayerInt;
    public GameObject[] images = new GameObject[4];
    public GameObject image;
    private PlayerBase targetPlayer;

    public void SetStats(int imageInt, string charName)
    {
        print(imageInt + " is the current player for " + gameObject.name);

        aBannerRect = aBanner.GetComponent<RectTransform>();
        bBannerRect = bBanner.GetComponent<RectTransform>();
        xBannerRect = xBanner.GetComponent<RectTransform>();
        yBannerRect = yBanner.GetComponent<RectTransform>();

        characterName.text = charName;

        targetPlayer = playerBase.GetComponentInParent<PlayerBase>();
        healthBar.transform.localScale = new Vector3(targetPlayer.currentHealth / 50f, 0.2f, 1);
        barBorder.transform.localScale = new Vector3(targetPlayer.currentHealth / 50f, 0.2f, 1);
        image.SetActive(false);
        image = images[imageInt];
        image.SetActive(true);
    }

    public void Update()
    {
        healthBar.transform.localScale = Vector3.Lerp(healthBar.transform.localScale, new Vector3(targetPlayer.currentHealth / 50f, 0.2f, 1), 0.3f);

        aBannerRect.sizeDelta = new Vector2(100, (targetPlayer.aCooldown - targetPlayer.aTimer)*5);
        bBannerRect.sizeDelta = new Vector2(100, (targetPlayer.bCooldown - targetPlayer.bTimer)*5);
        xBannerRect.sizeDelta = new Vector2(100, (targetPlayer.xCooldown - targetPlayer.xTimer)*5);
        yBannerRect.sizeDelta = new Vector2(100, (targetPlayer.yCooldown - targetPlayer.yTimer)*5);

        Mathf.Clamp(targetPlayer.currentHealth, 0, 2000);
    }
}