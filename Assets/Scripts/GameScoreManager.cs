/* ------------------------------------------------------------------------------
GameScoreManager Class
  This script handles
  1> Managing game score
--------------------------------------------------------------------------------*/
using TMPro;
using UnityEngine;

public class GameScoreManager : MonoBehaviour
{
    // Public Property set from editor, to reference score text tmp_text game object in game HUD
    [SerializeField] public TMP_Text ScoreText;
    // Public Property set from editor, to reference final score text tmp_text game object in end game UI
    [SerializeField] public TMP_Text FinalScoreText;
    [SerializeField] public TMP_Text StartMenuHighScoreText;
    // Public Property set from editor, to reference final score text tmp_text game object in end game UI
    [SerializeField] public TMP_Text EndMenuHighScoreText;
    // Public property to set score
    public int Score = 0;
    // Public property to set HighScore
    // Value is fetched from Player Prefs
    public int HighScore;
    // Reference to high score key in playerprefs
    private string HighScoreKey = "HighScore";

    // Start method
    private void Start()
    {
        // Check if ScoreText or FinalScoreText reference is null
        if (ScoreText == null) { Debug.Log("ERR: GameScoreManager ====== Start() ====== Score Text Not Found"); return; }
        if (FinalScoreText == null) { Debug.Log("ERR: GameScoreManager ====== Start() ====== Final Score Text Not Found Not Found"); return; }
        // Assign high score
        HighScore = GetHighScore();
        // Display high score in start menu
        SetStartMenuHighScoreText();
    }

    // SetScore method
    public void SetScore(int score)
    {
        // Set score property to parameter value
        Score = score;
        // Update score text value with new score
        ScoreText.GetComponent<TextMeshProUGUI>().text = "Score: " + Score.ToString();
    }

    // SetFinalScore method
    public void SetFinalScore()
    {
        // Set final score text value with final score
        FinalScoreText.GetComponent<TextMeshProUGUI>().text = "Score: " + Score.ToString();
        // Disable game over high score text to false
        EndMenuHighScoreText.enabled = false;
    }

    // SetHighScore method
    public void SetHighScore(int value)
    {
        // Set highscore to player prefs high score key
        PlayerPrefs.SetInt(HighScoreKey, value);
    }

    // GetHighScore method
    public int GetHighScore()
    {
        // Get highscore from player prefs high score key
        return PlayerPrefs.GetInt(HighScoreKey);
    }


    // SetStartMenuHighScoreText method
    public void SetStartMenuHighScoreText()
    {
        // Get highscore and display it in the start menu
        StartMenuHighScoreText.GetComponent<TextMeshProUGUI>().text = "HighScore: " + HighScore.ToString();
    }

    // SetEndMenuHighScoreText method
    public void SetEndMenuHighScoreText()
    {
        // If high score attained
        // Set final score text reference to false
        FinalScoreText.enabled = false;
        // Set end menu highscore text reference to true
        EndMenuHighScoreText.enabled = true;
        // Set highscore and display it in the end menu
        EndMenuHighScoreText.GetComponent<TextMeshProUGUI>().text = "New HighScore: " + Score.ToString();
    }

    // NewHighScore method
    internal void NewHighScore()
    {
        // Set highscore value saved in player prefs to new highscore value 
        SetHighScore(Score);
        // Set the end menu to include highscore and not final score text display
        SetEndMenuHighScoreText();
    }
}
