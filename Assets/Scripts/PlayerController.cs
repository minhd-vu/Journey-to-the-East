﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;

public class PlayerController : MonoBehaviour
{
    private Rigidbody2D rb;
    private Animator animator;

    private Vector2 input;
    private Vector2 mousePosition;

    [SerializeField] private float moveSpeed = 3f;

    [SerializeField] private GameObject arm = null;
    private GameObject leftArm = null;
    private GameObject rightArm = null;

    [SerializeField] private GameObject weapon = null;
    [SerializeField] private GameObject offhand = null;

    [SerializeField] private Vector3[] leftArmPositions = null;
    [SerializeField] private Vector3[] rightArmPositions = null;

    private Coroutine roll;
    [SerializeField] private float rollForce = 3f;
    private enum Direction
    {
        Left = 0,
        Right,
        Up,
        Down,
    }

    private bool[] facingDirection = new bool[Enum.GetNames(typeof(Direction)).Length];
    private bool[] movingDirection = new bool[Enum.GetNames(typeof(Direction)).Length];

    private Dictionary<string, float> animationTimes = new Dictionary<string, float>();

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        leftArm = Instantiate(arm, transform);
        rightArm = Instantiate(arm, transform);
        weapon = Instantiate(weapon, rightArm.GetComponent<Transform>());
        offhand = Instantiate(offhand, leftArm.GetComponent<Transform>());
        GetAnimationClipLengths();
    }

    /**
     * Store all animation lengths in the animator in animationTimes corresponding to the animation name.
     */
    private void GetAnimationClipLengths()
    {
        AnimationClip[] clips = animator.runtimeAnimatorController.animationClips;
        foreach (AnimationClip clip in clips)
        {
            animationTimes[clip.name] = clip.length;
        }
    }

    // Update is called once per frame
    void Update()
    {
        // Store movement input.
        input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")).normalized;
        // Store mouse input.
        mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        // Prevent the player from slashing or moving when they are already slashing or rolling.
        if (!animator.GetBool("Slashing") && !animator.GetBool("Rolling"))
        {
            if (Input.GetButtonDown("Fire2"))
            {
                StartCoroutine(Slash());
            }

            else if (Input.GetButtonDown("Jump") && animator.GetBool("Moving"))
            {
                // Store the roll coroutine so that we can kill it later.
                roll = StartCoroutine(Roll());
            }
        }

        // Prevent the player from moving when they are slashing or rolling.
        if (animator.GetBool("Slashing") || animator.GetBool("Rolling"))
        {
            input = Vector2.zero;
        }
    }

    /**
     * Slash coroutine.
     */
    IEnumerator Slash()
    {
        animator.SetBool("Slashing", true);
        // Play a random sword slash sound.
        AudioManager.instance.Play("Sword Slash " + UnityEngine.Random.Range(1, 3));
        // Make the arms disappear.
        leftArm.SetActive(false);
        rightArm.SetActive(false);

        // Temporarily set the duration.
        float duration = animator.GetCurrentAnimatorStateInfo(0).length;

        // Set the duration of the coroutine to the duration of the animation clip length.
        if (facingDirection[(int)Direction.Left])
        {
            duration = animationTimes["Player_Slash_Left"];

            if (facingDirection[(int)Direction.Up])
            {
                duration = animationTimes["Player_Slash_Up_Left"];
            }
            else if (facingDirection[(int)Direction.Down])
            {
                duration = animationTimes["Player_Slash_Down_Left"];
            }
        }
        else if (facingDirection[(int)Direction.Right])
        {
            duration = animationTimes["Player_Slash_Right"];

            if (facingDirection[(int)Direction.Up])
            {
                duration = animationTimes["Player_Slash_Up_Right"];
            }
            else if (facingDirection[(int)Direction.Down])
            {
                duration = animationTimes["Player_Slash_Down_Right"];
            }
        }
        else if (facingDirection[(int)Direction.Up])
        {
            duration = animationTimes["Player_Slash_Up"];
        }
        else if (facingDirection[(int)Direction.Down])
        {
            duration = animationTimes["Player_Slash_Down"];
        }

        yield return new WaitForSeconds(duration);

        // Return the arms to normal.
        animator.SetBool("Slashing", false);
        leftArm.SetActive(true);
        rightArm.SetActive(true);
    }
    IEnumerator Roll()
    {
        animator.SetBool("Rolling", true);
        // Deactivate the arms.
        leftArm.SetActive(false);
        rightArm.SetActive(false);

        Vector3 rollVector = rb.transform.position + (Vector3)input * rollForce;

        float duration = animator.GetCurrentAnimatorStateInfo(0).length;

        // Set the duration of the coroutine to the duration of the animation clip length.
        if (movingDirection[(int)Direction.Left])
        {
            duration = animationTimes["Player_Roll_Left"];

            if (movingDirection[(int)Direction.Up])
            {
                duration = animationTimes["Player_Roll_Up_Left"];
            }
            else if (movingDirection[(int)Direction.Down])
            {
                duration = animationTimes["Player_Roll_Down_Left"];
            }
        }
        else if (movingDirection[(int)Direction.Right])
        {
            duration = animationTimes["Player_Roll_Right"];

            if (movingDirection[(int)Direction.Up])
            {
                duration = animationTimes["Player_Roll_Up_Right"];
            }
            else if (movingDirection[(int)Direction.Down])
            {
                duration = animationTimes["Player_Roll_Down_Right"];
            }
        }
        else if (movingDirection[(int)Direction.Up])
        {
            duration = animationTimes["Player_Roll_Up"];
        }
        else if (movingDirection[(int)Direction.Down])
        {
            duration = animationTimes["Player_Roll_Down"];
        }

        rb.DOMove(rollVector, duration);

        yield return new WaitForSeconds(duration);

        animator.SetBool("Rolling", false);
        leftArm.SetActive(true);
        rightArm.SetActive(true);
    }
    void OnCollisionEnter2D(Collision2D collision)
    {
        // Stop the player from rolling if there is a collision.
        if (animator.GetBool("Rolling") && roll != null)
        {
            StopCoroutine(roll);
            rb.DOKill();
            animator.SetBool("Rolling", false);
            leftArm.SetActive(true);
            rightArm.SetActive(true);
        }
    }

    private void FixedUpdate()
    {
        rb.velocity = input * moveSpeed;
        Vector2 direction = (mousePosition - rb.position).normalized;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        //if (facingDirection[(int)Direction.Left] = (angle >= 135 && angle <= 180) || (angle >= -180 && angle < -135))
        //{
        //    leftArm.GetComponent<Transform>().position = transform.position + leftArmPositions[0];
        //    rightArm.GetComponent<Transform>().position = transform.position + rightArmPositions[0];
        //}
        //else if (facingDirection[(int)Direction.Right] = (angle >= -45 && angle < 45))
        //{
        //    leftArm.GetComponent<Transform>().position = transform.position + leftArmPositions[1];
        //    rightArm.GetComponent<Transform>().position = transform.position + rightArmPositions[1];
        //}
        //if (facingDirection[(int)Direction.Up] = (angle >= 45 && angle < 135))
        //{
        //    leftArm.GetComponent<Transform>().position = transform.position + leftArmPositions[2];
        //    rightArm.GetComponent<Transform>().position = transform.position + rightArmPositions[2];
        //}
        //else if (facingDirection[(int)Direction.Down] = (angle >= -135 && angle <= -45))
        //{
        //    leftArm.GetComponent<Transform>().position = transform.position + leftArmPositions[3];
        //    rightArm.GetComponent<Transform>().position = transform.position + rightArmPositions[3];
        //}


        // Set the direction the mouse is facing.
        if (facingDirection[(int)Direction.Left] = (angle >= 135 && angle <= 180) || (angle >= -180 && angle < -135))
        {
            leftArm.GetComponent<Transform>().position = transform.position + leftArmPositions[0];
            leftArm.GetComponent<SpriteRenderer>().sortingOrder = GetComponent<SpriteRenderer>().sortingOrder + 1;
            rightArm.GetComponent<Transform>().position = transform.position + rightArmPositions[0];
            rightArm.GetComponent<SpriteRenderer>().sortingOrder = GetComponent<SpriteRenderer>().sortingOrder - 1;
            weapon.GetComponent<SpriteRenderer>().sortingOrder = rightArm.GetComponent<SpriteRenderer>().sortingOrder + 1;
            offhand.GetComponent<SpriteRenderer>().sortingOrder = leftArm.GetComponent<SpriteRenderer>().sortingOrder - 1;
        }
        else if (facingDirection[(int)Direction.Right] = (angle >= -45 && angle < 45))
        {
            leftArm.GetComponent<Transform>().position = transform.position + leftArmPositions[1];
            leftArm.GetComponent<SpriteRenderer>().sortingOrder = GetComponent<SpriteRenderer>().sortingOrder - 1;
            rightArm.GetComponent<Transform>().position = transform.position + rightArmPositions[1];
            rightArm.GetComponent<SpriteRenderer>().sortingOrder = GetComponent<SpriteRenderer>().sortingOrder + 1;
            weapon.GetComponent<SpriteRenderer>().sortingOrder = rightArm.GetComponent<SpriteRenderer>().sortingOrder - 1;
            offhand.GetComponent<SpriteRenderer>().sortingOrder = leftArm.GetComponent<SpriteRenderer>().sortingOrder + 1;
        }
        if (facingDirection[(int)Direction.Up] = (angle >= 45 && angle < 135))
        {
            leftArm.GetComponent<Transform>().position = transform.position + leftArmPositions[2];
            leftArm.GetComponent<SpriteRenderer>().sortingOrder = GetComponent<SpriteRenderer>().sortingOrder - 1;
            rightArm.GetComponent<Transform>().position = transform.position + rightArmPositions[2];
            rightArm.GetComponent<SpriteRenderer>().sortingOrder = GetComponent<SpriteRenderer>().sortingOrder - 1;
            weapon.GetComponent<SpriteRenderer>().sortingOrder = rightArm.GetComponent<SpriteRenderer>().sortingOrder - 1;
            offhand.GetComponent<SpriteRenderer>().sortingOrder = leftArm.GetComponent<SpriteRenderer>().sortingOrder - 1;
        }
        else if (facingDirection[(int)Direction.Down] = (angle >= -135 && angle <= -45))
        {
            leftArm.GetComponent<Transform>().position = transform.position + leftArmPositions[3];
            leftArm.GetComponent<SpriteRenderer>().sortingOrder = GetComponent<SpriteRenderer>().sortingOrder + 1;
            rightArm.GetComponent<Transform>().position = transform.position + rightArmPositions[3];
            rightArm.GetComponent<SpriteRenderer>().sortingOrder = GetComponent<SpriteRenderer>().sortingOrder + 1;
            weapon.GetComponent<SpriteRenderer>().sortingOrder = rightArm.GetComponent<SpriteRenderer>().sortingOrder + 1;
            offhand.GetComponent<SpriteRenderer>().sortingOrder = leftArm.GetComponent<SpriteRenderer>().sortingOrder + 1;
        }

        // Set the direction for diagonal directions.
        if (angle >= 22.5 && angle <= 67.5)
        {
            facingDirection[(int)Direction.Up] = facingDirection[(int)Direction.Right] = true;
        }

        else if (angle >= 112.5 && angle <= 157.5)
        {
            facingDirection[(int)Direction.Up] = facingDirection[(int)Direction.Left] = true;
        }

        else if (angle >= -157.5 && angle < -112.5)
        {
            facingDirection[(int)Direction.Down] = facingDirection[(int)Direction.Left] = true;
        }

        else if (angle <= -22.5 && angle >= -67.5)
        {
            facingDirection[(int)Direction.Down] = facingDirection[(int)Direction.Right] = true;
        }

        // Flip the arms if the player is facing to the left.
        leftArm.transform.localScale = rightArm.transform.localScale = (angle > 90 || angle < -90) ? new Vector3(1, -1, 1) : new Vector3(1, 1, 1);

        animator.SetBool("Moving", Mathf.Abs(input.magnitude) > 0);

        // Prevent the player from moving if they are slashing.
        if (!animator.GetBool("Slashing"))
        {
            animator.SetFloat("Direction X", direction.x);
            animator.SetFloat("Direction Y", direction.y);
        }

        // Prevent the player from moving if the are rolling.
        if (!animator.GetBool("Rolling"))
        {
            animator.SetFloat("Velocity X", rb.velocity.normalized.x);
            animator.SetFloat("Velocity Y", rb.velocity.normalized.y);

            movingDirection[(int)Direction.Left] = rb.velocity.x < 0;
            movingDirection[(int)Direction.Right] = rb.velocity.x > 0;
            movingDirection[(int)Direction.Up] = rb.velocity.y > 0;
            movingDirection[(int)Direction.Down] = rb.velocity.y < 0;
        }

        // Rotate the arms based on the mouse position.
        leftArm.GetComponent<Transform>().rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        rightArm.GetComponent<Transform>().rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }
}
