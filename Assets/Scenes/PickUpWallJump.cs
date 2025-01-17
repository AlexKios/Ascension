using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpWallJump : MonoBehaviour
{
    public string playerTag = "Player"; // Tag to identify the player
    public string message = "Item Collected!"; // Optional message or feedback
    public bool destroyOnPickup = true; // Should the item disappear after pickup?

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag(playerTag))
        {
            // Access the player's script to update the value
            HeroKnight player = other.GetComponent<HeroKnight>();
            if (player != null)
            {
                player.m_hasWallJump = true; // Example boolean in player script
                Debug.Log(message);
            }
            else
            {
                Debug.LogWarning("HeroKnight component not found on the player!");
            }

            // Destroy the item if needed
            if (destroyOnPickup)
            {
                Destroy(gameObject);
            }
        }
    }
}
