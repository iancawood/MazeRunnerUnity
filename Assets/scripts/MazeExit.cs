using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

// Handles all functionality relating to the game ending
public class MazeExit : MonoBehaviour {
    // Called when the player reaches the end of the maze
    void OnTriggerEnter() {
        GameObject.Find("Timer").SendMessage("saveTimeScore");
        SceneManager.LoadScene("Menu");
    }
}
