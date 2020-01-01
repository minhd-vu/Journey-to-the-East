using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosiveProjectile : Projectile
{
    [SerializeField] protected float radius = 0f;
    [SerializeField] private LayerMask layers = 0;
    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Hitbox"))
        {
            AreaOfEffectCollision();
        }
    }


    protected override void OnCollisionEnter2D(Collision2D collision)
    {
        AreaOfEffectCollision();
    }

    protected virtual void AreaOfEffectCollision()
    {
        GetComponent<Animator>().SetTrigger("Hit");
        GetComponent<Collider2D>().enabled = false;
        GetComponent<Rigidbody2D>().isKinematic = false;
        GetComponent<Rigidbody2D>().velocity = Vector3.zero;
        GetComponent<SpriteRenderer>().sortingOrder += 1;

        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, radius, layers);

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
}
