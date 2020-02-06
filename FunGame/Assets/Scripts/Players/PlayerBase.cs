using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;
using System;

public abstract class PlayerBase : ThingThatCanDie
{
    [Header("GameMode Stuff")]
    public string thisPlayer;
    public int playerID;
    [HideInInspector] public int numOfDeaths = 0;
    public bool pvp;

    [Header("Movement Stats")]
    public float speed;
    public float dodgeSpeed;
    public float dodgeDur;
    [SerializeField] protected float bonusSpeed;
    protected float moving;
    [HideInInspector] public Vector3 dir;

    [Header("Common Stats")]
    public float damageMult = 1;
    public float incomingMult = 1;

    [Header("Status Effects")]
    [SerializeField] private int poisonPerTick;
    [SerializeField] private float secsBetweenTicks;
    [SerializeField] public bool poison;
    private bool hyperArmour;
    protected bool iFrames;
    [HideInInspector] public bool trueIFrames;
    [HideInInspector] public bool hazardFrames;
    protected bool acting;
    protected Vector3 knockbackForce;
    private float knockBackPower;
    [HideInInspector] public State state;
    [HideInInspector]
    public enum State
    {
        normal,
        lockedOn,
        dodging,
        knockback,
        attack,
        unique,
        stun
    }

    [Header("Components")]
    public GameObject visuals;
    [SerializeField] GameObject respawnEffects;
    [SerializeField] GameObject hitEffects;
    [HideInInspector] public Transform aimTarget;
    protected Outline outline;
    protected Animator anim;
    protected Rigidbody rb2d;
    protected PlayerController playerCont;
    protected Player player;
    protected Transform walkDirection;
    protected UniverseController universe;
    HUDController HUD;

    [Header("Lockon Mechanics")]
    [SerializeField] protected float lockOnLerpSpeed = 0.3f;
    protected int currentLock;
    protected List<Transform> lockTargetList = new List<Transform>();
    protected Vector3 lookAtVariant = new Vector3(0, -5, 0);
    protected Transform currentLockTran;
    protected bool onCooldown;
    bool dead;

    [Header("Cooldowns")]
    public float aCooldown;
    public float bCooldown;
    public float xCooldown;
    public float yCooldown;
    [HideInInspector] public float aTimer;
    [HideInInspector] public float bTimer;
    [HideInInspector] public float xTimer;
    [HideInInspector] public float yTimer;

    [Header("Sound String")]
    [SerializeField] protected string aSound;
    [SerializeField] protected string bSound;
    [SerializeField] protected string xSound;
    [SerializeField] protected string ySound;
    [SerializeField] protected string ouchSound;
    [SerializeField] protected string deathSound;

    public virtual void Start()
    {
        outline = visuals.GetComponent<Outline>();
        anim = gameObject.GetComponentInChildren<Animator>();
        rb2d = gameObject.GetComponent<Rigidbody>();
        bTimer = bCooldown;

        healthMax = currentHealth;
        //StartCoroutine(PoisonTick());
        player = ReInput.players.GetPlayer(playerID);
        walkDirection = new GameObject("WalkDirection").transform;
        HUD = GameObject.Find(thisPlayer + "HUDController").GetComponent<HUDController>();
    }

    public virtual void SetInfo(UniverseController uni, int layerNew)
    {
        universe = uni;
        gameObject.layer = layerNew;

        RegainTargets();

        aimTarget = new GameObject("Aimer").transform;
        StartCoroutine(PoisonTick());
    }

    public void RegainTargets()
    {
        lockTargetList.Clear();

        if (playerID == 0)
        {
            lockTargetList.Add(GameObject.Find("Player2Base").transform);
            lockTargetList.Add(GameObject.Find("Player3Base").transform);
            lockTargetList.Add(GameObject.Find("Player4Base").transform);
        }
        else if (playerID == 1)
        {
            lockTargetList.Add(GameObject.Find("Player1Base").transform);
            lockTargetList.Add(GameObject.Find("Player3Base").transform);
            lockTargetList.Add(GameObject.Find("Player4Base").transform);
        }
        else if (playerID == 2)
        {
            lockTargetList.Add(GameObject.Find("Player2Base").transform);
            lockTargetList.Add(GameObject.Find("Player1Base").transform);
            lockTargetList.Add(GameObject.Find("Player4Base").transform);
        }
        else
        {
            lockTargetList.Add(GameObject.Find("Player2Base").transform);
            lockTargetList.Add(GameObject.Find("Player3Base").transform);
            lockTargetList.Add(GameObject.Find("Player1Base").transform);
        }
    }

    public void RemoveTarget(Transform targetTemp)
    {
        lockTargetList.Remove(targetTemp);
        currentLock = 0;
    }

    public virtual void Update()
    {
        if (aTimer > 0) aTimer -= Time.deltaTime;
        if (bTimer > 0) bTimer -= Time.deltaTime;
        if (xTimer > 0) xTimer -= Time.deltaTime;
        if (yTimer > 0) yTimer -= Time.deltaTime;

        dir = new Vector3(player.GetAxis("HoriMove"), 0, player.GetAxis("VertMove"));

        aimTarget.position = transform.position + dir * 5;

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
                    if (player.GetButtonDown("AAction")) { AAction(); }
                    if (player.GetButtonDown("BAttack") || Input.GetKeyDown(KeyCode.B)) { BAction(); }
                    if (player.GetButtonDown("XAttack")) { XAction(); }
                    if (player.GetButtonDown("YAttack")) { YAction(); }

                    anim.SetFloat("Movement", dir.magnitude + 0.001f);
                }
                else
                {
                    dir = Vector3.zero;
                }
                break;

            case State.lockedOn:

                walkDirection.position = dir + transform.position;

                anim.SetBool("LockOn", true);
                if (player.GetAxis("LockOn") <= 0.4f) { state = State.normal; }

                if (!acting)
                {
                    rb2d.velocity = dir * (speed + bonusSpeed);

                    if (player.GetButtonDown("AAction")) { AAction(); }
                    if (player.GetButtonDown("BAttack")) { BAction(); }
                    if (player.GetButtonDown("XAttack")) { XAction(); }
                    if (player.GetButtonDown("YAttack")) { YAction(); }

                    anim.SetFloat("Movement", dir.magnitude + 0.001f);
                    anim.SetFloat("Movement_X", visuals.transform.InverseTransformDirection(rb2d.velocity).x / speed);
                    anim.SetFloat("Movement_ZY", visuals.transform.InverseTransformDirection(rb2d.velocity).z / speed);

                    aimTarget.LookAt(lockTargetList[currentLock].position + lookAtVariant);

                    visuals.transform.forward = Vector3.Lerp(visuals.transform.forward, aimTarget.forward, lockOnLerpSpeed * Time.deltaTime);

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

        if (Input.GetKeyDown(KeyCode.H))
        {
            if (thisPlayer != "P1")
            {
                TakeDamage(3000, Vector3.zero, 0, true, false, this);
            }
        }
    }

    protected virtual void LockOnScroll()
    {
        if (player.GetAxis("CharRotate") >= 0.2f && !onCooldown)
        {
            if (currentLock < lockTargetList.Count - 1)
            {
                currentLock++;
                StartCoroutine(OffCooldown());
            }
            else
            {
                currentLock = 0;
                StartCoroutine(OffCooldown());
            }
        }
        if (player.GetAxis("CharRotate") <= -0.2 && !onCooldown)
        {
            if (currentLock != 0)
            {
                currentLock--;
                StartCoroutine(OffCooldown());
            }
            else
            {
                currentLock = lockTargetList.Count - 1;
                StartCoroutine(OffCooldown());
            }
        }
    }

    IEnumerator OffCooldown()
    {
        onCooldown = true;
        yield return new WaitForSecondsRealtime(0.1f);
        onCooldown = false;
    }

    #region Input Actions
    public virtual void AAction()
    {
        if (aTimer <= 0 && dir != Vector3.zero)
        {
            HUD.UsedA();

            anim.SetTrigger("AAction");

            state = State.dodging;

            Invoke("EndDodge", dodgeDur);

            universe.PlaySound(aSound);
        }
    }
    public virtual void EndDodge()
    {
        state = State.normal;
        aTimer = aCooldown;
    }

    public virtual void BAction()
    {
        HUD.UsedB();
    }
    public virtual void XAction()
    {
        HUD.UsedX();
    }
    public virtual void YAction()
    {
       HUD.UsedY();
    }
    #endregion

    #region Common Events
    public override void TakeDamage(int damageInc, Vector3 dirTemp, int knockback, bool fromAttack, bool stopAttack, PlayerBase attacker)
    {
        if (!iFrames && !trueIFrames)
        {
            ControllerRumble(0.2f, 0.1f);
            universe.CameraRumbleCall(Mathf.Clamp(damageInc * 0.01f, 0.3f, 0.1f));
            hitEffects.SetActive(true);
            if (fromAttack)
            {
                StartCoroutine(HitStun(0.1f));
            }
            HealthChange(Mathf.RoundToInt(-damageInc * incomingMult), attacker);
            if (currentHealth > 0 && !hyperArmour && stopAttack) { anim.SetTrigger("Stagger"); }
            Knockback(knockback, dirTemp);
            universe.PlaySound(ouchSound);
        }
    }
    IEnumerator HitStun(float time)
    {
        Time.timeScale = 0.2f;
        yield return new WaitForSecondsRealtime(time);
        StartCoroutine(EndParticles());
        EndTimeScale();
    }

    IEnumerator EndParticles()
    {
        yield return new WaitForSeconds(0.5f);
        hitEffects.SetActive(false);
    }

    private void EndTimeScale() { Time.timeScale = 1; }

    public void BecomeStunned(float duration)
    {
        state = State.stun;
        Invoke("EndStun", duration);
        anim.SetBool("Stunned", false);
    }
    void EndStun()
    {
        state = State.normal;
    }

    public virtual void OnKill() { }

    public virtual void Death(PlayerBase killer)
    {
        enabled = false;
        dead = true;

        GainIFrames();
        respawnEffects.SetActive(false);

        dir = Vector3.zero;
        rb2d.velocity = Vector3.zero;
        anim.SetFloat("Movement", 0);

        GameObject.Find(thisPlayer + "HUDController").GetComponent<HUDController>().PlayerDeath();
        Time.timeScale = 1;
        anim.SetBool("LockOn", false);
        anim.SetTrigger("Death");
        if (killer != null)
        {
            universe.PlayerDeath(gameObject, killer.gameObject);
            killer.OnKill();
        }
        else
        {
            print("killer is null");
            universe.PlayerDeath(gameObject, null);
            universe.PlaySound(deathSound);
        }
    }
    public virtual void KnockbackContinual()
    {
        transform.position += knockbackForce * knockBackPower * Time.deltaTime;
        //visuals.transform.LookAt(lockTargetList.position + lookAtVariant);
    }
    public override void Knockback(int power, Vector3 direction)
    {
        knockbackForce = direction;
        knockBackPower = power * 10;
        state = State.knockback;
        Invoke("StopKnockback", power * 0.01f);
    }
    public virtual void StopKnockback() { knockbackForce = Vector3.zero; state = State.normal; }
    #endregion

    #region Utility Functions
    public virtual void HealthChange(int healthChange, PlayerBase attacker) { currentHealth += healthChange; if (currentHealth <= 0) { Death(attacker); } }

    public void GainHA() { hyperArmour = true; }
    public void LoseHA() { hyperArmour = false; }

    public void GainIFrames() { iFrames = true; }
    public void GainTrueFrames() { iFrames = true; trueIFrames = true; outline.OutlineColor = Color.yellow; }

    public void LoseIFrames() { iFrames = false; }
    public IEnumerator LoseTrueFrames(float time) { yield return new WaitForSeconds(time); iFrames = false; trueIFrames = false; outline.OutlineColor = Color.black; poison = false; }

    public virtual void Respawn()
    {
        currentHealth = healthMax;
        poison = false;
        gameObject.SetActive(true);
        LoseIFrames();

        aTimer = 0;
        bTimer = 0;
        xTimer = 0;
        yTimer = 0;

        GainTrueFrames();
        StartCoroutine(LoseTrueFrames(2));
        anim.SetTrigger("Respawn");
        damageMult = 1;
        incomingMult = 1;
        rb2d.isKinematic = false;
        dead = false;

        respawnEffects.SetActive(true);

        EndActing();
        anim.SetFloat("Movement", 0);
        //visuals.transform.eulerAngles = Vector3.zero;
    }
    protected IEnumerator PoisonTick()
    {
        yield return new WaitForSeconds(secsBetweenTicks);
        if (poison && !trueIFrames && currentHealth > 1)
        {
            currentHealth -= poisonPerTick;
            ControllerRumble(0.1f, 0.05f);
        }
        StartCoroutine(PoisonTick());
        ExtraUpdate();
        if (currentHealth < 1 && !dead) currentHealth = 1;
    }
    public virtual void ExtraUpdate() { }

    public virtual void BeginActing() { acting = true; rb2d.velocity = Vector3.zero; state = State.attack; }
    public void EndActing()
    {
        acting = false;
        rb2d.velocity = Vector3.zero;
        if (!anim.GetBool("LockOn"))
        {
            state = State.normal;
        }
        else
        {
            state = State.lockedOn;
        }
    }

    public void ControllerRumble(float intensity, float dur)
    {
        player.SetVibration(1, intensity, dur);
        player.SetVibration(0, intensity, dur);
    }

    public virtual void DodgeSliding(Vector3 dir) { transform.position += dir * dodgeSpeed * Time.deltaTime; visuals.transform.LookAt(aimTarget); }

    public virtual void LeaveCrack(Vector3 pos) { ControllerRumble(3, 0.3f); CameraShake(); }

    public virtual void CameraShake() { universe.CameraRumbleCall(0.1f); }
    #endregion

}