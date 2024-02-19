/* ------------------------------------------------------------------------------
GameScoreManager Class
  This script handles
  1> Managing game score
--------------------------------------------------------------------------------*/

using System;
using TMPro;
using Unity.VisualScripting;
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
    public int HighScore;
    private string HighScoreKey = "HighScore";

    // Start method
    private void Start()
    {
        if (ScoreText == null) { Debug.Log("ERR: GameScoreManager ====== Start() ====== Score Text Not Found"); return; }
        if (FinalScoreText == null) { Debug.Log("ERR: GameScoreManager ====== Start() ====== Final Score Text Not Found Not Found"); return; }

        HighScore = GetHighScore();
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
        EndMenuHighScoreText.enabled = false;
    }

    public void SetHighScore(int value)
    {
        PlayerPrefs.SetInt(HighScoreKey, value);
    }

    public int GetHighScore()
    {
        return PlayerPrefs.GetInt(HighScoreKey);
    }

    public void SetStartMenuHighScoreText()
    {
        StartMenuHighScoreText.GetComponent<TextMeshProUGUI>().text = "HighScore: " + HighScore.ToString();
    }

    public void SetEndMenuHighScoreText()
    {
        FinalScoreText.enabled = false;
        EndMenuHighScoreText.enabled = true;
        EndMenuHighScoreText.GetComponent<TextMeshProUGUI>().text = "New HighScore: " + Score.ToString();
    }

    internal void NewHighScore()
    {
        SetHighScore(Score);
        SetEndMenuHighScoreText();
    }
}
