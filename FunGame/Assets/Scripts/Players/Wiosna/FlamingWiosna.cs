using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlamingWiosna : MonoBehaviour
{
    private Transform target;
    private string thisID;
    [SerializeField] float speed;
    [SerializeField] float lookSpeed;
    [SerializeField] int damage;
    [SerializeField] ParticleSystem particles;
    [SerializeField] float lifeSpan;
    float remainingTime;
    GameObject looker;

    void Update()
    {
        looker.transform.LookAt(target);
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
        PlayerBase player = other.GetComponent<PlayerBase>();

        if (player != null && player.thisPlayer != thisID)
        {
            print(player.thisPlayer);
            player.TakeDamage(damage);
            particles.Play();
            Invoke("Disappear", 0.4f);
        }
    }

    void Disappear()
    {
        gameObject.SetActive(false);
        particles.Stop();
        particles.Clear();
    }

    public void AwakenClone() { remainingTime = lifeSpan; }

    public void SetInfo(Transform targetTemp, string id)
    {
        target = targetTemp;
        thisID = id;
        looker = new GameObject("FlamingCloneLooker");
    }
}
