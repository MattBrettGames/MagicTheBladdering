using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlamingWiosna : MonoBehaviour
{
    private Transform target;
    private string thisID;
    [SerializeField] float speed;
    [SerializeField] int damage;
    [SerializeField] ParticleSystem particles;
    [SerializeField] float lifeSpan;
    float remainingTime;

    void Update()
    {
        transform.position = Vector3.Lerp(transform.position, target.transform.position, speed);
        transform.LookAt(target);
        remainingTime -= Time.deltaTime;
        if (remainingTime <= lifeSpan)
        {
            gameObject.SetActive(false);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        PlayerBase player = other.GetComponent<PlayerBase>();

        if (player != null && player.playerID != thisID)
        {
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

    public void SetInfo(Transform targetTemp, string id) { target = targetTemp; thisID = id; }
}
