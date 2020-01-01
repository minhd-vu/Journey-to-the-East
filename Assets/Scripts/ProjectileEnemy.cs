using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileEnemy : Enemy
{
    [SerializeField] private GameObject projectile = null;
    [SerializeField] private float range = 10f;
    [SerializeField] private float projectilesPerSecond = 1f;
    private Transform firePoint;

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        firePoint = transform.Find("Fire Point");
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();

        if (isAlive && Vector3.Distance(GameObject.FindWithTag("Player").transform.position, transform.position) <= range && (attackTimer += Time.deltaTime) >= 1f / projectilesPerSecond)
        {
            animator.SetTrigger("Attack");
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            Instantiate(projectile, firePoint.position, Quaternion.AngleAxis(angle, Vector3.forward)).GetComponent<Bullet>().damage = damage;
            attackTimer = 0f;
        }
    }

    protected override void OnCollisionEnter2D(Collision2D collision)
    {
    }

    protected override void OnCollisionStay2D(Collision2D collision)
    {
    }
}
