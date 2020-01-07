using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : Ability
{
    [SerializeField] private GameObject bullet = null;
    [SerializeField] private GameObject muzzleFlash = null;
    [HideInInspector] public Transform firePoint = null;

    [SerializeField] private float shakeMagnitude = 1f;
    [SerializeField] private float shakeRoughness = 1f;
    [SerializeField] private float shakeDuration = 0.3f;

    [SerializeField] private int numberOfBullets = 1;
    [SerializeField] private bool isAutomatic = false;
    [SerializeField] private float spreadDegree = 5f;

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();

        firePoint = transform.Find("Fire Point");
    }

    protected override bool CanCast()
    {
        return Input.GetButtonDown(buttonName) || (isAutomatic && base.CanCast());
    }

    protected override IEnumerator CastAbility()
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

        yield return null;
    }
}
