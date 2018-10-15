using UnityEngine;
using UnityEngine.UI;
using System;

// Handles keep track off the time taken to complete the maze
public class Timer : MonoBehaviour {
    private float startTime;
    public Text displayTime;

    // Saves current time
    void Start () {
        startTime = Time.time;
    } 

    void Update() {
        displayTime.text = "Time: " + Math.Round(Time.time - startTime, 2);
    }

    // Finds the duration of the time needed to beat the maze and saves it
    void saveTimeScore() {
        float duration = Time.time - startTime;
        PlayerPrefs.SetFloat("TimeScore", (float) Math.Round(duration, 2));
    }
}
