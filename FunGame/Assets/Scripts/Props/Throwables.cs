using UnityEngine;

public abstract class Throwables : BlankMono
{
    private void OnCollisionEnter(Collision collision) { CollisionEvent(collision.gameObject); }
    public virtual void CollisionEvent(GameObject collision) { }
}