using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

// Handles the functionality of the play again button
public class PlayAgainButton : MonoBehaviour {
    // Called when play again button is pressed. Loads the game again.
    public void playAgain() {
        SceneManager.LoadScene("MazeRunner");
    }
}
