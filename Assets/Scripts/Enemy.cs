using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class Enemy : Damageable
{
    protected Rigidbody2D rb;
    protected Animator animator;
    protected Vector3 direction;
    [SerializeField] private GameObject healthBar = null;
    protected float attackTimer;


    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        healthBar = Instantiate(healthBar, transform);
        attackTimer = 0f;
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        // Set the direction the enemy is moving in.
        if (isAlive)
        {
            GameObject player = GameObject.FindWithTag("Player");
            direction = (player.transform.position - transform.position).normalized;
            animator.SetFloat("Move X", direction.x);
            animator.SetFloat("Move Y", direction.y);
            animator.SetBool("Moving", !rb.IsSleeping());
        }
    }

    protected virtual void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.name.Equals("Player"))
        {
            Attack(collision.gameObject.GetComponent<Damageable>());
        }
    }

    protected virtual void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.name.Equals("Player"))
        {
            Attack(collision.gameObject.GetComponent<Damageable>());
        }
    }

    protected virtual void Attack(Damageable d)
    {
        if ((attackTimer -= Time.deltaTime) <= 0f)
        {
            animator.SetTrigger("Attack");
            attackTimer = animator.GetCurrentAnimatorStateInfo(0).length;
            d.Damage(damage);
        }
    }

    public override void Damage(float damage)
    {
        if (isAlive)
        {
            // Damage the enemy.
            Health -= damage;

            // Instantiate the damage effects appropriately.
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            GameObject de = Instantiate(damageEffect, new Vector3(transform.position.x, transform.position.y - 0.0001f), Quaternion.identity);
            de.transform.GetChild(0).rotation = Quaternion.AngleAxis(angle - 180f, Vector3.forward);
            healthBar.GetComponent<HealthBar>().Percent = HealthPercent;
        }
    }

    protected override void Kill()
    {
        animator.SetTrigger("Death");
        GetComponent<AIPath>().canMove = false;

        foreach (Collider2D collider in GetComponents<Collider2D>())
        {
            collider.enabled = false;
        }

        Destroy(healthBar);
        Destroy(gameObject, animator.GetCurrentAnimatorStateInfo(0).length);
    }
}
