using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class Enemy : Damageable
{
    private Rigidbody2D rb;
    private Animator animator;
    private Vector3 direction;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        // Set the direction the enemy is moving in.
        if (isAlive)
        {
            direction = (GameObject.FindWithTag("Player").transform.position - transform.position).normalized;
            animator.SetFloat("Move X", direction.x);
            animator.SetFloat("Move Y", direction.y);
            animator.SetBool("Moving", !rb.IsSleeping());
        }
    }
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.name.Equals("Player"))
        {
            Attack();
        }
    }

    void Attack()
    {
        animator.SetTrigger("Attack");
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

        Destroy(gameObject, animator.GetCurrentAnimatorStateInfo(0).length);
    }
}
