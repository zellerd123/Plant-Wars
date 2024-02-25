using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public Transform player; // Reference to the player's Transform component
    public Vector3 offset = new Vector3(0f, 0f, -10f); // Adjust this to set the desired camera offset
    public float smoothSpeed = 0.125f; // Adjust this to control camera smoothness
    public bool enableBounds = false; // Toggle to enable camera bounds
    public Vector2 minBounds; // Minimum camera bounds
    public Vector2 maxBounds; // Maximum camera bounds
    public float yOffset = 0f;
    private Vector3 velocity = Vector3.zero;

    private void LateUpdate()
    {
        if (player != null)
        {
            // Calculate the desired position for the camera based on the player's position and offset.
            Vector3 desiredPosition = new Vector3(player.position.x, transform.position.y + yOffset, transform.position.z);

            // Optionally, clamp the camera within specified bounds.
            if (enableBounds)
            {
                desiredPosition.x = Mathf.Clamp(desiredPosition.x, minBounds.x, maxBounds.x);
                desiredPosition.y = Mathf.Clamp(desiredPosition.y, minBounds.y, maxBounds.y);
            }

            // Smoothly interpolate to the desired position.
            Vector3 smoothedPosition = Vector3.SmoothDamp(transform.position, desiredPosition, ref velocity, smoothSpeed);

            // Set the camera's position to the smoothed position.
            transform.position = smoothedPosition;
        }
    }
}
