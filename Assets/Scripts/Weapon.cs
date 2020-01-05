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

    [SerializeField] private int numberOfBullets = 1;
    [SerializeField] private bool isAutomatic = false;
    private float bulletTimer;
    [SerializeField] private float bulletsPerSecond = 0f;
    [SerializeField] private float damage = 0f;
    [SerializeField] private float manaCost = 0f;
    [SerializeField] private string buttonName = "Fire1";
    [SerializeField] private float spreadDegree = 5f;

    // Start is called before the first frame update
    void Start()
    {
        firePoint = transform.Find("Fire Point");
        bulletTimer = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        if (!PauseMenu.isPaused)
        {
            if (Input.GetButtonDown(buttonName) || (isAutomatic && (bulletTimer += Time.deltaTime) >= 1f / bulletsPerSecond) && Input.GetButton(buttonName))
            {
                Shoot();
            }
        }
    }

    void Shoot()
    {
        PlayerController player = GameObject.FindWithTag("Player").GetComponent<PlayerController>();
        if (player.Mana >= manaCost)
        {
            for (int i = 0; i < numberOfBullets; ++i)
            {
                Projectile p = Instantiate(bullet, firePoint.position, firePoint.rotation).GetComponent<Projectile>();
                p.damage = damage;
                p.transform.Rotate(Vector3.forward * Random.Range(-1f, 1f) * spreadDegree);
            }

            if (muzzleFlash != null)
            {
                Instantiate(muzzleFlash, firePoint);
            }

            //AudioManager.instance.Play("Fire");
            CameraShake.instance.ShakeOnce(shakeMagnitude, shakeRoughness, shakeDuration);

            player.Mana -= manaCost;
            bulletTimer = 0f;
        }
    }
}
