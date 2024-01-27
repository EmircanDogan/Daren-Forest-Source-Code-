using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Pause : MonoBehaviour
{
    void Update()
    {
        // Check if the Escape key is pressed
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            // Load the Options menu scene
            SceneManager.LoadScene("Main"); // Replace "OptionsSceneName" with the actual name of your options menu scene
        }
    }
}
