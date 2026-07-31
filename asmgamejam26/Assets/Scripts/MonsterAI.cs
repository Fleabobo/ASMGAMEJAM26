using UnityEngine;

public class MonsterAI : MonoBehaviour
{
    [Header("References")]
    public Transform player;            // drag Player object here
    public Animator monsterAnimator;    // optional, for walk/attack animations

    [Header("Movement")]
    public float moveSpeed = 2f;

    [Header("Attack")]
    public float attackRange = 1.2f;    // how close it needs to be to hit player
    public float attackDamage = 1f;
    public float attackCooldown = 1.5f; // seconds between hits

    private float attackTimer;

    void Update()
    {
        if (player == null) return;

        float distance = Vector3.Distance(transform.position, player.position);

        if (distance > attackRange)
        {
            MoveTowardsPlayer();
        }
        else
        {
            AttackPlayer();
        }

        attackTimer += Time.deltaTime;
    }

    void MoveTowardsPlayer()
    {
        Vector3 direction = (player.position - transform.position).normalized;
        direction.y = 0f; // keep monster on the ground, don't tilt upward

        transform.position += direction * moveSpeed * Time.deltaTime;
        transform.rotation = Quaternion.LookRotation(direction);

        // Optional: trigger walk animation if you have one
        // monsterAnimator?.SetBool("IsWalking", true);
    }

    void AttackPlayer()
    {
        // Optional: trigger walk-stop
        // monsterAnimator?.SetBool("IsWalking", false);

        if (attackTimer >= attackCooldown)
        {
            PlayerHealth playerHealth = player.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(attackDamage);
            }

            // Optional: trigger attack animation
            // monsterAnimator?.SetTrigger("Attack");

            attackTimer = 0f; // reset cooldown
        }
    }
}