using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    [SerializeField] private GameObject bullet = null;
    [SerializeField] private GameObject muzzleFlash = null;
    [HideInInspector] public Transform firePoint;
    [SerializeField] private float shakeMagnitude = 1f;
    [SerializeField] private float shakeRoughness = 1f;
    [SerializeField] private float shakeDuration = 0.3f;
    [SerializeField] private bool isAutomatic = false;

    // Start is called before the first frame update
    void Start()
    {
        firePoint = transform.Find("Fire Point");
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Fire1") )
        {
            Shoot();
        }
    }

    void Shoot()
    {
        Instantiate(bullet, firePoint.position, firePoint.rotation);
        Instantiate(muzzleFlash, firePoint.position, firePoint.rotation);
        CameraShake.instance.ShakeOnce(shakeMagnitude, shakeRoughness, shakeDuration);
    }
}
