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

    [Header("Attacking")]
    public bool canAttack = true;
    [SerializeField] private int cooldownTime;
    [SerializeField] private int damageAmt;

    [Header("Enemy Stats")]
    [SerializeField] private int enemyHealth = 50;
    [SerializeField] private int fallDamageAmt;
    public static float speed = 20f;

    public void Start()
    {
        isPatroling = true;
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
    }
    public void Attacked()
    {
        canAttack = false;
        StartCoroutine("Cooldown");
    }
    IEnumerator Cooldown()
    {
        yield return new WaitForSeconds(cooldownTime);
        canAttack = true;
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
