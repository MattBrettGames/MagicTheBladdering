using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TotemType
{
    turtle,
    wolf,
    bear,
    ram,
    bird
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
    [SerializeField, Tooltip("This is the time it takes for the totem options to cycle")] float timeBetweenTotems;
    int i_currentTotem;
    bool isSelectingTotem;

    bool isTurtle;
    bool isWolf;
    bool isBear;
    bool isRam;
    bool isBird;

    List<int> totemChoiceList = new List<int>();

    [Header("Turtle Stats")]
    [SerializeField] int turtleHealthGainPerHit;

    [Header("Wolf Stats")]
    [SerializeField] float bleedDur;
    [SerializeField] float wolfAttackSpeed;

    [Header("Bear Stats")]
    [SerializeField] float bearDamageGainedOnAttack;

    [Header("Ram Stats")]
    [SerializeField] float ramBurstDistance;
    [SerializeField] int ramBurstPower;
    [SerializeField] int ramAttackKnockback;

    [Header("Bird Stats")]
    [SerializeField] int birdBonusSpeed;
    [SerializeField] int birdCritChance;
    [SerializeField] int birdCritDamageBonus;



    public override void Start()
    {
        base.Start();

        for (int i = 0; i < totemGameObjectArray.Length; i++)
        {
            totemBaseList.Add(Instantiate(totemGameObjectArray[i]).GetComponent<TotemBase>());
            totemBaseList[i].gameObject.SetActive(false);
        }
        StartCoroutine(SkjeggUpdate());
        anim.SetFloat("AttackSpeedMult", 1);
    }


    public IEnumerator SkjeggUpdate()
    {
        yield return new WaitForSeconds(timeBetweenTotems);
        StartCoroutine(SkjeggUpdate());

        if (i_currentTotem < 2) i_currentTotem++;
        else i_currentTotem = 0;

        totemSprite.sprite = totemSymbolArray[totemChoiceList[i_currentTotem]];
    }

    public override void XAction()
    {
        if (xTimer <= 0)
        {
            base.XAction();
            anim.SetTrigger("XAttack");
            rightFist.GainInfo(xDamage, xKnockback, visuals.transform.forward, true, 0, this, true);
            leftFist.GainInfo(xDamage, xKnockback, visuals.transform.forward, true, 0, this, true);
            xTimer = xCooldown;
        }
    }

    public override void YAction()
    {
        if (yTimer <= 0)
        {
            base.YAction();
            anim.SetTrigger("YAttack");
            rightFist.GainInfo(yDamage, yKnockback, visuals.transform.forward, true, 0, this, true);
            leftFist.GainInfo(yDamage, yKnockback, visuals.transform.forward, true, 0, this, true);
            yTimer = yCooldown;
        }
    }


    public override void BAction()
    {

        if (bTimer <= 0)
        {
            base.BAction();

            if (!isSelectingTotem)
            {
                totemChoiceList.AddRange(new List<int> { 0, 1, 2, 3, 4 });
                for (int i = 0; i < 2; i++)
                {
                    totemChoiceList.RemoveAt(Random.Range(0, totemChoiceList.Count));
                }
                isSelectingTotem = true;
                totemSprite.gameObject.SetActive(true);
            }
            else
            {
                totemSprite.gameObject.SetActive(false);
                isSelectingTotem = false;
                anim.SetTrigger("BAttack");
                bTimer = bCooldown;
                PlaceTotem();
            }
        }
    }

    public override void OnHit(PlayerBase hitTarget)
    {
        base.OnHit(hitTarget);
        print(isTurtle + "|" + isWolf + "|" + isBear + "|" + isRam + "|" + isBird);

        if (isTurtle)
        {
            currentHealth += turtleHealthGainPerHit;
            if (currentHealth > healthMax) currentHealth = healthMax;
        }

        if (isWolf)
        {
            hitTarget.poison = true;
            StartCoroutine(EndTargetPoison(hitTarget));
        }

        if (isBear)
        {
            damageMult += bearDamageGainedOnAttack;
        }

        if (isRam)
        {
            hitTarget.Knockback(ramAttackKnockback, visuals.transform.forward);
        }

        if (isBird)
        {
            if (Random.Range(0, 100) <= birdCritChance)
            {
                hitTarget.TakeDamage(birdCritDamageBonus, Vector3.zero, 0, false, true, this);
            }
        }
    }

    void PlaceTotem()
    {
        totemBaseList[totemChoiceList[i_currentTotem]].gameObject.transform.position = transform.position + (visuals.transform.forward * 2);
        totemBaseList[totemChoiceList[i_currentTotem]].SummonTotem(totemLifeSpans[totemChoiceList[i_currentTotem]], this);
        totemBaseList[totemChoiceList[i_currentTotem]].gameObject.SetActive(true);

        switch (totemBaseList[totemChoiceList[i_currentTotem]].thisTotemType)
        {
            case TotemType.turtle:
                isTurtle = true;
                currentHealth += Mathf.RoundToInt(healthMax * 0.1f);
                if (currentHealth > healthMax) currentHealth = healthMax;
                break;

            case TotemType.wolf:
                isWolf = true;
                anim.SetFloat("AttackSpeedMult", wolfAttackSpeed);
                break;

            case TotemType.bear:
                isBear = true;
                break;

            case TotemType.ram:
                isRam = true;
                if (Vector3.Distance(totemBaseList[totemChoiceList[i_currentTotem]].gameObject.transform.position, lockTargetList[currentLock].transform.position) < ramBurstDistance)
                {
                    lockTargetList[currentLock].gameObject.GetComponent<PlayerBase>().Knockback(ramBurstPower,
                        lockTargetList[currentLock].transform.position - totemBaseList[totemChoiceList[i_currentTotem]].gameObject.transform.position
                        );
                }
                break;

            case TotemType.bird:
                isBird = true;
                bonusSpeed += birdBonusSpeed;
                break;
        }
    }

    public void EndTotemEffect(TotemType incomingTotem)
    {
        switch (incomingTotem)
        {
            case TotemType.turtle:
                isTurtle = false;
                break;

            case TotemType.wolf:
                isWolf = false;
                anim.SetFloat("AttackSpeedMult", 1);

                break;

            case TotemType.bear:
                isBear = false;
                damageMult = 1;
                break;

            case TotemType.ram:
                isRam = false;
                break;

            case TotemType.bird:
                isBird = false;
                bonusSpeed -= birdBonusSpeed;
                break;
        }
    }


    #region Wolf Functions
    IEnumerator EndTargetPoison(PlayerBase hitTarget)
    {
        yield return new WaitForSeconds(bleedDur);
        hitTarget.poison = false;
    }

    public override void BeginActing()
    {
        if (isWolf)
            base.BeginActing();
    }
    #endregion

    #region Bear Functions
    public override void GainHA()
    {
        if (isBear)
            base.GainHA();
    }
    #endregion

}