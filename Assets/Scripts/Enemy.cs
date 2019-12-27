using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private Rigidbody2D rb;
    private Animator animator;
    private Vector3 lastPosition;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 delta = (transform.position - lastPosition).normalized;
        animator.SetFloat("Move X", delta.x);
        animator.SetFloat("Move Y", delta.y);
        lastPosition = transform.position;
    }
}
