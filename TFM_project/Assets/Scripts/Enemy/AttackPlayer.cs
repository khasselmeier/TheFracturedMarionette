using System.Runtime.Remoting.Messaging;
using Unity.VisualScripting;
using UnityEngine;

public class AttackPlayer : MonoBehaviour
{
    PlayerManager player;
    EnemyBehavior enemy;
    public void Start()
    {
        enemy = GetComponent<EnemyBehavior>();
        player = GetComponent<PlayerManager>();
    }


    public void OnTriggerEnter(Collider other)
    {
            //enemy.isfollowplayer = false;
            Debug.Log("attacking player");
        if (other.gameObject.GetComponent<PlayerManager>() != null && enemy.canAttack)
        {
            enemy.isPatroling = false;
            player.TakeHit(GetRandomLimb());
            enemy.Attacked();
        }
    }
    public int GetRandomLimb()
    {
        int limbID = Random.Range(0, 3);
        return limbID;
    }
}
