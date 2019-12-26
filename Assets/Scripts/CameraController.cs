using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] public Transform target;

    void Update()
    {
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        transform.position = new Vector3((target.position.x + mousePosition.x) / 4, (target.position.y + mousePosition.y) / 4, 0f);
        //transform.position = new Vector3(mousePosition.x, mousePosition.y, 0f);
    }
}
