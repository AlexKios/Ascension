using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    public float currentTime = 0f; // Current playtime
    public float bestTime = 0f;   // Best time
    private bool isRunning = false;

    public TextMeshProUGUI currentTimeText;
    public TextMeshProUGUI bestTimeText;

    void Start()
    {
        // Load the best time from PlayerPrefs
        bestTime = PlayerPrefs.GetFloat("BestTime", float.MaxValue);

        if (bestTime == float.MaxValue)
            bestTimeText.text = "Best Time: None";
        else
            bestTimeText.text = $"Best Time: {bestTime:F2}s";

        currentTimeText.text = "Current Time: 0.00s";

        Debug.Log($"Loaded Best Time: {bestTime}");

        StartTimer();
    }

    void Update()
    {
        if (isRunning)
        {
            currentTime += Time.deltaTime; // Increment time
            currentTimeText.text = $"Current Time: {currentTime:F2}s";
        }
        bestTimeText.text = $"Best Time: {bestTime:F2}s";
    }

    public void StartTimer()
    {
        isRunning = true;
        currentTime = 0f;
    }

    public void StopTimer()
    {
        isRunning = false;

        if (currentTime < bestTime)
        {
            bestTime = currentTime;
            PlayerPrefs.SetFloat("BestTime", bestTime);
            PlayerPrefs.Save();

            // Update the best time text
            bestTimeText.text = $"Best Time: {bestTime:F2}s";
        }
    }

    public void ClearBestTime()
    {
        // Deletes the key for best time from PlayerPrefs
        PlayerPrefs.DeleteKey("BestTime");

        // Save changes to PlayerPrefs
        PlayerPrefs.Save();

        // Update UI to reflect that the best time is cleared
        bestTimeText.text = "Best Time: None";
        Debug.Log("Best time cleared.");
    }
}
