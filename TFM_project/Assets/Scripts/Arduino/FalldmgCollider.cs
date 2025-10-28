using UnityEngine;

public class FalldmgCollider : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            Debug.Log("Enemy Crushed");
            other.GetComponent<EnemyBehavior>().TakeDamage();
        }
    }
}
