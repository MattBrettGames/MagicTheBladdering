using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrapplingTrap : MonoBehaviour
{
    bool travelling;
    Vector3 dir;
    [SerializeField] Rigidbody rb2d;
    [SerializeField] float speed;
    [SerializeField] float maxDistance;
    float curDistance;
    Carmen carTrue;
    int damageTrue;
    int ignoredLayer;

    public void OnThrow(Vector3 dirNew, Carmen car, int layer, int damage)
    {
        damageTrue = damage;
        travelling = true;
        rb2d.angularVelocity = Vector3.zero;
        dir = dirNew;
        carTrue = car;
        gameObject.layer = layer;
        Physics.IgnoreLayerCollision(layer, layer);
        transform.forward = dir;
        curDistance = 0;
    }

    void Update()
    {
        if (travelling)
        {
            rb2d.velocity = dir * speed;
        }

        curDistance += Time.deltaTime;
        if (curDistance >= maxDistance)
        {
            End();
        }

    }

    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag != "Hazard")
        {
            travelling = false;
            carTrue.GetLocation(gameObject.transform.position);
            rb2d.velocity = Vector3.zero;

            other.gameObject.GetComponent<PlayerBase>().TakeDamage(damageTrue, Vector3.zero, 0, true, true);
        }
        else
        {
            ignoredLayer = other.gameObject.layer;
            Physics.IgnoreLayerCollision(gameObject.layer, ignoredLayer, true);
        }
    }

    public void End()
    {
        gameObject.SetActive(false);
        transform.position = new Vector3(0, -100, 0);
        rb2d.angularVelocity = Vector3.zero;
        Physics.IgnoreLayerCollision(gameObject.layer, ignoredLayer, false);
    }

}
