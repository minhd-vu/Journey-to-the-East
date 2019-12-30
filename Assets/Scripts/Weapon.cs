using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    [SerializeField] private GameObject bullet = null;
    [SerializeField] private GameObject muzzleFlash = null;
    [HideInInspector] public Transform firePoint = null;

    [SerializeField] private float shakeMagnitude = 1f;
    [SerializeField] private float shakeRoughness = 1f;
    [SerializeField] private float shakeDuration = 0.3f;

    [SerializeField] private bool isAutomatic = false;
    private float bulletTimer;
    [SerializeField] private float bulletsPerSecond = 0f;
    [SerializeField] private float damage = 0f;

    // Start is called before the first frame update
    void Start()
    {
        firePoint = transform.Find("Fire Point");
        bulletTimer = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Fire1") || (isAutomatic && Input.GetButton("Fire1") && (bulletTimer += Time.deltaTime) >= 1f / bulletsPerSecond))
        {
            Shoot();
        }
    }

    void Shoot()
    {
        Instantiate(bullet, firePoint.position, firePoint.rotation).GetComponent<Bullet>().damage = damage;
        Instantiate(muzzleFlash, firePoint);
        AudioManager.instance.Play("Fire");
        CameraShake.instance.ShakeOnce(shakeMagnitude, shakeRoughness, shakeDuration);
        bulletTimer = 0f;
    }
}
