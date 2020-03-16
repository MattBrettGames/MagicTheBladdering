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
    [SerializeField] Weapons spiritAttackBox;
    [SerializeField] Weapons leftFist;
    [SerializeField] Weapons rightFist;

    [Header("X Attack")]
    [SerializeField] int xShortDamage;
    [SerializeField] int xLongDamage;
    [SerializeField] int xShortKnockback;
    [SerializeField] int xLongKnockback;
    [SerializeField] float xShortKnockbackDuration;
    [SerializeField] float xLongKnockbackDuration;
    float timeXHeld;
    [SerializeField] Vector2 minTimeHeldTOMaxHoldtime;

    [Header("Y Attack")]
    [SerializeField] int yDamage;
    [SerializeField] int yKnockback;
    [SerializeField] float yKnockbackDuration;

    [Header("Totem Vars")]
    [SerializeField] SpriteRenderer totemSprite;
    [SerializeField] Sprite[] totemSymbolArray = new Sprite[5];
    [SerializeField] GameObject[] totemGameObjectArray = new GameObject[5];
    List<TotemBase> totemBaseList = new List<TotemBase>();
    [SerializeField] float[] totemLifeSpans = new float[5];
    [SerializeField] GameObject[] orbitingSymbols = new GameObject[5];
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
    [SerializeField] float ramBurstRadius;
    [SerializeField] int ramBurstPower;
    [SerializeField] float ramBurstKnockbackDur;
    [SerializeField] int ramAttackKnockback;
    [SerializeField] float ramAttackKnockbackDur;

    [Header("Bird Stats")]
    [SerializeField] int birdBonusSpeed;
    [SerializeField, Tooltip("This is as a percentage out of 100%")] int birdCritChance;
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

        // anim.ResetTrigger("XAttack");
        // anim.ResetTrigger("YAttack");
    }

    public override void Update()
    {
        if (!isAI)
        {
            if (aTimer > 0) aTimer -= Time.deltaTime;
            if (bTimer > 0) bTimer -= Time.deltaTime;
            if (xTimer > 0) xTimer -= Time.deltaTime;
            if (yTimer > 0) yTimer -= Time.deltaTime;

            dir = new Vector3(player.GetAxis("HoriMove"), 0, player.GetAxis("VertMove"));
            if (dir != Vector3.zero)
                lastDir = dir;

            aimTarget.position = transform.position + (dir * 2) + lastDir;

            if (anim.GetCurrentAnimatorStateInfo(0).IsName("Idle") || anim.GetCurrentAnimatorStateInfo(0).IsName("Walking")) acting = false;

            transform.position = new Vector3(transform.position.x, 0, transform.position.z);

            switch (state)
            {
                case State.stun:
                    anim.SetBool("Stunned", true);
                    break;

                case State.attack:
                    break;

                case State.normal:

                    anim.SetBool("LockOn", false);
                    if (player.GetAxis("LockOn") >= 0.4f) { state = State.lockedOn; }

                    if (!acting)
                    {
                        //Rotating the Character Model
                        visuals.transform.LookAt(aimTarget);
                        rb2d.velocity = dir * (speed + bonusSpeed);

                        //Standard Inputs
                        if (player.GetButtonDown("AAction")) { AAction(true); }
                        if (player.GetButtonDown("BAttack")) { BAction(); }
                        if (player.GetButtonDown("YAttack")) { YAction(); }


                        if (player.GetButton("XAttack")) XAction();
                        if (player.GetButtonUp("XAttack")) ReleaseXAction();

                        anim.SetFloat("Movement", dir.magnitude + 0.001f);
                    }
                    break;

                case State.lockedOn:

                    walkDirection.position = dir + transform.position;

                    anim.SetBool("LockOn", true);
                    if (player.GetAxis("LockOn") <= 0.4f) { state = State.normal; }

                    if (!acting)
                    {
                        rb2d.velocity = dir * (speed + bonusSpeed);

                        if (player.GetButtonDown("AAction")) { AAction(true); }
                        if (player.GetButtonDown("BAttack")) { BAction(); }
                        if (player.GetButtonDown("YAttack")) { YAction(); }

                        if (player.GetButton("XAttack")) XAction();
                        if (player.GetButtonUp("XAttack")) ReleaseXAction();

                        anim.SetFloat("Movement", dir.magnitude + 0.001f);
                        anim.SetFloat("Movement_X", visuals.transform.InverseTransformDirection(rb2d.velocity).x / speed);
                        anim.SetFloat("Movement_ZY", visuals.transform.InverseTransformDirection(rb2d.velocity).z / speed);

                        aimTarget.LookAt(lockTargetList[currentLock].position + lookAtVariant);

                        visuals.transform.forward = Vector3.Lerp(visuals.transform.forward, aimTarget.forward, lockOnLerpSpeed);

                        LockOnScroll();
                    }

                    break;

                case State.dodging:

                    if (aTimer <= 0)
                    {
                        DodgeSliding(visuals.transform.forward);
                    }
                    break;

                case State.knockback:
                    KnockbackContinual();
                    break;
            }
        }

        // This bit is the AI
        else
        {
            AIUpdate();
        }
    }

    public override void XAction()
    {
        if (xTimer <= 0 && !acting)
        {
            anim.SetBool("Charging", true);
            timeXHeld += Time.deltaTime;

            if (timeXHeld >= minTimeHeldTOMaxHoldtime.x)
            {
                anim.SetBool("Charging", false);
                anim.SetTrigger("LongPunch");
                rightFist.GainInfo(xLongDamage, xLongKnockback, visuals.transform.forward, true, 0, this, true, AttackType.X, xLongKnockbackDuration);
                leftFist.GainInfo(xLongDamage, xLongKnockback, visuals.transform.forward, true, 0, this, true, AttackType.X, xLongKnockbackDuration);

                base.XAction();
                timeXHeld = 0;
                xTimer = xCooldown;
            }
        }
    }
    void ReleaseXAction()
    {
        anim.SetBool("Charging", false);
        if (timeXHeld < minTimeHeldTOMaxHoldtime.x && !acting)
        {
            anim.SetTrigger("ShortPunch");
            rightFist.GainInfo(xShortDamage, xShortKnockback, visuals.transform.forward, true, 0, this, true, AttackType.X, xShortKnockbackDuration);
            leftFist.GainInfo(xShortDamage, xShortKnockback, visuals.transform.forward, true, 0, this, true, AttackType.X, xShortKnockbackDuration);
        }
        else if (!acting)
        {
            anim.SetTrigger("LongPunch");
            rightFist.GainInfo(xLongDamage, xLongKnockback, visuals.transform.forward, true, 0, this, true, AttackType.X, xLongKnockbackDuration);
            leftFist.GainInfo(xLongDamage, xLongKnockback, visuals.transform.forward, true, 0, this, true, AttackType.X, xLongKnockbackDuration);
        }
        base.XAction();
        timeXHeld = 0;
        xTimer = xCooldown;
    }


    public override void YAction()
    {
        if (yTimer <= 0)
        {
            base.YAction();
            anim.SetTrigger("YAttack");
            rightFist.GainInfo(yDamage, yKnockback, visuals.transform.forward, true, 0, this, true, AttackType.Y, yKnockbackDuration);
            leftFist.GainInfo(yDamage, yKnockback, visuals.transform.forward, true, 0, this, true, AttackType.Y, yKnockbackDuration);
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
                totemChoiceList = new List<int> { 0, 1, 2, 3, 4 };
                for (int i = 0; i < 2; i++)
                {
                    totemChoiceList.RemoveAt(Random.Range(0, totemChoiceList.Count));
                }
                isSelectingTotem = true;
                totemSprite.sprite = null;
                totemSprite.gameObject.SetActive(true);
            }
            else
            {
                anim.SetTrigger("BAttack");
                totemSprite.gameObject.SetActive(false);
                isSelectingTotem = false;
                bTimer = bCooldown;
                PlaceTotem();
            }
        }
    }

    public override void OnHit(PlayerBase hitTarget, AttackType hitWith)
    {
        base.OnHit(hitTarget, hitWith);

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
            hitTarget.Knockback(ramAttackKnockback, visuals.transform.forward, ramAttackKnockbackDur);
        }

        if (isBird)
        {
            if (Random.Range(0, 100) <= birdCritChance)
            {
                hitTarget.TakeDamage(birdCritDamageBonus, Vector3.zero, 0, false, true, this, 0);
            }
        }
    }

    void PlaceTotem()
    {
        totemBaseList[totemChoiceList[i_currentTotem]].gameObject.transform.position = transform.position + (visuals.transform.forward * 2);
        totemBaseList[totemChoiceList[i_currentTotem]].SummonTotem(totemLifeSpans[totemChoiceList[i_currentTotem]], this);
        totemBaseList[totemChoiceList[i_currentTotem]].gameObject.SetActive(true);
        orbitingSymbols[totemChoiceList[i_currentTotem]].SetActive(true);

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
                if (Vector3.Distance(totemBaseList[totemChoiceList[i_currentTotem]].gameObject.transform.position, lockTargetList[currentLock].transform.position) <= ramBurstRadius)
                {
                    lockTargetList[currentLock].gameObject.GetComponentInParent<PlayerBase>().Knockback(ramBurstPower,
                        lockTargetList[currentLock].transform.position - totemBaseList[totemChoiceList[i_currentTotem]].gameObject.transform.position
                        , ramBurstKnockbackDur);
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
                orbitingSymbols[0].SetActive(false);
                break;

            case TotemType.wolf:
                isWolf = false;
                anim.SetFloat("AttackSpeedMult", 1);
                orbitingSymbols[1].SetActive(false);

                break;

            case TotemType.bear:
                isBear = false;
                damageMult = 1;
                orbitingSymbols[2].SetActive(false);
                break;

            case TotemType.ram:
                isRam = false;
                orbitingSymbols[3].SetActive(false);
                break;

            case TotemType.bird:
                isBird = false;
                bonusSpeed -= birdBonusSpeed;
                orbitingSymbols[4].SetActive(false);
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
        if (!isWolf)
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