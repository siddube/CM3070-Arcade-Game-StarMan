/* ------------------------------------------------------------------------------
<PlayerCombatManager Class>
  This script handles
  1> Accepting keyboard input for the firing a bullet from the space ship
  2> Takes damage from collision
<PlayerMovementManager Class>
--------------------------------------------------------------------------------*/
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class PlayerCombatManager : MonoBehaviour
{
  [SerializeField] public HealthBar HealthBar;

  [SerializeField] public TMP_Text ScoreText;
  //Event emitted when player presses spacebar to shoot
  public UnityEvent ShootBullets;
  public UnityEvent PlayerShipDestroyed;

  private int m_currentHealth = 100;
  public int CurrentHealth { get => m_currentHealth; set => m_currentHealth = value; }
  private int m_score = 0;
  public int Score { get => m_score; set => m_score = value; }

  private void Start()
  {
    CurrentHealth = 100;
    Score = 0;
    HealthBar.SetMaxHealth(CurrentHealth);
  }
  public void OnShoot(InputValue input)
  {
    // OnShoot method is called by player input
    // Invoke event to fire bullets
    // The bullet spawner method is triggered in the editor 
    ShootBullets.Invoke();
  }

  public void UpdateScore(int currentScore, int pointsToAdd)
  {
    Score = currentScore + pointsToAdd;
    ScoreText.GetComponent<TextMeshProUGUI>().text = "Score: " + Score.ToString();
  }

  public void TakeDamage(int currentHealth, int damageValue)
  {
    CurrentHealth = currentHealth - damageValue;
    HealthBar.SetHealth(CurrentHealth);
    ComputeShipDamage();
  }

  private void ComputeShipDamage()
  {
    if (m_currentHealth <= 0)
    {
      PlayerShipDestroyed.Invoke();
      Destroy(this.gameObject);
    }
  }
}
