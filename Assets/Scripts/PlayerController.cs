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
    [SerializeField] private GameObject leftArm;
    [SerializeField] private GameObject rightArm;
    [SerializeField] private GameObject weapon;

    private bool facingLeft, facingRight, facingUp, facingDown;
    [SerializeField] private Vector3[] leftArmPositions;
    [SerializeField] private Vector3[] rightArmPositions;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        weapon = Instantiate(weapon, rightArm.GetComponent<Transform>());
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

        if (facingLeft = (angle >= 135 && angle <= 180) || (angle >= -180 && angle < -135))
        {
            leftArm.GetComponent<Transform>().position = transform.position + leftArmPositions[0];
            leftArm.GetComponent<SpriteRenderer>().sortingOrder = GetComponent<SpriteRenderer>().sortingOrder + 1;
            rightArm.GetComponent<Transform>().position = transform.position + rightArmPositions[0];
            rightArm.GetComponent<SpriteRenderer>().sortingOrder = GetComponent<SpriteRenderer>().sortingOrder - 1;
            weapon.GetComponent<SpriteRenderer>().sortingOrder = rightArm.GetComponent<SpriteRenderer>().sortingOrder + 1;
        }
        if (facingRight = (angle >= -45 && angle < 45))
        {
            leftArm.GetComponent<Transform>().position = transform.position + leftArmPositions[1];
            leftArm.GetComponent<SpriteRenderer>().sortingOrder = GetComponent<SpriteRenderer>().sortingOrder - 1;
            rightArm.GetComponent<Transform>().position = transform.position + rightArmPositions[1];
            rightArm.GetComponent<SpriteRenderer>().sortingOrder = GetComponent<SpriteRenderer>().sortingOrder + 1;
            weapon.GetComponent<SpriteRenderer>().sortingOrder = rightArm.GetComponent<SpriteRenderer>().sortingOrder - 1;
        }
        if (facingUp = (angle >= 45 && angle < 135))
        {
            leftArm.GetComponent<Transform>().position = transform.position + leftArmPositions[2];
            leftArm.GetComponent<SpriteRenderer>().sortingOrder = GetComponent<SpriteRenderer>().sortingOrder - 1;
            rightArm.GetComponent<Transform>().position = transform.position + rightArmPositions[2];
            rightArm.GetComponent<SpriteRenderer>().sortingOrder = GetComponent<SpriteRenderer>().sortingOrder - 1;
            weapon.GetComponent<SpriteRenderer>().sortingOrder = rightArm.GetComponent<SpriteRenderer>().sortingOrder - 1;
        }
        if (facingDown = (angle >= -135 && angle <= -90) || (angle < -45 && angle > 0))
        {
            leftArm.GetComponent<Transform>().position = transform.position + leftArmPositions[3];
            leftArm.GetComponent<SpriteRenderer>().sortingOrder = GetComponent<SpriteRenderer>().sortingOrder + 1;
            rightArm.GetComponent<Transform>().position = transform.position + rightArmPositions[3];
            rightArm.GetComponent<SpriteRenderer>().sortingOrder = GetComponent<SpriteRenderer>().sortingOrder + 1;
            weapon.GetComponent<SpriteRenderer>().sortingOrder = rightArm.GetComponent<SpriteRenderer>().sortingOrder + 1;
        }

        rightArm.GetComponentInChildren<Weapon>().isFlipped = leftArm.GetComponent<SpriteRenderer>().flipY = rightArm.GetComponent<SpriteRenderer>().flipY = angle > 90 || angle < -90;        

        animator.SetBool("Facing Left", facingLeft);
        animator.SetBool("Facing Right", facingRight);
        animator.SetBool("Facing Up", facingUp);
        animator.SetBool("Facing Down", facingDown);
        animator.SetBool("Moving", Mathf.Abs(input.magnitude) > 0);

        leftArm.GetComponent<Transform>().rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        rightArm.GetComponent<Transform>().rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }
}
