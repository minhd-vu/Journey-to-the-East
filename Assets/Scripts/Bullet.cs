using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [HideInInspector] public float damage = 0f;
    [SerializeField] private float range = 10f;
    private Vector3 initialPosition;
    [SerializeField] float force = 20f;

    // Start is called before the first frame update
    void Start()
    {
        initialPosition = transform.position;

        // Propel the bullet.
        GetComponent<Rigidbody2D>().AddForce(transform.right * force, ForceMode2D.Impulse);
    }

    // Update is called once per frame
    void Update()
    {
        // Destroy the bullet if it goes beyond its maximum range.
        if (Vector3.Distance(initialPosition, transform.position) > range)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Hitbox"))
        {
            collision.GetComponentInParent<Damageable>().Damage(damage);
        }

        Destroy(gameObject);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        Destroy(gameObject);
    }
}
