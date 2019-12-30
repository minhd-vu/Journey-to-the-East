using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField] private Transform bar = null;

    private float percent = 1f;
    public float Percent
    {
        get { return percent; }
        set
        {
            bar.localScale = new Vector3(Mathf.Clamp(value, 0f, 1f), bar.localScale.y, bar.localScale.z);
        }
    }
}
