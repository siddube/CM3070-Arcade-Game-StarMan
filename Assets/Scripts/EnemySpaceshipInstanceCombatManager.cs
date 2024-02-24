/* ------------------------------------------------------------------------------
EnemySpaceshipInstanceCombatManager Class
  * This script handles enemy ship's
  1> Combat features
  2> Destroy enemies particle and sound fx
  3> Destroy enemies
--------------------------------------------------------------------------------*/

using System.Collections;
using UnityEngine;

public class EnemySpaceshipInstanceCombatManager : MonoBehaviour
{
    // Private property that references player combat script
    private PlayerCombatManager m_playerCombatManager;
    // Private property that references enemy health
    private int m_currentHealth = 100;
    // Public property that references enemy health for getting and setting value outside class
    public int CurrentHealth { get => m_currentHealth; set => m_currentHealth = value; }

    // Private property that references enemy spaeship particle system fx
    private ParticleSystem m_particleSystemInstance;
    // Private property that references asteroid audio fx
    private AudioSource m_audioSource;
    // Private property that references collider on the asteroid instance
    private Collider m_collider;
    // Private property that references mesh renderer on the asteroid instance
    private MeshRenderer m_meshRenderer;
    // Private property that references enemy bullet spawn manager
    private BulletSpawnManager m_enemyBulletSpawnManager;

    // Private property that references how many seconds to cool enemy bullet firing
    private float m_shootBulletCoolDownPeriod = 3f;
    // Private property that references the status if the enemy spaceship is alive
    private bool m_isShipAlive;
    // Private property to reference the delay in destroying enemy game object 
    private float m_destroyDelay = 3f;
    // Private property that references the score to add on enemy spaceship being shot
    private int m_shipDestroyScore = 500;
    // Start Method
    public void Start()
    {
        // Set reference of player combat manager script
        m_playerCombatManager = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerCombatManager>();
        // Set reference of enemy bullet spawner script
        m_enemyBulletSpawnManager = this.gameObject.GetComponentInChildren<BulletSpawnManager>();
        // Set reference of current enemy ship health to 100
        m_currentHealth = 100;
        // Set reference to the enemy ship collider
        m_collider = this.gameObject.GetComponent<Collider>();
        // Set reference to the enemy ship mesh renderer
        m_meshRenderer = this.gameObject.GetComponent<MeshRenderer>();
        // Set reference to the enemy ship movement soundfx
        m_audioSource = this.gameObject.GetComponent<AudioSource>();
        // Set reference to particle system on the asteroid instace
        m_particleSystemInstance = this.gameObject.GetComponent<ParticleSystem>();
        // Set particle system to stop by default
        m_particleSystemInstance.Stop();
        // Set alive status to true by default
        m_isShipAlive = true;
        // Set spawn enemy delay of the parent which is enemy spawnner game object of the enemy ship
        this.gameObject.GetComponentInParent<EnemySpaceshipSpawner>().SpawnEnemyDelay = 3f;
    }

    // Update method
    public void Update()
    {
        // Decay cool down period by a second
        m_shootBulletCoolDownPeriod -= Time.deltaTime;
        // Check if cool down period is up and the enemy spaceship is alive
        if (m_shootBulletCoolDownPeriod <= 0 && m_isShipAlive)
        {
            // Check if player combat manager script is attached
            if (m_playerCombatManager == null) { Debug.Log("ERR: EnemySpaceshipInstanceCombatManager ====== Update() ====== Player Combat Script Not Found"); return; }
            // Check if player is alive by accessing current health
            if (m_playerCombatManager.CurrentHealth > 0)
            {
                // If yes, then Shoot bullet
                OnShoot();
                // Setback cooldown period with a slight variation in decay
                m_shootBulletCoolDownPeriod = 3f + (float)Random.Range(0, 1);
            }
        }
    }

    // OnShoot method
    public void OnShoot()
    {
        // Shoot a bullet
        // Spawn a new bullet
        m_enemyBulletSpawnManager.SpawnBullet();
    }

    // TakeDamage method
    public void TakeDamage(int currentHealth, int damageValue)
    {
        // Method to take damage when player shoots or a collision occurs with asteroid
        // Set current health to a value subtracted from damage value
        m_currentHealth = currentHealth - damageValue;
        // Check if ship is damged enough to be destoryed
        IsShipDamaged();
    }

    //IsShipDamaged method
    private void IsShipDamaged()
    {
        // Check if current health is lesser than 0
        if (m_currentHealth <= 0)
        {
            // If yes, then trigger particle and sound fx of ship being destroyed
            DestroyEnemyFx();
            // Start destory enemy spaceship 
            StartCoroutine("DelayDestroyEnemyRoutine");
        }
    }

    //DestroyEnemyFx method
    private void DestroyEnemyFx()
    {
        // Check if audio source property is null and log warning
        if (!m_audioSource) { Debug.Log("ERR: EnemySpaceshipInstanceCombatManager ====== DestroyEnemyFx() ====== Audio Source Not Found"); return; }
        // Check if m_particle system instance is null and log warning
        if (!m_particleSystemInstance) { Debug.Log("ERR: EnemySpaceshipInstanceCombatManager ====== DestroyEnemyFx() ====== Particle System Not Found"); return; }
        // Play enemy spaceship destroy particle effects
        m_particleSystemInstance.Play();
        // Play enemy spaceship bullet fired audio fx
        m_audioSource.Play();
    }

    // DelayDestroyEnemyRoutine
    IEnumerator DelayDestroyEnemyRoutine()
    {
        // Add enemy shot score and update score to the player combat manager
        m_playerCombatManager.UpdateScore(m_playerCombatManager.Score, m_shipDestroyScore);
        // Set collider off to disable further collisions
        m_collider.enabled = false;
        // Make enemy ship invisible
        m_meshRenderer.enabled = false;
        // Set status that enemy is not alive on the parent object
        this.gameObject.GetComponentInParent<EnemySpaceshipSpawner>().IsEnemyChildAlive = false;
        // Set spawn delay of enemy on the parent object
        this.gameObject.GetComponentInParent<EnemySpaceshipSpawner>().SpawnEnemyDelay = 3f;
        // Set local reference of enemy spaceship to false
        m_isShipAlive = false;
        // Wait for 3 seconds to destroy bullet
        yield return new WaitForSeconds(m_destroyDelay);
        // Call destroy ship
        DestroyShip();
    }

    // DestroyShip method
    public void DestroyShip()
    {
        // Method to destroy spaceship instance
        // Destroy the spaceship instance
        Destroy(this.gameObject);
    }
}
