using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private float range = 10f;
    private Vector3 initialPosition;
    [SerializeField] float force = 20f;

    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Rigidbody2D>().AddForce(transform.right * force, ForceMode2D.Impulse);
    }

    // Update is called once per frame
    void Update()
    {
        // Destroy the bullet if it goes beyond its maximum range.
        if (Vector3.Distance(initialPosition, transform.position) > range)
        {
            Destroy(gameObject);
        }
    }
}
