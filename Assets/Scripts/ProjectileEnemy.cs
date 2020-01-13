using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileEnemy : Enemy
{
    [SerializeField] private GameObject projectile = null;
    [SerializeField] private float range = 10f;
    [SerializeField] private float projectilesPerSecond = 1f;
    [SerializeField] private int numberOfProjectiles = 1;
    [SerializeField] private float spreadDegree = 0f;
    private float projectileTimer;
    private Transform firePoint;

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        firePoint = transform.Find("Fire Point");
        projectileTimer = 0f;
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();

        if (isAlive && Vector3.Distance(GameObject.FindWithTag("Player").transform.position, transform.position) <= range && (projectileTimer += Time.deltaTime) >= 1f / projectilesPerSecond)
        {
            animator.SetTrigger("Attack");
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

            for (int i = 0; i < numberOfProjectiles; ++i)
            {
                Instantiate(projectile, firePoint.position, Quaternion.AngleAxis(angle + UnityEngine.Random.Range(-1f, 1f) * spreadDegree, Vector3.forward)).GetComponent<Projectile>().damage = damage;
            }

            projectileTimer = 0f;
        }
    }

    protected override void OnCollisionEnter2D(Collision2D collision)
    {
    }

    protected override void OnCollisionStay2D(Collision2D collision)
    {
    }
}
