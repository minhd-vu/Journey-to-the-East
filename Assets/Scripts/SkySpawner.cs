
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkySpawner : MonoBehaviour
{
    [System.Serializable]
    private class MinMaxRange
    {
        public float min = 0;
        public float max = 0;
    }

    [SerializeField] private GameObject[] objects = null;
    [SerializeField] private MinMaxRange speedRange = null;
    [SerializeField] private MinMaxRange heightRange = null;
    [SerializeField] private float time = 0f;

    private void Start()
    {
        StartCoroutine(Spawn());
    }

    private IEnumerator Spawn()
    {
        while (objects != null)
        {
            GameObject b = Instantiate(objects[Random.Range(0, objects.Length)], new Vector2(28, Random.Range(heightRange.min, heightRange.max)), Quaternion.identity);
            b.GetComponent<SkyController>().moveSpeed = Random.Range(speedRange.min, speedRange.max);
            yield return new WaitForSeconds(time);
        }
    }
}
