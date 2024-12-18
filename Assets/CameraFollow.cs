using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform player; // The player's transform
    public float smoothSpeed = 0.125f; // Smoothing factor for camera movement
    public Vector2 bufferZone = new Vector2(1f, 1f); // Horizontal and vertical buffer zone around the player

    private Vector3 targetPosition; // The target position the camera should move towards

    void LateUpdate()
    {
        if (player == null) return;

        // Get the current camera position and player's position
        Vector3 cameraPosition = transform.position;
        Vector3 playerPosition = player.position;

        // Check if the player is outside the horizontal buffer zone
        if (Mathf.Abs(playerPosition.x - cameraPosition.x) > bufferZone.x)
        {
            // Update the target X position
            targetPosition.x = playerPosition.x;
        }
        else
        {
            // Maintain current X position
            targetPosition.x = cameraPosition.x;
        }

        // Check if the player is outside the vertical buffer zone
        if (Mathf.Abs(playerPosition.y - cameraPosition.y) > bufferZone.y)
        {
            // Update the target Y position
            targetPosition.y = playerPosition.y;
        }
        else
        {
            // Maintain current Y position
            targetPosition.y = cameraPosition.y;
        }

        // Maintain the current Z position of the camera
        targetPosition.z = cameraPosition.z;

        // Smoothly move the camera towards the target position
        transform.position = Vector3.Lerp(cameraPosition, targetPosition, smoothSpeed);
    }
}
