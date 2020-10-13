using System;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class PlayerController : MonoBehaviour
{
    [Header("General")]
    [Tooltip("In ms-1")][SerializeField] private float controlSpeed = 20f;
    [Tooltip("In ms-1")] [SerializeField] private float xRange = 5f;
    [Tooltip("In ms-1")] [SerializeField] private float yRange = 3f;
    [SerializeField] GameObject[] guns;

    [Header("Screen-position Based")]
    [SerializeField] private float positionPitchFactor = -5f;
    [SerializeField] private float controlPitchFactor = -20f;
    
    [Header("Control-throw based")]
    [SerializeField] private float positionYawFactor = 5f;
    [SerializeField] private float controlRollFactor = -20f;
    
    private float xThrow, yThrow;
    private bool isControlEnabled = true;

    void Update()
    {
        if (isControlEnabled)
        {
            ProcessTranslation();
            ProcessRotation();
            ProcessFiring();
        }
    }

    private void OnPlayerDeath() // called by string reference
    {
        isControlEnabled = false;
    }
    
    private void ProcessRotation()
    {
        float pitchDueToPosition = transform.localPosition.y * positionPitchFactor;
        float pitchDueToControlThrow = yThrow * controlPitchFactor;
        float pitch = pitchDueToPosition + pitchDueToControlThrow;

        float yaw = transform.localPosition.x * positionYawFactor;

        float roll = xThrow * controlRollFactor;
        transform.localRotation = Quaternion.Euler(pitch, yaw,roll);
    }

    private void ProcessTranslation()
    {
        xThrow = CrossPlatformInputManager.GetAxis("Horizontal");
        yThrow = CrossPlatformInputManager.GetAxis("Vertical");
        
        float xOffset = xThrow * controlSpeed * Time.deltaTime;
        float yOffset = yThrow * controlSpeed * Time.deltaTime;

        float rawXPos = transform.localPosition.x + xOffset;
        float rawYPos = transform.localPosition.y + yOffset;
        
        float clampedXPos = Mathf.Clamp(rawXPos, -xRange, xRange);
        float clampedYPos = Mathf.Clamp(rawYPos, -yRange, yRange);

        transform.localPosition = new Vector3(clampedXPos, clampedYPos, transform.localPosition.z);
    }

    void ProcessFiring()
    {
        if (CrossPlatformInputManager.GetButton("Fire"))
        {
            SetGunsActive(true);
        }
        else
        {
            SetGunsActive(false);
        }
    }

    private void SetGunsActive(bool isActive)
    {
        foreach (GameObject gun in guns) // care may affect death FX
        {
            var emissionModule = gun.GetComponent<ParticleSystem>().emission;
            emissionModule.enabled = isActive;
            //gun.SetActive(isActive);
        }
    }
}
