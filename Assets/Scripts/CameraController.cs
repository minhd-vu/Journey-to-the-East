using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private float smoothing = 5f;
    [SerializeField] private float viewDistance = 5f;
    private Vector3 center;

    void Update()
    {
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        center = new Vector3((player.position.x + mousePosition.x) / viewDistance, (player.position.y + mousePosition.y) / viewDistance, transform.position.z);
        transform.position = Vector3.Lerp(transform.position, center, Time.deltaTime * smoothing);
    }
}
