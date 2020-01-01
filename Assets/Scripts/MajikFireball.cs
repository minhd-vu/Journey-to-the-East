using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MajikFireball : ExplosiveProjectile
{
    [SerializeField] private string preFireSound = "";
    // Start is called before the first frame update
    protected override IEnumerator Shoot()
    {
        float delay = 0f;
        GameObject.FindWithTag("Main Hand").GetComponent<Animator>().SetTrigger("Cast Majik Fireball");
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
}
