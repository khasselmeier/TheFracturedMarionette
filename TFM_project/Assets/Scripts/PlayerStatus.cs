using UnityEngine;
using System.Collections.Generic;

public class PlayerStatus : MonoBehaviour
{
    public static PlayerStatus Instance;

    [Header("Health Stats")]
    public float maxHealth = 100f;
    public float currentHealth;
    public float damage = 10f;

    [Header("Status Effects")] //Broken Bones and stuff
    public List<string> conditions = new List<string>(); //There is prob a better way to handle this, just a list for rn

    void Awake()
    {
        if(Instance == null) // Keep object between scenes
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            currentHealth = maxHealth;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        Debug.Log("Damage Taken, Health = " + currentHealth);

        if (currentHealth < 0) 
        {
            Debug.Log("Dead");
        }
    }

    public void AddCondition(string condition)
    {
        if(!conditions.Contains(condition))
        {
            conditions.Add(condition);
            Debug.Log("Condition added- " + condition);
        }
    }

    public void RemoveCondition(string condition)
    {
        if (conditions.Contains(condition))
        {
            conditions.Remove(condition);
            Debug.Log("Condition Removed-" + condition);
        }
    }

    public bool HasCondition(string condition)
    {
        return conditions.Contains(condition);
    }
}
