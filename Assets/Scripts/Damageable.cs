using UnityEngine;
using System.Collections;

public abstract class Damageable : MonoBehaviour
{
    public float damage = 0f;
    private float health;
    [SerializeField] private float maxHealth = 100f;
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
                Kill();
            }
        }
    }
    protected float healthPercent
    {
        get
        {
            return health / maxHealth;
        }
    }

    private void Awake()
    {
        health = maxHealth;
    }

    // Damage the entity.
    public abstract void Damage(float damage);
    // Kill the entity.
    protected abstract void Kill();
}

