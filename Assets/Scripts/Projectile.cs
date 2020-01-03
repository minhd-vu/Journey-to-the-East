using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [HideInInspector] public float damage = 0f;
    [SerializeField] protected float range = 10f;
    [HideInInspector] public Vector3 initialPosition;
    [SerializeField] public float force = 20f;
    [SerializeField] protected string fireSound = "";

    // Start is called before the first frame update
    protected virtual void Start()
    {
        initialPosition = transform.position;

        // Propel the bullet.
        StartCoroutine(Shoot());
    }

    protected virtual IEnumerator Shoot()
    {
        GetComponent<Rigidbody2D>().AddForce(transform.right * force, ForceMode2D.Impulse);
        GetComponent<Collider2D>().enabled = true;
        AudioManager.instance.Play(fireSound);
        yield return null;
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        // Destroy the bullet if it goes beyond its maximum range.
        if (Vector3.Distance(initialPosition, transform.position) > range)
        {
            Destroy(gameObject);
        }
    }
    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Hitbox"))
        {
            collision.GetComponentInParent<Damageable>().Damage(damage);
            Destroy(gameObject);
        }
    }

    protected virtual void OnCollisionEnter2D(Collision2D collision)
    {
        Damageable d = collision.gameObject.GetComponent<Damageable>();
        if (d != null)
        {
            d.Damage(damage);
        }

        Destroy(gameObject);
    }
}
