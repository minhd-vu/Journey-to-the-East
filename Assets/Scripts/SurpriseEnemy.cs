using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class SurpriseEnemy : Enemy
{
    [SerializeField] private float range = 4f;
    private GameObject player;
    private bool isAwoken;
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        player = GameObject.FindWithTag("Player");
        GetComponent<AIDestinationSetter>().target = player.transform;
        GetComponent<AIPath>().canMove = false;
        isAwoken = false;
        isAlive = false;

        foreach (Collider2D collider in GetComponentsInChildren<Collider2D>())
        {
            collider.enabled = false;
        }

    }

    // Update is called once per frame
    protected override void Update()
    {
        if (!isAwoken && Vector2.Distance(player.transform.position , transform.position) <= range)
        {
            animator.SetTrigger("Awaken");
        }

        if (isAwoken)
        {
            base.Update();
        }
    }

    private void Awaken()
    {
        isAwoken = true;
        foreach (Collider2D collider in GetComponentsInChildren<Collider2D>())
        {
            collider.enabled = true;
        }

        isAlive = true;
        GetComponent<AIPath>().canMove = true;

    }
}
