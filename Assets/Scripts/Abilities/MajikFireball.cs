using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MajikFireball : ExplosiveProjectile
{
    [SerializeField] private string preFireSound = "";
    [SerializeField] private GameObject majikFire = null;
    [SerializeField] private float majikFireDuration = 5f;
    [SerializeField] private int majikFireCount = 3;
    // Start is called before the first frame update
    protected override IEnumerator Shoot()
    {
        float delay = 0f;
        GameObject.FindWithTag("Off Hand").GetComponent<Animator>().SetTrigger("Cast Spell");
        AudioManager.instance.Play(preFireSound);
        if (GetComponent<Animator>() != null)
        {
            delay = GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).length;
            GetComponent<Collider2D>().enabled = false;
        }

        yield return new WaitForSeconds(delay);
        GetComponent<Rigidbody2D>().AddForce(transform.right * force, ForceMode2D.Impulse);
        GetComponent<Collider2D>().enabled = true;
        AudioManager.instance.Play(fireSound);
    }
    protected override void AreaOfEffectCollision()
    {
        base.AreaOfEffectCollision();

        for (int i = 0; i < majikFireCount; ++i) {
            Destroy(Instantiate(majikFire, transform.position + Random.insideUnitSphere * radius, Quaternion.identity), majikFireDuration);
        }
    }
}
