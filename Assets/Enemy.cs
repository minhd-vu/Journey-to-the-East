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
        float angle = Mathf.Atan2(rb.velocity.y, rb.velocity.x) * Mathf.Rad2Deg;
        animator.SetBool("Moving Left", (angle >= 135 && angle <= 180) || (angle >= -180 && angle < -135));
        animator.SetBool("Moving Right", (angle >= -45 && angle < 45));
        animator.SetBool("Moving Up", (angle >= 45 && angle < 135));
        animator.SetBool("Moving Down", (angle >= -135 && angle <= -45));
    }
}
