/* ------------------------------------------------------------------------------
Enemy BulletInstanceManager Class
  * This script handles enemy ship's
  1> Playing bullet sound fx
  2> Moving bullets
  3> Destory bullets if out of screen bounds
  4> Play sound fx of firing bullets
--------------------------------------------------------------------------------*/

using System.Collections;
using UnityEngine;

public class EnemyBulletInstanceManager : MonoBehaviour
{
	// Private property that references the player game object
	private GameObject m_player;
	// Private property that references the player combat manager script
	private PlayerCombatManager m_playerCombatManager;
	// Private property that references the player ship collider
	private Collider m_playerShipCollider;

	// Private property that references enemy bullet particle system fx
	private ParticleSystem m_blastParticleSystem;
	// Private property that references audio source with thrust sound fx
	private AudioSource m_audioSource;
	// Private property that references audio source with sound fx for bullet shot ship 
	private AudioSource m_shipShot;
	// Private property that references the collider on enemy bullet
	private Collider m_collider;
	// Reference to mesh renderer of bullet
	private MeshRenderer m_meshRenderer;
	// Private property that references the rigidbody on bullet
	private Rigidbody m_rb;

	// Private property to set speed of enemy bullets
	private float m_speed = 10f;
	// Private property to set delay destroying the enemy bullets
	private float m_destroyDelay = 3f;
	// Private bool property to allow movement of enemy bullet
	private bool m_canMove = true;

	// Awake method
	private void Awake()
	{
		// Set property to player spaceship instance with player tag
		m_player = GameObject.FindGameObjectWithTag("Player");
		// Set reference to player combat script
		m_playerCombatManager = m_player.GetComponent<PlayerCombatManager>();
		// Set reference to the apsule collider component on the player spaceship
		m_playerShipCollider = m_player.GetComponent<CapsuleCollider>();
		// Set reference to audio source to play enemy bullet fired soundfx 
		m_audioSource = this.gameObject.GetComponent<AudioSource>();
		// Set reference to audio source to play enemy bullet collision soundfx 
		m_shipShot = GameObject.FindGameObjectWithTag("SoundFx").GetComponent<AudioSource>();
		// Set reference to mesh renderer on the enemy bullet
		m_meshRenderer = this.gameObject.GetComponentInChildren<MeshRenderer>();
		// Set reference to mesh renderer on the enemy bullet
		m_collider = this.gameObject.GetComponent<Collider>();
		// Set reference to particle system on the enemy bullet
		m_blastParticleSystem = this.gameObject.GetComponent<ParticleSystem>();
		// Set reference to rigid body on the enemy bullet
		m_rb = this.GetComponent<Rigidbody>();
		// Set can move property to true by default
		m_canMove = true;
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
	public void Update()
	{
		// Check if can move bullets property is true
		if (m_canMove)
		{
			// If true  then Call move bullets method
			MoveBullets();
		}
		// Destory bullets if out of screen bounds
		DestroyOnOutOfScreenBounds();
	}

	// Move bullets method
	private void MoveBullets()
	{
		// Move bullet in the forward direction with value of the speed property
		this.gameObject.transform.Translate(Vector3.forward * m_speed * Time.deltaTime);
	}

	// OnTriggerEnter method
	private void OnTriggerEnter(Collider other)
	{
		// Trigger enter method
		if (other == m_playerShipCollider)
		{
			// Player shot
			// Check if player combat script is attached to game object
			if (m_playerCombatManager == null) { return; }
			m_playerCombatManager.TakeDamage(m_playerCombatManager.CurrentHealth, 5);
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
		// Disable collider on bullet instance to avoid further collisions
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
		// Set bullet instance game object to false
		// Destroy the bullet instance
		Destroy(this.gameObject);
	}
}
