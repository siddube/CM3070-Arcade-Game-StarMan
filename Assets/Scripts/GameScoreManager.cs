using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameScoreManager : MonoBehaviour
{
    [SerializeField] public TMP_Text ScoreText;

    [SerializeField] public TMP_Text FinalScoreText;

    public int Score = 0;

    public void SetScore(int score)
    {
        Score = score;
        ScoreText.GetComponent<TextMeshProUGUI>().text = "Score: " + Score.ToString();
    }

    public void SetFinalScore()
    {
        FinalScoreText.GetComponent<TextMeshProUGUI>().text = "Score: " + Score.ToString();
    }


}
