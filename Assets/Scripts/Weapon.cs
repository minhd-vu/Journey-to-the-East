using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EZCameraShake;

public class Weapon : MonoBehaviour
{
    [SerializeField] private GameObject bullet = null;
    [HideInInspector] public Transform firePoint;
    [SerializeField] private float shakeMagnitude = 1f;
    [SerializeField] private float shakeRoughness = 1f;

    // Start is called before the first frame update
    void Start()
    {
        firePoint = transform.Find("Fire Point");
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            Shoot();
        }
    }

    void Shoot()
    {
        Instantiate(bullet, firePoint.position, firePoint.rotation);
        CameraShaker.Instance.ShakeOnce(shakeMagnitude, shakeRoughness, 0.1f, 0.3f);
    }
}
