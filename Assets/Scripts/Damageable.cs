using UnityEngine;
using System.Collections;
using System;

public abstract class Damageable : MonoBehaviour
{
    [HideInInspector] public bool isAlive = true;
    public float damage = 0f;
    private float health;
    public float maxHealth = 100f;

    public GameObject damageEffect = null;

    [HideInInspector]
    public float Health
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

    protected virtual void Start()
    {
        health = maxHealth;
        isAlive = true;
    }

    public void DamageOverTime(float damage, int ticks, float time)
    {
        StartCoroutine(dot(damage, ticks, time));
    }

    private IEnumerator dot(float damage, int ticks, float time)
    {
        for (int i = 0; i < ticks; ++i)
        {
            Damage(damage);
            yield return new WaitForSeconds(time);
        }
    }

    // Damage the entity.
    public abstract void Damage(float damage);
    // Kill the entity.
    protected abstract void Kill();
}

