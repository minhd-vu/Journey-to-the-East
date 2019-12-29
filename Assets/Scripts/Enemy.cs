using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private Rigidbody2D rb;
    private Animator animator;

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
        Vector3 direction = (GameObject.FindWithTag("Player").transform.position - transform.position).normalized;
        animator.SetFloat("Move X", direction.x);
        animator.SetFloat("Move Y", direction.y);
        animator.SetBool("Moving", !rb.IsSleeping());
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
}
