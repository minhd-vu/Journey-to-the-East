using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private GameObject[] enemies = null;
    private float timer;
    [SerializeField] private float time = 5f;

    // Start is called before the first frame update
    void Start()
    {
        timer = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        if ((timer += Time.deltaTime) >= time)
        {
            GameObject enemy = Instantiate(enemies[Random.Range(0, enemies.Length)], transform);
            enemy.GetComponent<AIDestinationSetter>().target = GameObject.FindGameObjectWithTag("Player").transform;
            timer = 0f;
        }
    }
}
