using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [HideInInspector] public float damage = 0f;
    [SerializeField] private float range = 10f;
    private Vector3 initialPosition;
    [SerializeField] private float force = 20f;
    [SerializeField] private float radius = 0f;

    // Start is called before the first frame update
    void Start()
    {
        initialPosition = transform.position;

        // Propel the bullet.
        StartCoroutine(Shoot());
    }

    private IEnumerator Shoot()
    {
        float delay = 0f;

        if (GetComponent<Animator>() != null)
        {
            delay = GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).length;
            GetComponent<Collider2D>().enabled = false;
        }

        yield return new WaitForSeconds(delay);
        GetComponent<Rigidbody2D>().AddForce(transform.right * force, ForceMode2D.Impulse);
        GetComponent<Collider2D>().enabled = true;
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

    private void AreaOfEffectCollision()
    {
        GetComponent<Animator>().SetTrigger("Hit");
        GetComponent<Collider2D>().enabled = false;
        GetComponent<Rigidbody2D>().isKinematic = false;
        GetComponent<Rigidbody2D>().velocity = Vector3.zero;
        GetComponent<SpriteRenderer>().sortingOrder += 1;

        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, radius);

        foreach (Collider2D collider in colliders)
        {
            Damageable d = collider.GetComponent<Damageable>();

            if (d != null)
            {
                float proximity = (transform.position - d.transform.position).magnitude;
                float effect = 1 - (proximity / radius);
                d.Damage(damage * effect);
            }
        }

        Destroy(gameObject, GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).length);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Hitbox") && radius == 0)
        {
            collision.GetComponentInParent<Damageable>().Damage(damage);
            Destroy(gameObject);
        }

        else if (radius > 0)
        {
            AreaOfEffectCollision();
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (radius > 0)
        {
            AreaOfEffectCollision();
        }

        else
        {
            Destroy(gameObject);
        }
    }
}
