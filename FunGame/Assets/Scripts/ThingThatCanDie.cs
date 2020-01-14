using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThingThatCanDie : MonoBehaviour
{
    [Header("Sutff to do with Dying")]
    public int currentHealth;
    [HideInInspector] public int healthMax;


    public virtual void TakeDamage(int damage, Vector3 dirTemp, bool fromAttack, bool stopAttack) { }
    public virtual void Knockback(int power, Vector3 direction) { }

}
