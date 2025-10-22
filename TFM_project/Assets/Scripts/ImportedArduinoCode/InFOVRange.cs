using UnityEngine;

public class InFOVRange : MonoBehaviour
{
    public EnemyBehavior enemy;

    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            enemy.isPatroling = false;
            Debug.Log("LOCKED ON TARGET");
            enemy.LockedOnTarget(other.gameObject);
        }
    }
    public void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            enemy.isPatroling = true;
        }
    }
}
