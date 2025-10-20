using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(Animator))]
public class EnemyMovement : MonoBehaviour
{
    [Header("References")]
    public Transform player;

    [Header("Settings")]
    public float detectionRange = 2f;
    public float attackRange = 1f;
    public float attackCooldown = 2f;
    public float attackDamage = 1f;

    private NavMeshAgent agent;
    private Animator animator;
    private bool isChasing = false;
    private bool isAttacking = false;
    private float lastAttackTime = 0f;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();

        //idle at start
        animator.SetFloat("Speed", 0f);
    }

    private void Update()
    {
        if (player == null) return;

        float distance = Vector3.Distance(transform.position, player.position);

        //start chasing once the player passes by (within detection range)
        if (!isChasing && distance <= detectionRange)
        {
            isChasing = true;
        }

        //if chasing, move toward player
        if (isChasing && !isAttacking)
        {
            agent.SetDestination(player.position);

            //update animation based on velocity
            float speedPercent = agent.velocity.magnitude / agent.speed;
            animator.SetFloat("Speed", speedPercent);
        }

        //attack if close enough
        if (isChasing && distance <= attackRange && Time.time - lastAttackTime > attackCooldown)
        {
            StartCoroutine(AttackPlayer());
            isAttacking = true;
        }
    }

    private System.Collections.IEnumerator AttackPlayer()
    {
        agent.isStopped = true;
        animator.SetTrigger("Attack");

        //attack delay to sync with animation
        yield return new WaitForSeconds(1f);

        lastAttackTime = Time.time;
        agent.isStopped = false;
        isAttacking = false;
        PlayerStatus.Instance.TakeDamage(attackDamage);
    }

    /*
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            //stop moving and attack immediately on collision
            if (!isAttacking)
                StartCoroutine(AttackPlayer());
        }
    }
    */

    //visualize detection range in editor
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, detectionRange);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}
