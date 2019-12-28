using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] public Transform target;

    // Get the point between the mouse and the target for camera movement.
    void Update()
    {
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        transform.position = new Vector3((target.position.x + mousePosition.x) / 2f, (target.position.y + mousePosition.y) / 2f, 0f);
        //transform.position = new Vector3(mousePosition.x, mousePosition.y, 0f);
    }
}
