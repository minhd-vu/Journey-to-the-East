using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField]
    private Transform target;
    private float offset;
    [SerializeField]
    private float smoothTime;
    private Vector3 velocity;

    private void Start()
    {
        offset = transform.position.z;
    }

    // Update is called once per frame
    void Update()
    {
        //var mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        //var bounds = new Bounds(target.position, Vector3.zero);
        //bounds.Encapsulate(target.position);
        //bounds.Encapsulate(mousePosition);

        //Vector3 smooth = Vector3.SmoothDamp(transform.position, bounds.center, ref velocity, smoothTime);
        //smooth.z = offset;
        //transform.position = smooth;
        //velocity.z = 0;
        Vector3 point = GetComponentInChildren<Camera>().WorldToViewportPoint(target.position);
        Vector3 delta = target.position - GetComponentInChildren<Camera>().ViewportToWorldPoint(new Vector3(0.5f, 0.5f, point.z));
        Vector3 destination = transform.position + delta;
        transform.position = Vector3.SmoothDamp(transform.position, destination, ref velocity, smoothTime);
    }
}
