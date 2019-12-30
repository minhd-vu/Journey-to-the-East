using UnityEngine;
using System.Collections;
using System;

public abstract class Damageable : MonoBehaviour
{
    public bool isAlive = true;
    public float damage = 0f;
    private float health;
    [SerializeField] private float maxHealth = 100f;

    public GameObject damageEffect = null;

    [HideInInspector] public float Health
    {
        get
        {
            return health;
        }
        set
        {
            if ((health = value) > maxHealth)
            {
                health = maxHealth;
            }
            else if (health <= 0)
            {
                if (isAlive)
                {
                    isAlive = false;
                    Kill();
                }
            }
        }
    }

    // Return the entity's health in percentages.
    protected float HealthPercent
    {
        get
        {
            return health / maxHealth;
        }
    }

    private void Awake()
    {
        health = maxHealth;
        isAlive = true;
    }

    // Damage the entity.
    public abstract void Damage(float damage);
    // Kill the entity.
    protected abstract void Kill();
}

