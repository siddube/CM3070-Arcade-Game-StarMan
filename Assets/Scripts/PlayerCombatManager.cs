/* ------------------------------------------------------------------------------
<PlayerCombatManager Class>
  This script handles
  1> Accepting keyboard input for the firing a bullet from the space ship
  2> Takes damage from collision
<PlayerMovementManager Class>
--------------------------------------------------------------------------------*/

using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class PlayerCombatManager : MonoBehaviour
{
  // Property set from editor, to reference health bar
  [SerializeField] public HealthBar HealthBar;
  // Property set from editor, to reference health bar
  [SerializeField] private GameScoreManager ScoreManager;
  //
  [SerializeField] private ParticleSystem m_destroyParticleSystemFx;
  //
  [SerializeField] private AudioSource m_destroyAudioFx;
  [SerializeField] private GameObject[] m_livesLeftSprites;
  //
  private GameObject m_player;
  private PlayerMovementManager m_playerMovementManager;
  // Private property that references collider on the asteroid instance
  private Collider[] m_colliders;
  // Private property that references mesh renderer on the asteroid instance
  private MeshRenderer m_meshRenderer;

  // Event emitted when player presses spacebar to shoot
  // Event emmitted on shooting bullets with spacebar
  public UnityEvent OnShootBullets;
  // Event emmitted on player space ship getting destroyed
  public UnityEvent OnPlayerShipDestroyed;

  //
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

  private int m_livesLeft = 3;
  // 
  private string DelayDestroyShipString = "DelayDestroyShipRoutine";
  //
  private float m_destroyDelay = 3f;

  // Start method
  private void Start()
  {
    m_player = GameObject.FindGameObjectWithTag("Player");
    // Set health to 100
    m_currentHealth = 100;
    // Set score to 0
    m_score = 0;
    //
    m_colliders = this.gameObject.GetComponents<Collider>();
    m_meshRenderer = this.gameObject.GetComponentInChildren<MeshRenderer>();
    m_playerMovementManager = this.gameObject.GetComponent<PlayerMovementManager>();
    // Set health to maximum health on health bar
    if (!HealthBar) { Debug.Log("ERR: PlayerCombatManager ====== UpdateScore() ====== HealthBar not setup"); return; }
    HealthBar.SetMaxHealth(m_currentHealth);
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
      m_livesLeft -= 1;

      if (m_livesLeft > 0)
      {
        StartCoroutine("RespawnPlayer");
        m_livesLeftSprites[m_livesLeft - 1].SetActive(false);
      }
      else
      {
        OnPlayerShipDestroyed.Invoke();
        StartCoroutine(DelayDestroyShipString);
      }
      // If yes then invoke on player ship destoryed event

    }
  }

  IEnumerator RespawnPlayer()
  {
    foreach (Collider c in m_colliders)
    {
      c.enabled = false;
    }
    m_meshRenderer.enabled = false;
    m_destroyAudioFx.Play();
    m_destroyParticleSystemFx.Play();
    m_playerMovementManager.ThrustParticleSystem.Stop();
    m_player.GetComponent<Rigidbody>().velocity = Vector3.zero;
    yield return new WaitForSeconds(m_destroyDelay);
    m_meshRenderer.enabled = true;
    foreach (Collider c in m_colliders)
    {
      c.enabled = true;
    }
    m_playerMovementManager.transform.position = new Vector3(0f, 0f, 0f);
    m_currentHealth = 100;
    HealthBar.SetHealth(m_currentHealth);
  }

  IEnumerator DelayDestroyShipRoutine()
  {
    foreach (Collider c in m_colliders)
    {
      c.enabled = false;
    }
    // Make asteroid invisible
    m_meshRenderer.enabled = false;
    m_destroyAudioFx.Play();
    m_destroyParticleSystemFx.Play();
    m_playerMovementManager.ThrustParticleSystem.Stop();
    yield return new WaitForSeconds(m_destroyDelay);
    OnEndGame.Invoke();

  }
  public void DestroyPlayerPrefab()
  {
    Destroy(this.gameObject);
  }
}
