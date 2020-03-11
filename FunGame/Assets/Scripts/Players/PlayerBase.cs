using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Rewired;
using System;

public enum AttackType
{
    A, B, X, Y
}

public abstract class PlayerBase : ThingThatCanDie
{

    public bool isAI;

    [Header("GameMode Stuff")]
    public string thisPlayer;
    public int playerID;
    [HideInInspector] public int numOfDeaths = 0;
    [HideInInspector] public bool pvp = true;

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
    [SerializeField] protected int poisonPerTick;
    [SerializeField] protected float secsBetweenTicks;
    [SerializeField] public bool poison;
    private bool hyperArmour;
    [HideInInspector] public bool iFrames;
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
    [SerializeField] GameObject invincibleEffect;
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
    protected bool dead;
    protected Vector3 lastDir;

    [Header("Cooldowns")]
    public float aCooldown;
    public float bCooldown;
    public float xCooldown;
    public float yCooldown;
    [HideInInspector] public float aTimer;
    [HideInInspector] public float bTimer;
    [HideInInspector] public float xTimer;
    [HideInInspector] public float yTimer;

    [Header("Sounds")]
    [SerializeField] protected AudioClip aSound;
    [SerializeField] protected AudioClip bSound;
    [SerializeField] protected AudioClip xSound;
    [SerializeField] protected AudioClip ySound;
    [SerializeField] protected AudioClip[] ouchSounds = new AudioClip[0];
    [SerializeField] protected AudioClip[] deathSounds = new AudioClip[0];
    [SerializeField] protected AudioClip victorySound;
    AudioSource audioSource;

    public void OnSelected()
    {
        aiAgent = GetComponent<NavMeshAgent>();
        healthMax = currentHealth;
        audioSource = gameObject.AddComponent<AudioSource>();
    }

    public void TeleportPlayer(Vector3 newPos)
    {
        aiAgent.Warp(newPos);
    }

    public virtual void Start()
    {
        outline = visuals.GetComponent<Outline>();
        anim = gameObject.GetComponentInChildren<Animator>();
        rb2d = gameObject.GetComponent<Rigidbody>();
        bTimer = bCooldown;

        healthMax = currentHealth;
        player = ReInput.players.GetPlayer(playerID);
        walkDirection = new GameObject("WalkDirection").transform;
        HUD = GameObject.Find(thisPlayer + "HUDController").GetComponent<HUDController>();
    }

    public virtual void SetInfo(UniverseController uni, int layerNew)
    {
        universe = uni;
        gameObject.layer = layerNew;

        RegainTargets();
        if (isAI) currentPlayerTarget = lockTargetList[currentLock].GetComponentInParent<PlayerBase>();

        aiLooker = new GameObject("AILooker").transform;

        if (isAI)
        {
            aiAgent.stoppingDistance = 0;
            aiAgent.angularSpeed = Mathf.Infinity;
            aiAgent.acceleration = speed;
        }
        else aiAgent.enabled = false;

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
                        if (player.GetButtonDown("BAttack") || Input.GetKeyDown(KeyCode.B)) { BAction(); }
                        if (player.GetButtonDown("XAttack")) { XAction(); }
                        if (player.GetButtonDown("YAttack")) { YAction(); }

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
                        if (player.GetButtonDown("XAttack")) { XAction(); }
                        if (player.GetButtonDown("YAttack")) { YAction(); }

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
        yield return new WaitForSecondsRealtime(0.2f);
        onCooldown = false;
    }

    #region Input Actions
    public virtual void AAction(bool playAnim)
    {
        if (aTimer <= 0 && dir != Vector3.zero)
        {
            HUD.UsedA();

            if (playAnim)
                anim.SetTrigger("AAction");

            state = State.dodging;

            Invoke("EndDodge", dodgeDur);

            PlaySound(aSound);
        }
    }
    public virtual void EndDodge()
    {
        state = State.normal;
        aTimer = aCooldown;
        TeleportPlayer(transform.position);
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


    #region SoundControl
    public void PlaySound(AudioClip clipToPlay)
    {
        if (audioSource.clip != clipToPlay)
        {
            audioSource.volume = OptionMenuController.masterVolume * OptionMenuController.sfxVolume;

            audioSource.Stop();
            audioSource.clip = clipToPlay;
            audioSource.Play();
        }
    }
    public void PlaySound(AudioClip[] clipsToPlay)
    {
        int rando = UnityEngine.Random.Range(0, clipsToPlay.Length);

        if (audioSource.clip != clipsToPlay[rando])
        {
            audioSource.volume = OptionMenuController.masterVolume * OptionMenuController.sfxVolume;

            audioSource.Stop();
            audioSource.clip = clipsToPlay[rando];
            audioSource.Play();
        }
    }
    public void PlayVictorySound()
    {
        PlaySound(victorySound);
    }
    #endregion


    #region Common Events
    public override void TakeDamage(int damageInc, Vector3 dirTemp, int knockback, bool fromAttack, bool stopAttack, PlayerBase attacker, float knockbackDur)
    {
        if (!iFrames && !trueIFrames)
        {
            universe.CameraRumbleCall(Mathf.Clamp(damageInc * 0.01f, 0.3f, 0.1f));
            hitEffects.SetActive(true);
            if (fromAttack)
            {
                StartCoroutine(HitStop(0.1f));
            }
            HealthChange(Mathf.RoundToInt(-damageInc * incomingMult), attacker);
            if (currentHealth > 0 && !hyperArmour && stopAttack) { anim.SetTrigger("Stagger"); }
            Knockback(knockback, dirTemp, knockbackDur);
            PlaySound(ouchSounds);
        }
        if (iFrames || trueIFrames)
        {
            invincibleEffect.SetActive(false);
            invincibleEffect.SetActive(true);
        }
    }
    IEnumerator HitStop(float time)
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

    public virtual void OnVictory()
    {
        PlayVictorySound();
        anim.SetFloat("Movement", 0);
        anim.SetBool("LockOn", false);
    }


    public virtual void Death(PlayerBase killer)
    {
        anim.ResetTrigger("Respawn");
        anim.SetTrigger("Death");
        enabled = false;
        dead = true;

        GainTrueFrames();
        respawnEffects.SetActive(false);
        outline.OutlineColor = Color.black;

        dir = Vector3.zero;

        aiAgent.isStopped = true;

        rb2d.velocity = Vector3.zero;
        anim.SetFloat("Movement", 0);
        anim.SetBool("LockOn", false);

        GameObject.Find(thisPlayer + "HUDController").GetComponent<HUDController>().PlayerDeath();
        Time.timeScale = 1;
        if (killer != null)
        {
            universe.PlayerDeath(gameObject, killer.gameObject);
            killer.OnKill();
        }
        else
        {
            universe.PlayerDeath(gameObject, null);
        }
        PlaySound(deathSounds);
    }

    #region Knockback Controls
    public virtual void KnockbackContinual()
    {
        transform.position += knockbackForce * knockBackPower * Time.deltaTime;
    }
    public override void Knockback(int power, Vector3 direction, float knockbackDur)
    {
        knockbackForce = direction;
        knockBackPower = power * 10;
        state = State.knockback;
        Invoke("StopKnockback", knockbackDur);
    }
    public virtual void StopKnockback()
    {
        knockbackForce = Vector3.zero;
        state = State.normal;
        TeleportPlayer(transform.position);
    }
    #endregion

    #endregion

    #region Utility Functions
    public virtual void HealthChange(int healthChange, PlayerBase attacker) { currentHealth += healthChange; if (currentHealth <= 0) { Death(attacker); } }

    public virtual void OnHit(PlayerBase target, AttackType hitWith) { }
    public virtual void GainHA() { hyperArmour = true; }
    public virtual void LoseHA() { hyperArmour = false; }

    public void GainIFrames() { iFrames = true; }
    public void GainTrueFrames() { iFrames = true; trueIFrames = true; outline.OutlineColor = Color.yellow; }

    public void LoseIFrames()
    {
        iFrames = false;
    }
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

        aiAgent.isStopped = false;

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
    }

    public virtual IEnumerator PoisonTick()
    {
        yield return new WaitForSeconds(secsBetweenTicks);
        if (currentHealth < 1 && !dead) currentHealth = 1;
        StartCoroutine(PoisonTick());
        ExtraUpdate();

        if (poison && !trueIFrames && currentHealth > 1)
        {
            currentHealth -= poisonPerTick;
            //ControllerRumble(0.1f, 0.05f, false, null);
        }
    }
    public virtual void ExtraUpdate() { }

    public virtual void BeginActing()
    {
        acting = true;
        rb2d.velocity = Vector3.zero;
        state = State.attack;
        ResetTriggers();
    }

    private void ResetTriggers()
    {
        anim.ResetTrigger("XAttack");
        anim.ResetTrigger("YAttack");
        anim.ResetTrigger("AAction");
        anim.ResetTrigger("BAttack");
    }

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

    public virtual void ControllerRumble(float intensity, float dur, bool isSkjegg, PlayerBase hitTarget)
    {
        player.SetVibration(1, intensity, dur);
        player.SetVibration(0, intensity, dur);
    }

    public virtual void DodgeSliding(Vector3 dir)
    {
        if (isAI)
        {
            aiAgent.Move(visuals.transform.forward * dodgeSpeed * Time.deltaTime);
        }
        else
        {
            transform.position += dir * dodgeSpeed * Time.deltaTime;
            visuals.transform.LookAt(aimTarget);
        }
    }

    public virtual void LeaveCrack(Vector3 pos) { ControllerRumble(3, 0.3f, false, null); CameraShake(); }

    public virtual void CameraShake() { universe.CameraRumbleCall(0.1f); }
    #endregion


    #region AI Controls

    //[Header("AI Components")]
    NavMeshAgent aiAgent;
    float detectionDistance = 15;
    Transform aiLooker;
    bool hasGainedFleeTarget;
    PlayerBase currentPlayerTarget;
    AIState logicState;
    public enum AIState
    {
        idle,
        fleeing,
        aggresive
    }


    public void AIUpdate()
    {
        dir = visuals.transform.forward;
        AILogic();


        if (aTimer > 0) aTimer -= Time.deltaTime;
        if (bTimer > 0) bTimer -= Time.deltaTime;
        if (xTimer > 0) xTimer -= Time.deltaTime;
        if (yTimer > 0) yTimer -= Time.deltaTime;

        if (anim.GetCurrentAnimatorStateInfo(0).IsName("Idle") || anim.GetCurrentAnimatorStateInfo(0).IsName("Walking")) acting = false;

        switch (state)
        {
            case State.stun:
                anim.SetBool("Stunned", true);
                break;

            case State.attack:
                break;

            case State.normal:

                anim.SetBool("LockOn", false);

                if (!acting)
                {
                    anim.SetFloat("Movement", dir.magnitude + 0.001f);
                }
                else
                {
                    dir = Vector3.zero;
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

    public virtual void AILogic()
    {
        float distanceToTarget = Vector3.Distance(transform.position, lockTargetList[currentLock].position);

        aiAgent.speed = speed + bonusSpeed;

        switch (logicState)
        {

            case AIState.idle:
                anim.SetFloat("Movement", 0);

                if (distanceToTarget <= detectionDistance * 3)
                    logicState = AIState.fleeing;

                else
                    logicState = AIState.aggresive;

                break;


            case AIState.fleeing:
                anim.SetFloat("Movement", 1);
                if (!hasGainedFleeTarget)
                    aiAgent.SetDestination(transform.position + new Vector3(UnityEngine.Random.Range(-detectionDistance, detectionDistance), 0, UnityEngine.Random.Range(-detectionDistance, detectionDistance)) * 3);
                hasGainedFleeTarget = true;

                if (Vector3.Distance(transform.position, aiAgent.destination) >= 3)
                    logicState = AIState.idle;


                if (currentPlayerTarget.acting)
                {
                    AAction(false);
                    logicState = AIState.aggresive;
                }

                if (currentHealth >= currentPlayerTarget.currentHealth) logicState = AIState.aggresive;

                break;


            case AIState.aggresive:
                anim.SetFloat("Movement", 1);
                hasGainedFleeTarget = false;

                aiLooker.transform.position = transform.position;
                aiLooker.transform.LookAt(lockTargetList[currentLock].transform.position + lookAtVariant);
                visuals.transform.forward = Vector3.Lerp(visuals.transform.forward, aiLooker.transform.forward, lockOnLerpSpeed);

                aiAgent.SetDestination(new Vector3(lockTargetList[currentLock].position.x, 0, lockTargetList[currentLock].position.z));
                AttackLogic(distanceToTarget);

                if (currentHealth <= currentPlayerTarget.currentHealth) logicState = AIState.fleeing;

                break;

        }
        AttackLogic(distanceToTarget);
    }

    void AttackLogic(float distanceToTarget)
    {
        if (distanceToTarget <= detectionDistance)
        {
            XAction();
            logicState = AIState.idle;
        }
    }


    #endregion

}