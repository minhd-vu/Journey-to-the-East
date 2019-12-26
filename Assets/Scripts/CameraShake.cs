using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.Events;

public class CameraShake : MonoBehaviour
{
    public static CameraShake instance;

    // Cinemachine Shake
    private CinemachineVirtualCamera virtualCamera;
    private CinemachineBasicMultiChannelPerlin virtualCameraNoise;
    private Coroutine active;

    // Use this for initialization
    void Start()
    {
        // Get Virtual Camera Noise Profile
        virtualCamera = GetComponent<CinemachineVirtualCamera>();
        virtualCameraNoise = virtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        instance = this;
    }

    public void ShakeOnce(float amplitude, float frequency, float duration)
    {
        active = StartCoroutine(Shake(amplitude, frequency, duration));
    }

    private IEnumerator Shake(float amplitude, float frequency, float duration)
    {
        // If the Cinemachine componet is not set, avoid update
        if (virtualCamera != null && virtualCameraNoise != null)
        {
            float timer = duration;

            // If Camera Shake effect is still playing
            while (timer > 0)
            {
                // Set Cinemachine Camera Noise parameters
                virtualCameraNoise.m_AmplitudeGain = amplitude;
                virtualCameraNoise.m_FrequencyGain = frequency;

                // Update Shake Timer
                timer -= Time.deltaTime;
                yield return null;
            }

            // If Camera Shake effect is over, reset variables
            virtualCameraNoise.m_AmplitudeGain = 0f;
            virtualCameraNoise.m_FrequencyGain = 0f;
        }
    }
}

