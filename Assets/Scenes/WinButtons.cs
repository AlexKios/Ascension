using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WinButtons : MonoBehaviour
{
    public Timer timer;
    public void RestartGame()
    {
        Time.timeScale = 1f; // Resume time
        SceneManager.LoadScene(SceneManager.GetActiveScene().name); // Reload the current scene
    }

    public void QuitGame()
    {
        Application.Quit(); // Quit the application
        Debug.Log("Quit Game"); // This works only in a built version
    }

    public void ResetTimer()
    {
        timer.ClearBestTime();
    }
}
