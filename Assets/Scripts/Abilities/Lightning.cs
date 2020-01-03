using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lightning : Ability
{
    [SerializeField] private GameObject lightning = null;
    [SerializeField] private float radius = 1f;
    [SerializeField] private LayerMask layers = 0;

    protected override IEnumerator CastAbility()
    {
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Destroy(Instantiate(lightning, mousePosition, Quaternion.identity), 5f);
        AudioManager.instance.Play(sound);

        Collider2D[] colliders = Physics2D.OverlapCircleAll(mousePosition, radius, layers);

        foreach (Collider2D collider in colliders)
        {
            Damageable d = collider.GetComponent<Damageable>();

            if (d != null || (d = collider.GetComponentInParent<Damageable>()) != null)
            {
                float proximity = (mousePosition - (Vector2)d.transform.position).magnitude;
                float effect = 1 - (proximity / radius);
                d.Damage(damage * effect);
            }
        }

        yield return null;
    }
}
