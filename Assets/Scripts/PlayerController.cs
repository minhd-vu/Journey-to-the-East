using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

public class PlayerController : Damageable
{
    [HideInInspector] public Rigidbody2D rb;
    [HideInInspector] public Animator animator;

    [HideInInspector] public Vector2 input;
    [HideInInspector] public Vector2 mousePosition;

    [SerializeField] public float moveSpeed = 3f;

    [SerializeField] public GameObject leftArm = null;
    [SerializeField] public GameObject rightArm = null;

    [SerializeField] private GameObject mainHand = null;
    [SerializeField] private GameObject offHand = null;

    [SerializeField] private Vector3[] leftArmPositions = null;
    [SerializeField] private Vector3[] rightArmPositions = null;

    [SerializeField] private GameObject walkingParticles = null;

    private float walkingParticlesTimer;
    [SerializeField] private float walkingParticlesTime = 0.3f;
    [SerializeField] private float walkingParticlesDistance = 0.3f;

    private Image healthBar;
    private Image manaBar;

    private Coroutine lerpHealthBar;

    private enum Direction
    {
        Left = 0,
        Right,
        Up,
        Down,
    }

    private bool[] facingDirection = new bool[Enum.GetNames(typeof(Direction)).Length];
    private bool[] movingDirection = new bool[Enum.GetNames(typeof(Direction)).Length];

    [HideInInspector] public bool updateFacingDirection = true;
    [HideInInspector] public bool updateMovingDirection = true;

    private float mana;
    [SerializeField] private float maxMana = 100f;
    [HideInInspector]
    public float Mana
    {
        get
        {
            return mana;
        }
        set
        {
            if ((mana = value) > maxMana)
            {
                mana = maxMana;
            }
            else if (mana <= 0)
            {
                mana = 0;
            }
        }
    }

    [SerializeField] private float healthPerSecond = 5f;
    [SerializeField] private float manaPerSecond = 10f;

    [HideInInspector] public Vector2 direction;
    [HideInInspector] public float angle;

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        mainHand = Instantiate(mainHand, rightArm.transform.Find("Arm"));
        offHand = Instantiate(offHand, leftArm.transform.Find("Arm"));
        healthBar = GameObject.FindWithTag("Health Bar").GetComponent<Image>();
        manaBar = GameObject.FindWithTag("Mana Bar").GetComponent<Image>();
        leftArm.SetActive(false);
        rightArm.SetActive(false);
        isAlive = false;
        mana = maxMana;
        walkingParticlesTimer = 0f;

        StartCoroutine(RegenHealth());
        StartCoroutine(RegenMana());
        StartCoroutine(Awaken());
    }

    private IEnumerator Awaken()
    {
        yield return new WaitForSeconds(2f);

        animator.SetTrigger("Awake");
        
        yield return new WaitForSeconds(0.667f);

        isAlive = true;
        leftArm.SetActive(true);
        rightArm.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        if (!PauseMenu.isPaused && isAlive)
        {
            // Store movement input.
            input = updateFacingDirection ? new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")).normalized : Vector2.zero;
            // Store mouse input.
            mousePosition = updateMovingDirection ? (Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition) : Vector2.zero;

            foreach (Ability ability in GetComponents<Ability>())
            {
                ability.OnUpdate();
            }

            float manaPercent = Mana / maxMana;
            if (manaBar.fillAmount != manaPercent)
            {
                manaBar.fillAmount = Mathf.Lerp(manaBar.fillAmount, manaPercent, Time.deltaTime * 15);
            }
        }
    }

    IEnumerator RegenHealth()
    {
        while (isAlive)
        {
            Health += healthPerSecond;
            yield return new WaitForSeconds(1f);
        }
    }

    IEnumerator RegenMana()
    {
        while (isAlive)
        {
            Mana += manaPerSecond;
            yield return new WaitForSeconds(1f);
        }
    }

    private void FixedUpdate()
    {
        if (updateMovingDirection)
        {
            rb.velocity = input * moveSpeed;
        }

        if (updateFacingDirection)
        {
            direction = (mousePosition - rb.position).normalized;
            angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

            // Set the direction the mouse is facing. Sort the arms accordingly.
            if (facingDirection[(int)Direction.Left] = (angle >= 135 && angle <= 180) || (angle >= -180 && angle < -135))
            {
                leftArm.transform.localPosition = new Vector3(0f, -0.001f, 0f);
                rightArm.transform.localPosition = new Vector3(0f, 0.001f, 0f);
                leftArm.transform.Find("Arm").position = leftArm.transform.position + leftArmPositions[0];
                rightArm.transform.Find("Arm").position = rightArm.transform.position + rightArmPositions[0];
                mainHand.GetComponent<SpriteRenderer>().sortingOrder = rightArm.transform.Find("Arm").GetComponent<SpriteRenderer>().sortingOrder + 1;
                offHand.GetComponent<SpriteRenderer>().sortingOrder = leftArm.transform.Find("Arm").GetComponent<SpriteRenderer>().sortingOrder - 1;
            }
            else if (facingDirection[(int)Direction.Right] = (angle >= -45 && angle < 45))
            {
                leftArm.transform.localPosition = new Vector3(0f, 0.001f, 0f);
                rightArm.transform.localPosition = new Vector3(0f, -0.001f, 0f);
                leftArm.transform.Find("Arm").position = leftArm.transform.position + leftArmPositions[1];
                rightArm.transform.Find("Arm").position = rightArm.transform.position + rightArmPositions[1];
                mainHand.GetComponent<SpriteRenderer>().sortingOrder = rightArm.transform.Find("Arm").GetComponent<SpriteRenderer>().sortingOrder - 1;
                offHand.GetComponent<SpriteRenderer>().sortingOrder = leftArm.transform.Find("Arm").GetComponent<SpriteRenderer>().sortingOrder + 1;
            }
            if (facingDirection[(int)Direction.Up] = (angle >= 45 && angle < 135))
            {
                leftArm.transform.localPosition = new Vector3(0f, 0.001f, 0f);
                rightArm.transform.localPosition = new Vector3(0f, 0.001f, 0f);
                leftArm.transform.Find("Arm").position = leftArm.transform.position + leftArmPositions[2];
                rightArm.transform.Find("Arm").position = rightArm.transform.position + rightArmPositions[2];
                mainHand.GetComponent<SpriteRenderer>().sortingOrder = rightArm.transform.Find("Arm").GetComponent<SpriteRenderer>().sortingOrder - 1;
                offHand.GetComponent<SpriteRenderer>().sortingOrder = leftArm.transform.Find("Arm").GetComponent<SpriteRenderer>().sortingOrder - 1;
            }
            else if (facingDirection[(int)Direction.Down] = (angle >= -135 && angle <= -45))
            {
                leftArm.transform.localPosition = new Vector3(0f, -0.001f, 0f);
                rightArm.transform.localPosition = new Vector3(0f, -0.001f, 0f);
                leftArm.transform.Find("Arm").position = leftArm.transform.position + leftArmPositions[3];
                rightArm.transform.Find("Arm").position = rightArm.transform.position + rightArmPositions[3];
                mainHand.GetComponent<SpriteRenderer>().sortingOrder = rightArm.transform.Find("Arm").GetComponent<SpriteRenderer>().sortingOrder + 1;
                offHand.GetComponent<SpriteRenderer>().sortingOrder = leftArm.transform.Find("Arm").GetComponent<SpriteRenderer>().sortingOrder + 1;
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
            leftArm.transform.Find("Arm").localScale = rightArm.transform.Find("Arm").localScale = (angle > 90 || angle < -90) ? new Vector3(1, -1, 1) : new Vector3(1, 1, 1);
        }

        animator.SetBool("Moving", Mathf.Abs(input.magnitude) > 0);

        if (animator.GetBool("Moving"))
        {
            AudioManager.instance.PlayLoop("Walking");

            if ((walkingParticlesTimer += Time.deltaTime) >= walkingParticlesTime)
            {
                Instantiate(walkingParticles, transform.position - (transform.forward * walkingParticlesDistance), Quaternion.identity);
                walkingParticlesTimer = 0f;
            }
        }
        else
        {
            AudioManager.instance.StopLoop("Walking");
        }

        // Prevent the player from moving if they are slashing.
        if (updateFacingDirection)
        {
            animator.SetFloat("Direction X", direction.x);
            animator.SetFloat("Direction Y", direction.y);
        }

        // Prevent the player from moving if the are rolling.
        if (updateMovingDirection)
        {
            animator.SetFloat("Velocity X", rb.velocity.normalized.x);
            animator.SetFloat("Velocity Y", rb.velocity.normalized.y);

            movingDirection[(int)Direction.Left] = rb.velocity.x < 0;
            movingDirection[(int)Direction.Right] = rb.velocity.x > 0;
            movingDirection[(int)Direction.Up] = rb.velocity.y > 0;
            movingDirection[(int)Direction.Down] = rb.velocity.y < 0;
        }

        // Rotate the arms based on the mouse position.
        leftArm.transform.Find("Arm").rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        rightArm.transform.Find("Arm").rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }

    public override void Damage(float damage)
    {
        Health -= damage;

        if (lerpHealthBar != null)
        {
            StopCoroutine(lerpHealthBar);
        }

        lerpHealthBar = StartCoroutine(LerpHealthBar());
    }

    private IEnumerator LerpHealthBar()
    {
        while (Mathf.Abs(healthBar.fillAmount - HealthPercent) >= 0.01f)
        {
            healthBar.fillAmount = Mathf.Lerp(healthBar.fillAmount, HealthPercent, Time.deltaTime * 15);
            yield return new WaitForSeconds(Time.deltaTime);
        }
    }

    protected override void Kill()
    {
        animator.SetTrigger("Death");
        rb.velocity = Vector3.zero;
        GetComponent<Collider2D>().enabled = false;
        rb.isKinematic = false;
        leftArm.SetActive(false);
        rightArm.SetActive(false);
        enabled = false;
    }
}
