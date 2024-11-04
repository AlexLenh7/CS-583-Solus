using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Timer : MonoBehaviour
{
    public TextMeshProUGUI clockText;  // Reference to the TextMeshPro object
    public float elapsedTime = 0f;    // Variable to track elapsed time
    public PlayerMovement playerHealth;

    private void Update()
    {
        if (playerHealth.isDying)
        {
            gameObject.SetActive(false); // Disable the timer GameObject
        }

        // Update elapsed time
        elapsedTime += Time.deltaTime;

        // Format the time into minutes and seconds
        int minutes = Mathf.FloorToInt(elapsedTime / 60f);
        int seconds = Mathf.FloorToInt(elapsedTime % 60f);

        // Update the clock text with formatted time
        clockText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }
}
