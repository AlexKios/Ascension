using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinTrigger : MonoBehaviour
{
    public GameObject winScreen; // Reference to the Panel GameObject
    private Timer timer;

    private void Start()
    {
        // Find and reference the Timer script (assuming it's attached to a GameObject named "GameManager")
        timer = GameObject.Find("Timer").GetComponent<Timer>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Check if the colliding object is the player
        if (collision.CompareTag("Player"))
        {
            timer.StopTimer();

            // Show the win screen
            winScreen.SetActive(true);

            // (Optional) Pause the game
            Time.timeScale = 0f;
        }
    }
}
