/* ------------------------------------------------------------------------------
<PlayerCombatManager Class>
  This script handles
  1> Accepting keyboard input for the firing a bullet from the space ship
  2> Takes damage from collision
<PlayerMovementManager Class>
--------------------------------------------------------------------------------*/

using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class PlayerCombatManager : MonoBehaviour
{
  // Property set from editor, to reference health bar
  [SerializeField] public HealthBar HealthBar;
  // Property set from editor, to reference health bar
  [SerializeField] GameScoreManager ScoreManager;

  // Event emitted when player presses spacebar to shoot
  // Event emmitted on shooting bullets with spacebar
  public UnityEvent OnShootBullets;
  // Event emmitted on player space ship getting destroyed
  public UnityEvent OnPlayerShipDestroyed;

  // Private property to set current health
  // Initially set to full 100 points
  private int m_currentHealth = 100;
  // Public current health property other classes can get and set
  public int CurrentHealth { get => m_currentHealth; set => m_currentHealth = value; }
  // Private property to set game score
  // Initially set to 0
  private int m_score = 0;
  // Public score property other classes can get and set
  public int Score { get => m_score; set => m_score = value; }

  // Start method
  private void Start()
  {
    // Set health to 100
    m_currentHealth = 100;
    // Set score to 0
    m_score = 0;
    // Set health to maximum health on health bar
    if (!HealthBar) { Debug.Log("ERR: PlayerCombatManager ====== UpdateScore() ====== HealthBar not setup"); return; }
    HealthBar.SetMaxHealth(m_currentHealth);
  }

  // OnShoot method is called by player input
  public void OnShoot(InputValue input)
  {
    // Invoke event to fire bullets
    // The bullet spawner method is triggered in the editor
    if (OnShootBullets == null) { Debug.Log("ERR: PlayerCombatManager ====== OnShoot() ====== On shoot bullets event not setup"); return; }
    OnShootBullets.Invoke();
  }

  // Public method to update score
  public void UpdateScore(int currentScore, int pointsToAdd)
  {
    if (!ScoreManager) { Debug.Log("ERR: PlayerCombatManager ====== UpdateScore() ====== ScoreManager not setup"); return; }
    // Set score property
    m_score = currentScore + pointsToAdd;
    // Call set score method on score manager
    ScoreManager.SetScore(m_score);
  }

  // Public method to take damage
  public void TakeDamage(int currentHealth, int damageValue)
  {
    if (!HealthBar) { Debug.Log("ERR: PlayerCombatManager ====== UpdateScore() ====== HealthBar not setup"); return; }
    // Set health to equal difference of current health and damage value inflicted 
    m_currentHealth = currentHealth - damageValue;
    // Set health to current health on health bar
    HealthBar.SetHealth(m_currentHealth);
    // Call is ship destroyed method to check if health is 0 or negative
    // This implies the ship is damaged
    IsShipDamaged();
  }

  // Is ship destroyed method
  private void IsShipDamaged()
  {
    // Check if current health is lesser than 0 or negative
    if (m_currentHealth <= 0)
    {
      // If yes then invoke on player ship destoryed event
      OnPlayerShipDestroyed.Invoke();
      // Destroy the game object
      Destroy(this.gameObject);
    }
  }
}
