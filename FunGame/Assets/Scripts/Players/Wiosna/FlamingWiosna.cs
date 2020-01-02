using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlamingWiosna : MonoBehaviour
{
    private Transform target;
    [SerializeField] float speed;
    [SerializeField] int damage;
    ParticleSystem particles;

    void Update()
    {
        transform.position = Vector3.Lerp(transform.position, target.transform.position, speed);
    }

    void OnTriggerEnter(Collider other)
    {
        PlayerBase player = other.GetComponent<PlayerBase>();

        if (player != null)
        {
            player.TakeDamage(damage);
            particles.Play();
            Invoke("Disappear", 0.4f);
        }
    }

    void Disappear()
    {
        gameObject.SetActive(false);
    }
       
    public void SetInfo(Transform targetTemp) { target = targetTemp; }
}
