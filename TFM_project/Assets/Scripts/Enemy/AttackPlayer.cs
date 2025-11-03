using System.Runtime.Remoting.Messaging;
using Unity.VisualScripting;
using UnityEngine;

public class AttackPlayer : MonoBehaviour
{
    [SerializeField]
    private PlayerStatusManager player;
    [SerializeField]
    EnemyBehavior enemy;
    bool isInRange;


    public void Update()
    {
        if (isInRange)
        {
            Attack();
        }
    }
    public void OnTriggerEnter(Collider other)
    {
        //enemy.isfollowplayer = false;
        Debug.Log("attacking player");
        if(other.gameObject.CompareTag("Player")) isInRange = true;
    }

    public void OnCollisionExit(Collision collision)
    {
        isInRange = false;
    }
    public void Attack()
    {
        if (enemy.canAttack)
        {
            enemy.isPatroling = false;
            player.TakeHit(GetRandomLimb());
            enemy.Attacked();
        }
    }
    public int GetRandomLimb()
    {
        int limbID = Random.Range(0, 4);
        Debug.Log(limbID);
        return limbID;
    }
}
