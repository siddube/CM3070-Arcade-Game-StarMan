/* ------------------------------------------------------------------------------
PlayerCombatManager Class
  This script handles
  1> Accepting keyboard input for the firing a bullet from the space ship
  2> Takes damage from collision
  3) Update Score
  4) Check if player ship is destroyed
  5) Run Destroy particle and audio fx
  6) Destroy player ship
--------------------------------------------------------------------------------*/

using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class PlayerCombatManager : MonoBehaviour
{
  // Property set from editor, to reference health bar
  [SerializeField] public HealthBar HealthBar;
  // Property set from editor, to reference score manager
  [SerializeField] private GameScoreManager ScoreManager;
  // Property set from editor, to reference destroy player ship particle fx
  [SerializeField] private ParticleSystem m_destroyParticleSystemFx;
  // Property set from editor, to reference destory audio fx
  [SerializeField] private AudioSource m_destroyAudioFx;
  // Property set from editor, to reference libes sprite under health bar
  [SerializeField] private GameObject[] m_livesLeftSprites;
  // Private property to reference player game object
  private GameObject m_player;
  // private property to reference player movement
  private PlayerMovementManager m_playerMovementManager;
  // Private property that references collider on the player instance
  private Collider[] m_colliders;
  // Private property that references mesh renderer on the player instance
  private MeshRenderer m_meshRenderer;

  // Event emitted when player presses spacebar to shoot
  // Event emitted on shooting bullets with spacebar
  public UnityEvent OnShootBullets;
  // Event emitted on player space ship getting destroyed
  public UnityEvent OnPlayerShipDestroyed;
  // Event emitted on end of game 
  public UnityEvent OnEndGame;

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

  // Private properties to set lives left
  private int m_livesLeft = 3;
  // Private property to set destroy delay of player game object
  private float m_destroyDelay = 3f;

  // Start method
  private void Start()
  {
    // Set reference player game object
    m_player = GameObject.FindGameObjectWithTag("Player");
    // Set reference to player movement manager class 
    m_playerMovementManager = this.gameObject.GetComponent<PlayerMovementManager>();
    // Set health to 100
    m_currentHealth = 100;
    // Set score to 0
    m_score = 0;
    // Set reference colliders on the player spaceship 
    m_colliders = this.gameObject.GetComponents<Collider>();
    // Set refrence to mesh renderer on the player spaceship
    m_meshRenderer = this.gameObject.GetComponentInChildren<MeshRenderer>();
    // Check if health bar has been setup
    if (!HealthBar) { Debug.Log("ERR: PlayerCombatManager ====== Start() ====== HealthBar not setup"); return; }
    // Set health to maximum health on health bar
    HealthBar.SetMaxHealth(m_currentHealth);
    // Check if destory player has been setup
    if (!m_destroyParticleSystemFx) { Debug.Log("ERR: PlayerCombatManager ====== Start() ====== HealthBar not setup"); return; }
    // Stop destory particle system by default
    m_destroyParticleSystemFx.Stop();
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
    // Check if reference to score manager has been setup
    if (!ScoreManager) { Debug.Log("ERR: PlayerCombatManager ====== UpdateScore() ====== ScoreManager not setup"); return; }
    // Set score property with current score added with points to be added
    m_score = currentScore + pointsToAdd;
    // Call set score method on score manager
    ScoreManager.SetScore(m_score);
  }

  // Public method to take damage
  public void TakeDamage(int currentHealth, int damageValue)
  {
    // Check if health bar reference still exists
    if (!HealthBar) { Debug.Log("ERR: PlayerCombatManager ====== TakeDamage() ====== HealthBar not setup"); return; }
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
      // If yes, then reduce lives left property by 1
      m_livesLeft -= 1;
      // Check if player still has lives left
      if (m_livesLeft > 0)
      {
        // If player stil has life
        // Start coroutine to respawn player game object
        StartCoroutine("RespawnPlayerRoutine");
        // Remvoe HUD lives sprite under health bar
        m_livesLeftSprites[m_livesLeft - 1].SetActive(false);
      }
      // Else if all player lives are used
      else
      {
        // Invoke Ship Destoryed event
        OnPlayerShipDestroyed.Invoke();
        // Start coroutine to destroy player prefab
        StartCoroutine("DelayDestroyShipRoutine");
      }
    }
  }

  // RespawnPlayerRoutine coroutine
  IEnumerator RespawnPlayerRoutine()
  {
    // Couroutine when player still has life and player ship is respawning
    // Fetch all colliders on player ship
    foreach (Collider c in m_colliders)
    {
      // Set each to collision component to false
      // To avoid further collision
      c.enabled = false;
    }
    // Set mesh renderer to false to make playership invisible
    m_meshRenderer.enabled = false;
    // Check if m_destroyParticleSystemFx still exists
    if (!m_destroyParticleSystemFx) { Debug.Log("ERR: PlayerCombatManager ====== RespawnPlayerRoutine() ====== Destory Particle Fx not setup"); }
    // Play destroy particle system fx
    m_destroyParticleSystemFx.Play();
    // Check if destroy sound component still exists
    if (!m_destroyAudioFx) { Debug.Log("ERR: PlayerCombatManager ====== RespawnPlayerRoutine() ====== Destory Particle Fx not setup"); }
    // Play destroy sound fx
    m_destroyAudioFx.Play();
    // Stop player thrust particle system
    m_playerMovementManager.ThrustParticleSystem.Stop();
    // Set player ship velocity to zero
    m_player.GetComponent<Rigidbody>().velocity = Vector3.zero;
    // Wait for 3 seconds
    // Before resetting player ship properties for next life chance
    yield return new WaitForSeconds(m_destroyDelay);
    // Fetch all colliders on player ship
    foreach (Collider c in m_colliders)
    {
      // Set each to collision component to true
      // To turn on collisions again
      c.enabled = true;
    }
    // Re-enable mesh renderer to make player ship visible again
    m_meshRenderer.enabled = true;

    // Reset player ship position to default position
    m_playerMovementManager.transform.position = new Vector3(0f, 0f, 0f);
    // Reset current health property to 100 points again
    m_currentHealth = 100;
    // Check if health bar reference still exists
    if (!HealthBar) { Debug.Log("ERR: PlayerCombatManager ====== RespawnPlayerRoutine() ====== HealthBar not setup"); }
    // Set health bar to the current health value equal max value
    HealthBar.SetHealth(m_currentHealth);
  }

  //DelayDestroyShipRoutine coroutine
  IEnumerator DelayDestroyShipRoutine()
  {
    // Fetch all colliders on player ship
    foreach (Collider c in m_colliders)
    {
      // Set each to collision component to false
      // To avoid further collision
      c.enabled = false;
    }
    // Make playership invisible
    m_meshRenderer.enabled = false;
    // Check if m_destroyParticleSystemFx still exists
    if (!m_destroyParticleSystemFx) { Debug.Log("ERR: PlayerCombatManager ====== DelayDestroyShipRoutine() ====== Destory Particle Fx not setup"); }
    // Play destroy particle system fx
    m_destroyParticleSystemFx.Play();
    // Check if destroy sound component still exists
    if (!m_destroyAudioFx) { Debug.Log("ERR: PlayerCombatManager ====== DelayDestroyShipRoutine() ====== Destory Particle Fx not setup"); }
    // Play destroy sound fx
    m_destroyAudioFx.Play();
    // Stop player thrust particle system
    m_playerMovementManager.ThrustParticleSystem.Stop();
    // Wait for 3 seconds nefore ending game
    yield return new WaitForSeconds(m_destroyDelay);
    // Invoke end game event
    OnEndGame.Invoke();
  }

  // DestroyPlayerPrefab method
  public void DestroyPlayerPrefab()
  {
    // Method to destroy spaceship instance
    // Destroy the game object
    Destroy(this.gameObject);
  }
}
