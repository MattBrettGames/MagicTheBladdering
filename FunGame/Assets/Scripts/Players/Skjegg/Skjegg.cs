using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TotemType
{
    healing,
    damage,
    defence,
    speed,
    explosion
}

public class Skjegg : PlayerBase
{
    [Header("Components")]
    [SerializeField] Weapons leftFist;
    [SerializeField] Weapons rightFist;

    [Header("X Attack")]
    [SerializeField] int xDamage;
    [SerializeField] int xKnockback;

    [Header("Y Attack")]
    [SerializeField] int yDamage;
    [SerializeField] int yKnockback;

    [Header("Totem Vars")]
    [SerializeField] SpriteRenderer totemSprite;
    [SerializeField] Sprite[] totemSymbolArray = new Sprite[5];
    [SerializeField] GameObject[] totemGameObjectArray = new GameObject[5];
    List<TotemBase> totemBaseList = new List<TotemBase>();
    [SerializeField] float[] totemLifeSpans = new float[5];
    [SerializeField] float timeBetweenTotems;
    [Space]
    [SerializeField] int healthPerTotemTick;
    int i_currentTotem;
    bool hasTotemActive;
    bool isSelectingTotem;
    TotemBase activeTotemBase;
    TotemType currentTotem;



    public override void Start()
    {
        base.Start();

        for (int i = 0; i < totemGameObjectArray.Length; i++)
        {
            totemBaseList.Add(Instantiate(totemGameObjectArray[i]).GetComponent<TotemBase>());
        }
        StartCoroutine(SkjeggUpdate());
    }

    public IEnumerator SkjeggUpdate()
    {
        yield return new WaitForSeconds(timeBetweenTotems);

        if (i_currentTotem < 4) i_currentTotem++;
        else i_currentTotem = 0;

        totemSprite.sprite = totemSymbolArray[i_currentTotem];
        if (currentHealth < healthMax) currentHealth += healthPerTotemTick;
        if (currentHealth > healthMax) currentHealth = healthMax;
    }

    public override void XAction()
    {
        base.XAction();
        anim.SetTrigger("XAttack");
        rightFist.GainInfo(xDamage, xKnockback, visuals.transform.forward, true, 0, this, true);
        leftFist.GainInfo(xDamage, xKnockback, visuals.transform.forward, true, 0, this, true);
    }

    public override void YAction()
    {
        base.YAction();
        anim.SetTrigger("YAttack");
        rightFist.GainInfo(yDamage, yKnockback, visuals.transform.forward, true, 0, this, true);
        leftFist.GainInfo(yDamage, yKnockback, visuals.transform.forward, true, 0, this, true);
    }


    public override void BAction()
    {
        base.BAction();

        if (hasTotemActive)
        {
            activeTotemBase.RecallTotem(transform.position);
            hasTotemActive = false;
        }
        else
        {
            if (bTimer <= 0)
            {
                if (!isSelectingTotem)
                {
                    isSelectingTotem = true;
                    totemSprite.gameObject.SetActive(true);
                }
                else
                {
                    totemSprite.gameObject.SetActive(false);
                    isSelectingTotem = false;
                    anim.SetTrigger("BAttack");
                    PlaceTotem();
                }
            }
        }
    }


    void PlaceTotem()
    {
        totemBaseList[i_currentTotem].gameObject.transform.position = transform.position + (visuals.transform.forward * 2);
        totemBaseList[i_currentTotem].SummonTotem(totemLifeSpans[i_currentTotem], this);
        totemBaseList[i_currentTotem].gameObject.SetActive(true);
        hasTotemActive = true;

        switch (totemBaseList[i_currentTotem].thisTotemType)
        {
            case TotemType.healing:

                break;

            case TotemType.damage:
                damageMult += 0.2f;
                break;

            case TotemType.defence:
                incomingMult -= 0.2f;

                break;

            case TotemType.speed:
                bonusSpeed += 5;
                break;

            case TotemType.explosion:

                break;
        }

    }

    public void EndTotemEffect(TotemType incomingTotem)
    {
        switch (incomingTotem)
        {
            case TotemType.healing:

                break;

            case TotemType.damage:
                damageMult -= 0.2f;

                break;

            case TotemType.defence:
                incomingMult += 0.2f;

                break;

            case TotemType.speed:
                bonusSpeed -= 5;
                break;

            case TotemType.explosion:

                break;
        }
    }

}
