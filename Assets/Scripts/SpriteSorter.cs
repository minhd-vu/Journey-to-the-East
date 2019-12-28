using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteSorter : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    private const int scale = -100;

    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        spriteRenderer.sortingOrder = (int)(spriteRenderer.transform.position.y * scale);
    }

    public static void UpdateSpriteRenderer(SpriteRenderer sr)
    {
        sr.sortingOrder = (int)(sr.transform.position.y * scale);
    }
}
