using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class GameTimer : MonoBehaviour
{
    [SerializeField] public PlayerCombatManager playerCombatManager;
    [SerializeField] public TMP_Text ScoreText;
    [SerializeField] public TMP_Text LevelTimeText;
    [SerializeField] public float LevelTime;
    public UnityEvent OnTimeUp;
    private float m_timer;

    private void Awake()
    {
        m_timer = LevelTime;
    }

    private void Update()
    {
        LevelTimeText.GetComponent<TextMeshProUGUI>().text = "Time: " + ((int)m_timer).ToString();
        TickTimer(Time.deltaTime);
    }

    private void TickTimer(float dt)
    {
        m_timer -= 1 * dt;
        CheckOutOfTime();
    }

    private void CheckOutOfTime()
    {

        if (m_timer <= 0f)
        {

            int gameOverScore = playerCombatManager.Score;
            ScoreText.GetComponent<TextMeshProUGUI>().text = "Score: " + gameOverScore.ToString();
            OnTimeUp.Invoke();
        }
    }
}
