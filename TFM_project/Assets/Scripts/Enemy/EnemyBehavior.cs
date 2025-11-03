using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyBehavior : MonoBehaviour
{
    [Header("Ai Navigation")]
    public UnityEngine.Transform[] waypoints;
    public int curWaypt;
    public NavMeshAgent agent;
    public bool isPatroling;

    [Header("Attack Info")]
    public bool canAttack = true;
    [SerializeField] private int cooldownTime;
    [SerializeField] private int damageAmt;

    [Header("Enemy Stats")]
    [SerializeField] private int enemyHealth = 50;
    [SerializeField] private int fallDamageAmt;
    public static float speed = 20f;

    [Header("Animations")]
    private Animator animator;

    public void Start()
    {
        isPatroling = true;
        animator = GetComponent<Animator>();

        //idle at start
        animator.SetFloat("Speed", 0f);
    }

    public void Update()
    {
        if (isPatroling)
        {
            curWaypt = Random.Range(0, waypoints.Length-1);
            agent.SetDestination(waypoints[curWaypt].transform.position);
        }
    }
    public void LockedOnTarget(GameObject player)
    {
        agent.SetDestination(player.transform.position);

        //update animation based on velocity
        float speedPercent = agent.velocity.magnitude / agent.speed;
        animator.SetFloat("Speed", speedPercent);
    }
    public void Attacked()
    {
        canAttack = false;
        animator.SetTrigger("Attack");
        StartCoroutine("Cooldown");
    }
    IEnumerator Cooldown()
    {
        yield return new WaitForSeconds(cooldownTime);
        canAttack = true;
        Debug.Log("cooldown done");
    }
    public void TakeDamage()
    {
        if (enemyHealth <= 0)
        {
            Debug.Log("Dead");
            Destroy(gameObject);
        }
        enemyHealth -= fallDamageAmt;
    }

}
