using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyAnimation : MonoBehaviour
{
    [SerializeField] private GameObject parent = null;

    // Start is called before the first frame update
    void Start()
    {
        // Get the animation's duration.
        float duration = GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).length;

        // Destroy the animation after it reaches the last frame.
        if (parent != null)
        {
            Destroy(parent, duration);
        }
        else
        {
            Destroy(gameObject, duration);
        }
    }
}
