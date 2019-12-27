using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private float range = 10f;
    private Vector3 initialPosition;
    [SerializeField] float force = 20f;
    [SerializeField] float muzzleFlashTime = 0.02f;
    private Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        initialPosition = transform.position;
        GetComponent<Rigidbody2D>().AddForce(transform.right * force, ForceMode2D.Impulse);
        animator = GetComponent<Animator>();

    }

    // Update is called once per frame
    void Update()
    {
        if (animator.GetBool("Flashing") && (muzzleFlashTime -= Time.deltaTime) < 0)
        {
            animator.SetBool("Flashing", false);
        }

        // Destroy the bullet if it goes beyond its maximum range.
        if (Vector3.Distance(initialPosition, transform.position) > range)
        {
            Destroy(gameObject);
        }
    }
}
