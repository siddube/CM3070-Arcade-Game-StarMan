/* ------------------------------------------------------------------------------
BulletInstanceManager Class
  * This script handles
  1> Playing bullet sound fx
  2> Moving bullets
  3> Destory bullets if out of screen bounds
	4> Handle collision with asteroid and enemy spaceship
  5> Play sound fx of firing bullets
--------------------------------------------------------------------------------*/

using System.Collections;
using UnityEngine;

public class BulletInstanceManager : MonoBehaviour
{

	// Private property to reference the player space ship game object
	private GameObject m_player;
	// Private property to reference the player combat script on the player game object
	private PlayerCombatManager m_playerCombatManager;
	// Private property that references enemy spacehip instance combat script
	private EnemySpaceshipInstanceCombatManager m_enemyComabatManager;
	// Private property to reference the enemy space ship game object
	private GameObject m_enemy;

	// Private property that references bullet particle system fx
	private ParticleSystem m_blastParticleSystem;
	// Private property that references the audio source with bullet shot sound fx
	private AudioSource m_audioSource;
	// Private property that references the audio source with bullet collision sound fx
	private AudioSource m_shipShot;
	// Reference to mesh renderer of bullet
	private MeshRenderer m_meshRenderer;
	// Private property that references the collider on bullet
	private Collider m_collider;
	// Private property that references the rigidbody on bullet
	private Rigidbody m_rb;

	// Private property to set speed of bullets
	private float m_speed = 10f;
	// Private property to set delay destroying the bullets
	private float m_destroyDelay = 3f;
	// Private bool property to allow movement of bullet
	private bool m_canMove = true;

	// Awake method
	private void Awake()
	{
		// Set property to player spaceship instance with player tag
		m_player = GameObject.FindGameObjectWithTag("Player");
		// Set reference to player combat script 
		m_playerCombatManager = m_player.GetComponent<PlayerCombatManager>();
		// Set reference to player combat script 
		m_blastParticleSystem = this.gameObject.GetComponent<ParticleSystem>();
		// Set reference to the rigidbody component on the bullet instance
		m_rb = this.GetComponent<Rigidbody>();
		// Set reference to the audio source component on the bullet instance
		m_audioSource = this.gameObject.GetComponent<AudioSource>();
		// Set reference to the mesh renderer component on the bullet instance child
		m_meshRenderer = this.gameObject.GetComponentInChildren<MeshRenderer>();
		// Set reference to the collider component on the bullet instance
		m_collider = this.gameObject.GetComponent<Collider>();
		// Set reference to audio source to play bullet collision soundfx 
		m_shipShot = GameObject.FindGameObjectWithTag("SoundFx").GetComponent<AudioSource>();
		// Set can move property to true by default
		m_canMove = true;
		// Set reference to enemy game object by finding game object with enemy tag
		m_enemy = GameObject.FindGameObjectWithTag("Enemy");
		if (m_enemy == null) { Debug.Log("WARN: BulletInstanceManager ====== Awake() ====== Enemy Game Object Not Found"); return; }
		// Set reference to enemy combat script
		m_enemyComabatManager = m_enemy.GetComponent<EnemySpaceshipInstanceCombatManager>();
	}

	// Start method
	private void Start()
	{
		// Check if audio source property is null and log warning
		if (!m_audioSource) { Debug.Log("ERR: BulletInstanceManager ====== Start() ====== Audio Source Not Found"); return; }
		if (!m_blastParticleSystem) { Debug.Log("ERR: BulletInstanceManager ====== Start() ====== Particle System Not Found"); return; }
		// Play audio bullet fired audio fx
		m_audioSource.Play();
		// Set particle effects to stop by default
		m_blastParticleSystem.Stop();
	}

	// Update method
	private void Update()
	{
		// Check if can move bullets property is true
		if (m_canMove)
		{
			// Call move bullets method
			MoveBullets();
		}
		// Destory bullets if out of screen bounds
		DestroyOnOutOfScreenBounds();
	}

	// Move bullets method
	private void MoveBullets()
	{
		// Move bullet in the forward direction with value of the speed property
		this.gameObject.transform.Translate(Vector3.up * m_speed * Time.deltaTime);
	}

	// OnTriggerEnter method
	private void OnTriggerEnter(Collider other)
	{
		// Trigger enter method
		if (other.gameObject.tag == "Asteroids")
		{
			// Asteroid shot
			// Check if player combat script is attached to other game object
			if (!m_playerCombatManager) { Debug.Log("ERR: BulletInstanceManager ====== OnTriggerEnter() ====== Player Combat Script Not Found"); return; }
			// Add score of 100 points to player on shooting asteroid
			m_playerCombatManager.UpdateScore(m_playerCombatManager.Score, 100);
			// Call destroy asteroid fx function to play asteroid destroyed sound and particle fx
			other.GetComponent<AsteroidInstaceManager>().DestroyAsteroidFx();
			// Call destroy asteroid method to destroy asteroid game object
			other.GetComponent<AsteroidInstaceManager>().DestroyAsteroid();
			// Call destroy bullet coroutine
			StartCoroutine("DestroyBulletRoutine");
		}
		if (other.gameObject.tag == "Enemy")
		{
			// Enemy ship shot
			// Check if enemy combat script is attached to other game object
			if (m_enemyComabatManager == null) { Debug.Log("ERR: BulletInstanceManager ====== OnTriggerEnter() ====== Enemy Combat Script Not Found"); return; }
			// Add damage to enemy
			m_enemyComabatManager.TakeDamage(m_enemyComabatManager.CurrentHealth, 40);
			// Call destroy bullet coroutine
			StartCoroutine("DestroyBulletRoutine");
		}
	}

	// DestroyOnOutOfScreenBounds method
	private void DestroyOnOutOfScreenBounds()
	{
		// Calculate viewport position of bullet
		// If beyond screen bounds
		Vector3 viewportPos = Camera.main.WorldToViewportPoint(this.gameObject.transform.position);
		if (viewportPos.x < -0.1f ||
				viewportPos.x > 1.1f ||
				viewportPos.y < -0.1f ||
				viewportPos.y > 1.1f)
		{
			// Call destroy bullet instance method
			DestroyBulletInstance();
		}
	}

	// DestroyBulletRoutine Coroutine
	IEnumerator DestroyBulletRoutine()
	{
		// Play bullet collision fx
		m_shipShot.Play();
		// Play destory bullet partcile fx
		m_blastParticleSystem.Play();
		// Disable collider on bullet instance
		m_collider.enabled = false;
		// Disable mesh renderer to make bullet invisible
		m_meshRenderer.enabled = false;
		// Set bullet velocity to zero to stop further movement
		m_rb.velocity = Vector3.zero;
		// Set can move to false to stop further movement
		m_canMove = false;
		// Wait for 3 seconds to destroy bullet
		yield return new WaitForSeconds(m_destroyDelay);
		// Call destroy bullet method
		DestroyBulletInstance();
	}
	// DestroyBulletInstance method
	private void DestroyBulletInstance()
	{
		// Method to destroy bullet instance
		// Destroy the bullet instance
		Destroy(this.gameObject);
	}
}
