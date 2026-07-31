using UnityEngine;

public class MonsterAI : MonoBehaviour
{
    [Header("References")]
    public Transform player;
    public Animator monsterAnimator;

    [Header("Movement")]
    public float moveSpeed = 2f;

    [Header("Attack")]
    public float attackRange = 1.2f;
    public int attackDamage = 1;
    public float attackCooldown = 1.5f;

    private float attackTimer;

    void Start()
    {
        if (monsterAnimator == null)
            monsterAnimator = GetComponent<Animator>();
    }

    void Update()
    {
        if (player == null) return;

        float distance = Vector3.Distance(transform.position, player.position);

        if (distance > attackRange)
        {
            MoveTowardsPlayer();
            SetWalking(true);
        }
        else
        {
            SetWalking(false);
            AttackPlayer();
        }

        attackTimer += Time.deltaTime;
    }

    void MoveTowardsPlayer()
    {
        Vector3 direction = (player.position - transform.position).normalized;
        direction.y = 0f;

        transform.position += direction * moveSpeed * Time.deltaTime;
        transform.rotation = Quaternion.LookRotation(direction);
    }

    void SetWalking(bool walking)
    {
        if (monsterAnimator != null)
            monsterAnimator.SetBool("IsWalking", walking);
    }

    void AttackPlayer()
    {
        if (attackTimer >= attackCooldown)
        {
            PlayerHealth playerHealth = player.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(attackDamage);
            }

            attackTimer = 0f;
        }
    }
}