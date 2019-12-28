using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 3f;
    private Rigidbody2D rb;
    private Vector2 input;
    private Vector2 mousePosition;
    private Animator animator;
    [SerializeField] private GameObject arm = null;
    private GameObject leftArm = null;
    private GameObject rightArm = null;
    [SerializeField] private GameObject weapon = null;
    [SerializeField] private GameObject offhand = null;

    private bool facingLeft, facingRight, facingUp, facingDown;
    [SerializeField] private Vector3[] leftArmPositions = null;
    [SerializeField] private Vector3[] rightArmPositions = null;

    private Coroutine roll;
    [SerializeField] private float rollForce = 3f;

    public float attackTime;
    public float damageTime;
    public float deathTime;
    public float idleTime;

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
        input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")).normalized;
        mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        if (!animator.GetBool("Slashing") && !animator.GetBool("Rolling"))
        {
            if (Input.GetButtonDown("Fire2"))
            {
                StartCoroutine(Slash());
            }

            else if (Input.GetButtonDown("Jump") && animator.GetBool("Moving"))
            {
                roll = StartCoroutine(Roll());
            }
        }

        if (animator.GetBool("Slashing") || animator.GetBool("Rolling"))
        {
            input = Vector2.zero;
        }
    }

    IEnumerator Slash()
    {
        animator.SetBool("Slashing", true);
        AudioManager.instance.Play("Sword Slash " + Random.Range(1, 3));
        leftArm.SetActive(false);
        rightArm.SetActive(false);

        float duration = 0f;

        if (animator.GetBool("Facing Left"))
        {
            duration = animationTimes["Player_Slash_Left"];
        }
        else if (animator.GetBool("Facing Right"))
        {
            duration = animationTimes["Player_Slash_Right"];

        }
        else if (animator.GetBool("Facing Up"))
        {
            duration = animationTimes["Player_Slash_Up"];
        }
        else if (animator.GetBool("Facing Down"))
        {
            duration = animationTimes["Player_Slash_Down"];
        }

        yield return new WaitForSeconds(duration);

        animator.SetBool("Slashing", false);
        leftArm.SetActive(true);
        rightArm.SetActive(true);
    }
    IEnumerator Roll()
    {
        animator.SetBool("Rolling", true);
        leftArm.SetActive(false);
        rightArm.SetActive(false);

        Vector3 rollVector = rb.transform.position + (Vector3)input * rollForce;

        float duration = animator.GetCurrentAnimatorStateInfo(0).length;

        if (animator.GetBool("Moving Left"))
        {
            duration = animationTimes["Player_Roll_Left"];
        }
        else if (animator.GetBool("Moving Right"))
        {
            duration = animationTimes["Player_Roll_Right"];
        }
        else if (animator.GetBool("Moving Up"))
        {
            duration = animationTimes["Player_Roll_Up"];
        }
        else if (animator.GetBool("Moving Down"))
        {
            duration = animationTimes["Player_Roll_Down"];
        }
        else if (animator.GetBool("Moving Up Left"))
        {
            duration = animationTimes["Player_Roll_Up_Left"];
        }
        else if (animator.GetBool("Moving Up Right"))
        {
            duration = animationTimes["Player_Roll_Up_Right"];
        }
        else if (animator.GetBool("Moving Down Left"))
        {
            duration = animationTimes["Player_Roll_Down_Left"];
        }
        else if (animator.GetBool("Moving Down Right"))
        {
            duration = animationTimes["Player_Roll_Down_Right"];
        }

        rb.DOMove(rollVector, duration);

        yield return new WaitForSeconds(duration);

        animator.SetBool("Rolling", false);
        leftArm.SetActive(true);
        rightArm.SetActive(true);
    }
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (animator.GetBool("Rolling") && roll != null)
        {
            rb.DOKill();
            StopCoroutine(roll);

            animator.SetBool("Rolling", false);
            leftArm.SetActive(true);
            rightArm.SetActive(true);
        }
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
        if (facingDown = (angle >= -135 && angle <= -45))
        {
            leftArm.GetComponent<Transform>().position = transform.position + leftArmPositions[3];
            leftArm.GetComponent<SpriteRenderer>().sortingOrder = GetComponent<SpriteRenderer>().sortingOrder + 1;
            rightArm.GetComponent<Transform>().position = transform.position + rightArmPositions[3];
            rightArm.GetComponent<SpriteRenderer>().sortingOrder = GetComponent<SpriteRenderer>().sortingOrder + 1;
            weapon.GetComponent<SpriteRenderer>().sortingOrder = rightArm.GetComponent<SpriteRenderer>().sortingOrder + 1;
            offhand.GetComponent<SpriteRenderer>().sortingOrder = leftArm.GetComponent<SpriteRenderer>().sortingOrder + 1;
        }

        leftArm.transform.localScale = rightArm.transform.localScale = (angle > 90 || angle < -90) ? new Vector3(1, -1, 1) : new Vector3(1, 1, 1);

        if (!animator.GetBool("Slashing"))
        {
            animator.SetBool("Facing Left", facingLeft);
            animator.SetBool("Facing Right", facingRight);
            animator.SetBool("Facing Up", facingUp);
            animator.SetBool("Facing Down", facingDown);
        }

        animator.SetBool("Moving", Mathf.Abs(input.magnitude) > 0);

        if (!animator.GetBool("Rolling"))
        {
            animator.SetBool("Moving Left", rb.velocity.x < 0 && rb.velocity.y == 0);
            animator.SetBool("Moving Right", rb.velocity.x > 0 && rb.velocity.y == 0);
            animator.SetBool("Moving Up", rb.velocity.y > 0 && rb.velocity.x == 0);
            animator.SetBool("Moving Up Left", rb.velocity.y > 0 && rb.velocity.x < 0);
            animator.SetBool("Moving Up Right", rb.velocity.y > 0 && rb.velocity.x > 0);
            animator.SetBool("Moving Down", rb.velocity.y < 0 && rb.velocity.x == 0);
            animator.SetBool("Moving Down Left", rb.velocity.y < 0 && rb.velocity.x < 0);
            animator.SetBool("Moving Down Right", rb.velocity.y < 0 && rb.velocity.x > 0);
        }

        leftArm.GetComponent<Transform>().rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        rightArm.GetComponent<Transform>().rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }
}
