﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 3f;
    private Rigidbody2D rb;
    private Vector2 input;
    private Vector2 mousePosition;
    private Animator animator;
    [SerializeField] private GameObject leftArm = null;
    [SerializeField] private GameObject rightArm = null;
    [SerializeField] private GameObject weapon = null;
    [SerializeField] private GameObject offhand = null;

    private bool facingLeft, facingRight, facingUp, facingDown;
    [SerializeField] private Vector3[] leftArmPositions = null;
    [SerializeField] private Vector3[] rightArmPositions = null;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        weapon = Instantiate(weapon, rightArm.GetComponent<Transform>());
        offhand = Instantiate(offhand, leftArm.GetComponent<Transform>());
    }

    // Update is called once per frame
    void Update()
    {
        input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")).normalized;
        mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        if (Input.GetButtonDown("Fire2") && !animator.GetBool("Slashing"))
        {
            animator.SetBool("Slashing", true);
            leftArm.SetActive(false);
            rightArm.SetActive(false);
            StartCoroutine(Slash(0.7f));
        }
    }

    IEnumerator Slash(float duration)
    {
        yield return new WaitForSeconds(duration);
        animator.SetBool("Slashing", false);
        leftArm.SetActive(true);
        rightArm.SetActive(true);
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
            offhand.GetComponent<SpriteRenderer>().sortingOrder = leftArm.GetComponent<SpriteRenderer>().sortingOrder - 1;
        }
        if (facingRight = (angle >= -45 && angle < 45))
        {
            leftArm.GetComponent<Transform>().position = transform.position + leftArmPositions[1];
            leftArm.GetComponent<SpriteRenderer>().sortingOrder = GetComponent<SpriteRenderer>().sortingOrder - 1;
            rightArm.GetComponent<Transform>().position = transform.position + rightArmPositions[1];
            rightArm.GetComponent<SpriteRenderer>().sortingOrder = GetComponent<SpriteRenderer>().sortingOrder + 1;
            weapon.GetComponent<SpriteRenderer>().sortingOrder = rightArm.GetComponent<SpriteRenderer>().sortingOrder - 1;
            offhand.GetComponent<SpriteRenderer>().sortingOrder = leftArm.GetComponent<SpriteRenderer>().sortingOrder + 1;
        }
        if (facingUp = (angle >= 45 && angle < 135))
        {
            leftArm.GetComponent<Transform>().position = transform.position + leftArmPositions[2];
            leftArm.GetComponent<SpriteRenderer>().sortingOrder = GetComponent<SpriteRenderer>().sortingOrder - 1;
            rightArm.GetComponent<Transform>().position = transform.position + rightArmPositions[2];
            rightArm.GetComponent<SpriteRenderer>().sortingOrder = GetComponent<SpriteRenderer>().sortingOrder - 1;
            weapon.GetComponent<SpriteRenderer>().sortingOrder = rightArm.GetComponent<SpriteRenderer>().sortingOrder - 1;
            offhand.GetComponent<SpriteRenderer>().sortingOrder = leftArm.GetComponent<SpriteRenderer>().sortingOrder - 1;
        }
        if (facingDown = (angle >= -135 && angle <= -90) || (angle < -45 && angle > 0))
        {
            leftArm.GetComponent<Transform>().position = transform.position + leftArmPositions[3];
            leftArm.GetComponent<SpriteRenderer>().sortingOrder = GetComponent<SpriteRenderer>().sortingOrder + 1;
            rightArm.GetComponent<Transform>().position = transform.position + rightArmPositions[3];
            rightArm.GetComponent<SpriteRenderer>().sortingOrder = GetComponent<SpriteRenderer>().sortingOrder + 1;
            weapon.GetComponent<SpriteRenderer>().sortingOrder = rightArm.GetComponent<SpriteRenderer>().sortingOrder + 1;
            offhand.GetComponent<SpriteRenderer>().sortingOrder = leftArm.GetComponent<SpriteRenderer>().sortingOrder + 1;
        }

        leftArm.transform.localScale = rightArm.transform.localScale = (angle > 90 || angle < -90) ? new Vector3(1, -1, 1) : new Vector3(1, 1, 1);

        animator.SetBool("Facing Left", facingLeft);
        animator.SetBool("Facing Right", facingRight);
        animator.SetBool("Facing Up", facingUp);
        animator.SetBool("Facing Down", facingDown);
        animator.SetBool("Moving", Mathf.Abs(input.magnitude) > 0);

        leftArm.GetComponent<Transform>().rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        rightArm.GetComponent<Transform>().rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }
}
