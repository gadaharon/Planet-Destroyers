using System;
using UnityEngine;

public class Health : MonoBehaviour
{
    public Action OnDeath;
    public int MaxHealth => maxHealth;
    public int CurrentHealth => currentHealth;

    [SerializeField] int maxHealth = 30;
    [SerializeField] int currentHealth;

    void Awake()
    {
        currentHealth = maxHealth;
    }

    public void ResetHealth()
    {
        currentHealth = maxHealth;
    }

    public void AddHealth(int amount)
    {
        currentHealth += amount;
        if (currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }
    }

    public void TakeDamage(int amount)
    {
        currentHealth -= amount;

        if (currentHealth <= 0)
        {
            currentHealth = 0;
            // Die
            OnDeath?.Invoke();
            // Debug.Log($"{gameObject.name} - Has Died");
        }
    }
}
