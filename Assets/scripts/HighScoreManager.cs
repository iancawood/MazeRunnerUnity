using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

// Handles all functionality related to high scores included loading, saving, sorting, etc.
public class HighScoreManager : MonoBehaviour {
    public Text scoreOneName;
    public Text scoreOneTime;
    public Text scoreTwoName;
    public Text scoreTwoTime;
    public Text scoreThreeName;
    public Text scoreThreeTime;

    public GameObject highScorePanel;
    public GameObject inputScorePanel;

    public InputField nameInputField;

    class Score {
        public string name;
        public float time;

        public Score(string scoreName, float scoreTime) {
            name = scoreName;
            time = scoreTime;
        }
    }

    List<Score> highScores = new List<Score>();
    
    // Load high scores into memory
    void Start () {
        loadHighScores();

        if (highScores.Count == 0) {
            addDefaultScores();
        }

        highScorePanel.SetActive(false);
    }

    // Add default scores if it's the first time the game is played
    void addDefaultScores() {
        highScores.Add(new Score("AAA", 15));
        highScores.Add(new Score("BBB", 60));
        highScores.Add(new Score("CCC", 300));
    }

    // Display the high score information in the UI
    void displayScores() {
        scoreOneName.text = highScores[0].name;
        scoreOneTime.text = highScores[0].time.ToString() + " seconds";
        scoreTwoName.text = highScores[1].name;
        scoreTwoTime.text = highScores[1].time.ToString() + " seconds";
        scoreThreeName.text = highScores[2].name;
        scoreThreeTime.text = highScores[2].time.ToString() + " seconds";
    }

    // Called when the submit score button is clicked
    public void submitScore() {
        Score newScore = new Score(nameInputField.text, PlayerPrefs.GetFloat("TimeScore"));

        sortScores(newScore);
        displayScores();

        highScorePanel.SetActive(true);
        inputScorePanel.SetActive(false);

        saveHighScores();
    }

    // Determines if the newly submitted score is better than existing high scores
    void sortScores(Score newScore) {
        for (int i = 0; i < highScores.Count; i++) {
            if (newScore.time < highScores[i].time) {
                highScores.Remove(highScores[highScores.Count - 1]);
                highScores.Insert(i, newScore);
                break;
            }
        }
    } 

    // Naively serializes the high scores into player prefs so that they may be loaded later
    void loadHighScores() {
        string highScoresStr = PlayerPrefs.GetString("HighScores");

        if (highScoresStr == "") {
            return;
        }

        string[] tokens = highScoresStr.Split(',');

        for (int i = 0; i < tokens.Length; i++) {
            string[] highScoreVars = tokens[i].Split('|');
            highScores.Add(new Score(highScoreVars[0], float.Parse(highScoreVars[1])));
        }
    }

    // Parses and loads the serialized high scores 
    void saveHighScores() {
        string highScoresStr = "";
        
        for (int i = 0; i < highScores.Count; i++) {
            highScoresStr += highScores[i].name + "|" + highScores[i].time.ToString();
            
            if (i != highScores.Count - 1) {
                highScoresStr += ",";
            }
        }

        PlayerPrefs.SetString("HighScores", highScoresStr);
    }
}
