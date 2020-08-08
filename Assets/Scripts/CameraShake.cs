using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    // Transform of the camera to shake. Grabs the gameObject's transform
    // if null.
    public Transform mainCam;

    // How long the object should shake for.
    public float shakeDuration = 0f;

    // Amplitude of the shake. A larger value shakes the camera harder.
    public float shakeAmount = 0.0f;
    public float decreaseFactor = 1.0f;

    Vector3 originalPos;

    void Awake()
    {
        mainCam = GetComponent<Transform>();
    }

    void Start()
    {
        // originalPos = mainCam.localPosition;
    }

    void Update()
    {
        // if (shakeDuration > 0)
        // {
        //     mainCam.localPosition = originalPos + Random.insideUnitSphere * shakeAmount;

        //     shakeDuration -= Time.deltaTime * decreaseFactor;
        // }
        // else
        // {
        //     shakeDuration = 0f;
        //     mainCam.localPosition = originalPos;
        // }
    }

    public void Shake(float amt, float length)
    {
        shakeAmount = amt;
        InvokeRepeating("BeginShake", 0, 0.01f);
        Invoke("StopShake", length);
    }

    void BeginShake()
    {
        if (shakeAmount > 0)
        {
            Vector3 camPos = mainCam.transform.position;
            float offsetX = Random.value * shakeAmount * 2 - shakeAmount;
            float offsetY = Random.value * shakeAmount * 2 - shakeAmount;
            camPos.x += offsetX;
            camPos.y += offsetY;
            mainCam.transform.position = camPos;
        }
    }

    void StopShake()
    {
        CancelInvoke("BeginShake");
        mainCam.transform.localPosition = Vector3.zero;
    }
}
