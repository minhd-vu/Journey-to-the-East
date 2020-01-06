using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slash : Ability
{
    [SerializeField] private float slashRange = 1f;
    [SerializeField] private LayerMask slashLayers = 0;
    [SerializeField] private GameObject slashParticles = null;

    protected override bool CanCast()
    {
        return base.CanCast() && !isConcurrentActive;
    }

    protected override IEnumerator CastAbility()
    {
        // Display the sword slash animation.
        isActive = true;
        isConcurrentActive = true;
        player.animator.SetTrigger("Slash");

        // Play a random sword slash sound.
        AudioManager.instance.PlayRandom("Sword Slash 1", "Sword Slash 2");

        Collider2D[] colliders = Physics2D.OverlapCircleAll(player.rightArm.GetComponentInChildren<Weapon>().firePoint.position, slashRange, slashLayers);

        foreach (Collider2D collider in colliders)
        {
            Damageable d = collider.GetComponent<Damageable>();
            if (d != null || (d = collider.GetComponentInParent<Damageable>()) != null)
            {
                d.Damage(damage);
                Instantiate(slashParticles, d.transform.position, Quaternion.AngleAxis(player.angle, Vector3.forward));
            }

            else
            {
                Projectile p = collider.GetComponent<Projectile>();
                if (p != null)
                {
                    p.GetComponent<Rigidbody2D>().velocity = player.direction * p.GetComponent<Rigidbody2D>().velocity.magnitude;
                    p.initialPosition = p.transform.position;
                    p.transform.rotation = Quaternion.AngleAxis(player.angle, Vector3.forward);
                    p.gameObject.layer = LayerMask.NameToLayer("Player Projectile");

                    if ((p = collider.GetComponent<ExplosiveProjectile>()) != null)
                    {
                        ((ExplosiveProjectile)p).layers = LayerMask.GetMask("Enemy");
                    }
                }
            }
        }

        // Make the arms disappear.
        player.leftArm.SetActive(false);
        player.rightArm.SetActive(false);
        player.updateFacingDirection = false;

        //yield return new WaitForSeconds(player.animator.GetCurrentAnimatorStateInfo(0).length);
        yield return null;
    }

    protected void StopSlash()
    {
        StopAbility();
        // Return the arms to normal.
        if (player.isAlive)
        {
            player.leftArm.SetActive(true);
            player.rightArm.SetActive(true);
            isConcurrentActive = false;
            player.updateFacingDirection = true;
        }
    }
}
