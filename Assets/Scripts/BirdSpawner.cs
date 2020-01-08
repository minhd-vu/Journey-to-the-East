using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BirdSpawner : MonoBehaviour
{
    [SerializeField] private GameObject bird = null;
    [SerializeField] private Vector2 speedRange = new Vector2();
    [SerializeField] private Vector2 heightRange = new Vector2();
    [SerializeField] private float time = 0f;

    private void Start()
    {
        StartCoroutine(SpawnBird());
    }

    private IEnumerator SpawnBird()
    {
        while (bird != null)
        {
            GameObject b = Instantiate(bird, new Vector2(28, Random.Range(heightRange.x, heightRange.y)), Quaternion.identity);
            b.GetComponent<Rigidbody2D>().velocity = new Vector2(-Random.Range(speedRange.x, speedRange.y), 0);
            yield return new WaitForSeconds(time);
        }
    }
}
