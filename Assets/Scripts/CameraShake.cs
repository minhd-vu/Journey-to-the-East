using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.Events;

public class CameraShake : MonoBehaviour
{
    public float duration = 0.3f;          // Time the Camera Shake effect will last
    public float amplitude = 1.2f;         // Cinemachine Noise Profile Parameter
    public float frequency = 2.0f;         // Cinemachine Noise Profile Parameter

    private float shakeElapsedTime = 0f;

    // Cinemachine Shake
    private CinemachineVirtualCamera virtualCamera;
    private CinemachineBasicMultiChannelPerlin virtualCameraNoise;

    // Use this for initialization
    void Start()
    {
        // Get Virtual Camera Noise Profile
        virtualCamera = GetComponent<CinemachineVirtualCamera>();
        virtualCameraNoise = virtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
    }

    // Update is called once per frame
    void Update()
    {
        // TODO: Replace with your trigger
        if (Input.GetKey(KeyCode.E))
        {
            shakeElapsedTime = duration;
        }

        // If the Cinemachine componet is not set, avoid update
        if (virtualCamera != null && virtualCameraNoise != null)
        {
            // If Camera Shake effect is still playing
            if (shakeElapsedTime > 0)
            {
                // Set Cinemachine Camera Noise parameters
                virtualCameraNoise.m_AmplitudeGain = amplitude;
                virtualCameraNoise.m_FrequencyGain = frequency;

                // Update Shake Timer
                shakeElapsedTime -= Time.deltaTime;
            }
            else
            {
                // If Camera Shake effect is over, reset variables
                virtualCameraNoise.m_AmplitudeGain = 0f;
                shakeElapsedTime = 0f;
            }
        }
    }
}

