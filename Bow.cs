using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Bow : MonoBehaviour
{
    public GameObject arrowPrefab;
    public int maxAmmo = 5;
    public float ammoRegenRate = 1f; // Ammo regenerates per second
    public Slider rangeSlider; // UI slider to adjust the launch angle
    public float launchSpeed = 10f; // Speed at which arrows are launched

    private int currentAmmo;
    private float launchAngle = 45f; // Default launch angle in degrees

    void Start()
    {
        currentAmmo = maxAmmo;
        rangeSlider.onValueChanged.AddListener(AdjustLaunchAngle);
    }

    void Update()
    {
        RegenerateAmmo();
        if (Input.GetKeyDown(KeyCode.F) && currentAmmo > 0)
        {
            FireArrow();
            currentAmmo--;
        }
    }

    void FireArrow()
    {
        // Calculate launch direction and rotatiom
        Quaternion newRotation = Quaternion.Euler(0, 0, launchAngle);

        // Instantiate the arrow with the correct rotation
        GameObject arrow = Instantiate(arrowPrefab, transform.position, newRotation);
        Rigidbody2D rb = arrow.GetComponent<Rigidbody2D>();
        Arrow arrowScript = arrow.GetComponent<Arrow>();
        if (arrowScript != null)
        {
            // Pass the launch angle to the arrow
            arrowScript.SetLaunchAngle(launchAngle);
        }

        if (rb != null)
        {
            Vector2 launchDirection = new Vector2(Mathf.Cos(launchAngle * Mathf.Deg2Rad), Mathf.Sin(launchAngle * Mathf.Deg2Rad));
            rb.velocity = launchDirection * launchSpeed;
        }
    }

    public void AdjustLaunchAngle(float newAngle)
    {
        launchAngle = newAngle;
    }

    void RegenerateAmmo()
    {
        if (currentAmmo < maxAmmo)
        {
            currentAmmo += Mathf.FloorToInt(ammoRegenRate * Time.deltaTime);
            currentAmmo = Mathf.Min(currentAmmo, maxAmmo);
        }
    }
}