using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteSorter : MonoBehaviour
{
    public SpriteRenderer parentSpriteRenderer = null;
    public int sortingOrderOffset = 0;

    private SpriteRenderer spriteRenderer;
    private const int scale = -100;

    // Start is called before the first frame update
    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        /*
        if (parentSpriteRenderer != null)
        {
            spriteRenderer.sortingOrder = parentSpriteRenderer.sortingOrder + sortingOrderOffset;
        }
        else
        {
            spriteRenderer.sortingOrder = (int)(spriteRenderer.transform.position.y * scale);
        }
        */
    }

    public static void UpdateSpriteRenderer(SpriteRenderer sr)
    {
        sr.sortingOrder = (int)(sr.transform.position.y * scale);
    }
}
