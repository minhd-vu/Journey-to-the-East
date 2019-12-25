using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }
    Vector2 direction = mousePosition - rb.position;
    float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
}
