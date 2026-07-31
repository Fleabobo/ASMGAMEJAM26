using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Projectile : MonoBehaviour
{
    private float damage;
    private float lifetime;

    public void Init(float damage, float speed, float lifetime)
    {
        this.damage = damage;
        this.lifetime = lifetime;

        Rigidbody rb = GetComponent<Rigidbody>();
        rb.linearVelocity = transform.forward * speed; // Unity 6 renamed "velocity" to "linearVelocity"

        Destroy(gameObject, lifetime); // auto-cleanup if it never hits anything
    }

    void OnCollisionEnter(Collision collision)
    {
        MonsterHealth monster = collision.collider.GetComponent<MonsterHealth>();
        if (monster != null)
        {
            monster.TakeDamage(damage);
        }

        Destroy(gameObject);
    }
}