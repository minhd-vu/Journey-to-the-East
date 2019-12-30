using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField] private Transform bar = null;
    [SerializeField] private float healthBarTimer = 5f;
    private float healthBarTime;

    private float percent = 1f;
    public float Percent
    {
        get { return percent; }
        set
        {
            gameObject.SetActive(true);
            healthBarTime = 0f;
            bar.localScale = new Vector3(Mathf.Clamp(value, 0f, 1f), bar.localScale.y, bar.localScale.z);
        }
    }

    private void Start()
    {
        healthBarTime = 0f;
        gameObject.SetActive(false);
    }

    // Remove the health bar from view after a certain period of time.
    private void Update()
    {
        if ((healthBarTime += Time.deltaTime) >= healthBarTimer)
        {
            healthBarTime = 0f;
            gameObject.SetActive(false);
        }
    }
}
