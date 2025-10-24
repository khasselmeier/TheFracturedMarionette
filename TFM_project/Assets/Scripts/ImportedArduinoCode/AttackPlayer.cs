/*using Unity.VisualScripting;
using UnityEngine;

public class AttackPlayer : MonoBehaviour
{
    PlayerManager player;
    EnemyBehavior enemy;
    public int damageAmt;
    public void Start()
    {
        enemy = GetComponent<EnemyBehavior>();
        player = GetComponent<PlayerManager>();
    }


    public void OnTriggerEnter(Collider other)
    {
            //enemy.isfollowplayer = false;
            Debug.Log("attacking player");
        if (other.gameObject.GetComponent<GyroArm>() != null && enemy.canAttack)
        {
            enemy.isPatroling = false;
            if (other.gameObject.CompareTag("LeftArm"))
            {
                Debug.Log("Damage left arm");
                player.TakeHit(0, damageAmt);
                Debug.Log("Attack left arm");
                enemy.Attacked();
            }
            else if (other.gameObject.CompareTag("RightArm"))
            {
                Debug.Log("Damage right arm");
                player.TakeHit(1, damageAmt);
                enemy.Attacked();
            }
            
        }
        else if (other.gameObject.GetComponent<GyroLeg>() != null && enemy.canAttack)
        {
            if (other.gameObject.CompareTag("LeftLeg"))
            {
                Debug.Log("Damage left leg");
                player.TakeHit(2, damageAmt);
                enemy.Attacked();
            }
            else if (other.gameObject.CompareTag("RightLeg"))
            {
                Debug.Log("Damage right leg");
                player.TakeHit(3, damageAmt);
                enemy.Attacked();
            }
        }
    }
}
*/