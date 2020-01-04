using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.UI;

public class HurtEffect : MonoBehaviour
{
    [SerializeField] private PlayerController player = null;
    private ColorGrading colorGrading;
    private PostProcessVolume volume;
    [SerializeField] private Image image = null;

    // Start is called before the first frame update
    void Start()
    {
        volume = gameObject.GetComponent<PostProcessVolume>();
        volume.profile.TryGetSettings(out colorGrading);
        player = player.GetComponent<PlayerController>();
    }

    // Update is called once per frame
    void LateUpdate()
    {
        colorGrading.enabled.value = true;
        float saturation = (player.maxHealth - player.Health) / player.maxHealth * -100f;
        colorGrading.saturation.value = Mathf.Clamp(saturation, -100f, 0f);
        image.color = new Color(image.color.r, image.color.g, image.color.b, Mathf.Clamp((player.maxHealth - player.Health) / player.maxHealth, 0, 1f) * 0.1f);
    }
}
