using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlamingWiosna : ThingThatCanDie
{
    [SerializeField] float speed;
    [SerializeField] float lookSpeed;
    [SerializeField] float lifeSpan;
    [SerializeField] Material mat0;
    [SerializeField] Material mat1;
    [SerializeField] Material mat2;
    [SerializeField] Material mat3;
    private Transform target;
    private string thisID;
    int damage;
    Material activeMaterial;
    float remainingTime;
    GameObject looker;
    SkinnedMeshRenderer[] meshRenderers = new SkinnedMeshRenderer[7];
    GameObject cloneBurst;
    PlayerBase owner;

    void Update()
    {
        looker.transform.LookAt(target.position - new Vector3(0, 5, 0));
        looker.transform.position = transform.position;

        transform.forward = Vector3.Lerp(transform.forward, looker.transform.forward, lookSpeed);

        transform.position += transform.forward * speed;

        remainingTime -= Time.deltaTime;
        if (remainingTime <= 0)
        {
            gameObject.SetActive(false);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        ThingThatCanDie player = other.gameObject.GetComponent<ThingThatCanDie>();

        if (player != null && player.tag != tag)
        {
            player.TakeDamage(damage, Vector3.zero, 0, true, true, owner);
            cloneBurst.transform.position = transform.position;
            cloneBurst.SetActive(true);
            Disappear();
        }
    }

    public override void TakeDamage(int damageInc, Vector3 dirTemp, int knockback, bool fromAttack, bool stopAttack, PlayerBase attacker)
    {
        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            Disappear();
        }
    }

    void Disappear()
    {
        gameObject.SetActive(false);
    }

    public void AwakenClone(Transform targetTemp)
    {
        remainingTime = lifeSpan;
        cloneBurst.SetActive(false);
        target = targetTemp;
    }

    public void SetInfo(string id, int damageTemp, Color cloneColour, string newTag, GameObject cloneExplosion, PlayerBase ownerTemp)
    {
        meshRenderers = GetComponentsInChildren<SkinnedMeshRenderer>();
        thisID = id;
        damage = damageTemp;
        gameObject.tag = newTag;
        cloneBurst = cloneExplosion;
        owner = ownerTemp;

        if (id == "P1") { activeMaterial = mat0; }
        else if(id == "P2"){ activeMaterial = mat1; }
        else if (id == "P3") { activeMaterial = mat2; }
        else { activeMaterial = mat3; }

        looker = new GameObject("FlamingCloneLooker");


        for (int i = 0; i < meshRenderers.Length; i++)
        {
            meshRenderers[i].material = activeMaterial;
        }

        ParticleSystem[] parts = cloneExplosion.GetComponentsInChildren<ParticleSystem>();

        for (int i = 0; i < parts.Length; i++)
        {
            parts[i].startColor = cloneColour;
        }

        activeMaterial.SetColor("_EmissionColor", cloneColour * 3);
        activeMaterial.SetColor("_Color", cloneColour * 3);

        currentHealth = healthMax;
    }
}
