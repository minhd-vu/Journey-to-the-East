using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EZCameraShake;

public class Weapon : MonoBehaviour
{
    [SerializeField] private GameObject bullet = null;
    [HideInInspector] public Transform firePoint;
    [HideInInspector] public bool isFlipped;
    private Vector3 localScaleNormal;
    private Vector3 localScaleFlip;
    [SerializeField] private float shakeMagnitude = 1f;
    [SerializeField] private float shakeRoughness = 1f;

    // Start is called before the first frame update
    void Start()
    {
        firePoint = transform.Find("Fire Point");
        localScaleNormal = transform.localScale;
        localScaleFlip = new Vector3(transform.localScale.x, -transform.localScale.y, transform.localScale.z);
    }

    // Update is called once per frame
    void Update()
    {
        transform.localScale = isFlipped ? localScaleFlip : localScaleNormal;

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
