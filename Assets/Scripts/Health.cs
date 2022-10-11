using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


public class Health : MonoBehaviour
{
    [SerializeField] float health;
    [SerializeField] float maxHealth;

    public UnityEvent<float> OnHealthChange;

    private void Start()
    {
        health = maxHealth;
    }

    public float GetMaxHealth()
    {
        return maxHealth;
    }

    public void ReduceHealth(int damage)
    {
        health-= damage;
        OnHealthChange?.Invoke(health/maxHealth);
    }

    public void AddHealth(int healthAmount)
    {
        health += healthAmount;

        if (health > maxHealth) health = maxHealth;

        OnHealthChange?.Invoke(health / maxHealth);
    }

    public float GetCurrentHealth()
    {
        return health;
    }
}
