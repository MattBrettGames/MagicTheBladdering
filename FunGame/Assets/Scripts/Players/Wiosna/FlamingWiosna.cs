using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlamingWiosna : ThingThatCanDie
{
    private Transform target;
    [SerializeField] private string thisID;
    [SerializeField] float speed;
    [SerializeField] float lookSpeed;
    int damage;
    [SerializeField] ParticleSystem particles;
    [SerializeField] float lifeSpan;
    Material activeMaterial;
    [SerializeField] Material mat0;
    [SerializeField] Material mat1;
    float remainingTime;
    GameObject looker;
    SkinnedMeshRenderer[] meshRenderers = new SkinnedMeshRenderer[7];

    void Update()
    {
        looker.transform.LookAt(target.position - new Vector3(0,5,0));
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
            player.TakeDamage(damage, Vector3.zero, 0, true, true);
            Disappear();
            particles.Play();
        }
    }

    public override void TakeDamage(int damageInc, Vector3 dirTemp, int knockback, bool fromAttack, bool stopAttack)
    {
        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            Disappear();
        }
    }

    void Disappear()
    {
        particles.Stop();
        particles.Clear();
        gameObject.SetActive(false);
    }

    public void AwakenClone()
    {
        remainingTime = lifeSpan;
    }

    public void SetInfo(Transform targetTemp, string id, int damageTemp, Color cloneColour, string newTag)
    {
        meshRenderers = GetComponentsInChildren<SkinnedMeshRenderer>();
        target = targetTemp;
        thisID = id;
        damage = damageTemp;
        tag = newTag;

        if (id == "P1") { activeMaterial = mat0; }
        else { activeMaterial = mat1; }

        looker = new GameObject("FlamingCloneLooker");


        for (int i = 0; i < meshRenderers.Length; i++)
        {
            meshRenderers[i].material = activeMaterial;
        }

        activeMaterial.SetColor("_EmissionColor", cloneColour * 4);
        activeMaterial.SetColor("_Color", cloneColour * 4);

        currentHealth = healthMax;
    }
}
