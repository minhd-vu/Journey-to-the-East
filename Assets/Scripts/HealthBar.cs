using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField] private Transform target = null;
    [SerializeField] private Vector3 offset = Vector3.zero;
    [SerializeField] private Image image = null;

    // Update is called once per frame
    void Update()
    {
        transform.position = target.position + offset;
    }
}
