using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillHazard : MonoBehaviour
{

    public int damageToEnemy;
    public int damageToPlayer;

    void OnCollisionEnter(Collision other)
    {
        if (other.transform.tag != "Zone")
        {
            if (other.gameObject.GetComponent<PlayerBase>() != null)
            {
                other.gameObject.GetComponent<PlayerBase>().TakeDamage(damageToPlayer);
            }
            else if (other.gameObject.GetComponent<EnemyBase>() != null)
            {
                other.gameObject.GetComponent<EnemyBase>().TakeDamage(damageToEnemy, "trap");
            }
        }
    }


}
