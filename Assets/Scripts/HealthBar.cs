using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField] private Transform bar = null;
    [SerializeField] private float healthBarTimer = 5f;
    private float healthBarTime;

    private float target;
    private float percent = 1f;
    [SerializeField] private float lerpSpeed = 10f;
    public float Percent
    {
        get { return percent; }
        set
        {
            gameObject.SetActive(true);
            healthBarTime = 0f;
            target = Mathf.Clamp(value, 0f, 1f);
        }
    }

    private void Start()
    {
        target = percent;
        healthBarTime = 0f;
        gameObject.SetActive(false);
    }

    // Remove the health bar from view after a certain period of time.
    private void Update()
    {
        if (bar.localScale.x != target)
        {
            bar.localScale = new Vector3(Mathf.Lerp(bar.localScale.x, target, Time.deltaTime * lerpSpeed), bar.localScale.y, bar.localScale.z);
        }

        if ((healthBarTime += Time.deltaTime) >= healthBarTimer)
        {
            healthBarTime = 0f;
            gameObject.SetActive(false);
        }
    }
}
