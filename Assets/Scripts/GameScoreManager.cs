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
    // Public property to set score
    public int Score = 0;

    // Start method
    private void Start()
    {
        if (ScoreText == null) { Debug.Log("ERR: GameScoreManager ====== Start() ====== Score Text Not Found"); return; }
        if (FinalScoreText == null) { Debug.Log("ERR: GameScoreManager ====== Start() ====== Final Score Text Not Found Not Found"); return; }
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
    }
}
