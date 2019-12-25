using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;
    private Rigidbody2D rb;
    private Vector2 input;
    private Vector2 mousePosition;
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
        input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")).normalized;
        mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }

    private void FixedUpdate()
    {
        rb.velocity = input * moveSpeed;
        Vector2 direction = mousePosition - rb.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        animator.SetBool("Facing Left", (angle >= 135 && angle <= 180) || (angle < -135 && angle > -45));
        animator.SetBool("Facing Right", angle >= -45 && angle < 45);
        animator.SetBool("Facing Up", angle >= 45 && angle < 135);
        animator.SetBool("Facing Down", (angle >= -135 && angle <= -90) || (angle < -45 && angle > 0));
        animator.SetFloat("Mouse Angle", angle);

    }
}
