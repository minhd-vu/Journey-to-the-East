using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class SurpriseEnemy : Enemy
{
    [SerializeField] private float range = 4f;
    private GameObject player;
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        player = GameObject.FindWithTag("Player");
        GetComponent<AIDestinationSetter>().target = player.transform;
        GetComponent<AIPath>().canMove = false;
        isAlive = false;
    }

    // Update is called once per frame
    protected override void Update()
    {
        if (!isAlive && Vector2.Distance(player.transform.position , transform.position) <= range)
        {
            StartCoroutine(Awaken());
        }
        base.Update();
    }

    private IEnumerator Awaken()
    {
        animator.SetTrigger("Awaken");

        yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);

        isAlive = true;
        GetComponent<AIPath>().canMove = true;

    }
}
