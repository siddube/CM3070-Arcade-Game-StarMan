/* ------------------------------------------------------------------------------
BulletInstanceManager Class
  * This script handles
  1> Playing bullet sound fx
  2> Moving bullets
  3> Destory bullets if out of screen bounds
  4> Play sound fx of firing bullets
--------------------------------------------------------------------------------*/

using UnityEngine;

public class EnemyBulletInstanceManager : MonoBehaviour
{
	private GameObject m_player;
	private PlayerCombatManager m_playerCombatManager;
	private Collider m_playerShipCollider;
	private float m_speed = 10f;

	private AudioSource m_audioSource;

	private void Awake()
	{
		m_player = GameObject.FindGameObjectWithTag("Player");
		m_playerCombatManager = m_player.GetComponent<PlayerCombatManager>();
		m_playerShipCollider = m_player.GetComponent<CapsuleCollider>();
		m_audioSource = this.gameObject.GetComponent<AudioSource>();
	}

	private void Start()
	{
		m_audioSource.Play();
	}

	public void Update()
	{
		Debug.Log("Enemy Move Calling");
		MoveBullets();
	}

	private void MoveBullets()
	{
		this.gameObject.transform.Translate(Vector3.forward * m_speed * Time.deltaTime);
	}

	private void OnTriggerEnter(Collider other)
	{
		if (other == m_playerShipCollider)
		{
			if (m_player == null) { return; }
			m_playerCombatManager.TakeDamage(m_playerCombatManager.CurrentHealth, 5);
			DestroyBulletInstance();
		}
	}

	private void DestroyBulletInstance()
	{
		// Method to destroy bullet instance
		// Set bullet instance game object to false
		this.gameObject.SetActive(false);
		// Destroy the bullet instance
		Destroy(this.gameObject);
	}
}



/*
// Private property to reference the player space ship game object
	private GameObject m_player;
	// Private property to reference the player combat script on the player game object
	private PlayerCombatManager m_playerCombatManager;
	private EnemySpaceshipInstanceCombatManager m_enemyComabatManager;
	// private property to set speed of bullets
	private float m_speed = 10f;
	// Reference to audio source with bullet shot sound fx
	private AudioSource m_audioSource;

	private GameObject m_enemy;
	public GameObject Enemy { get => m_enemy; set => m_enemy = value; }

	// Awake method
	private void Awake()
	{
		// Set property to player spaceship instance with tag
		m_player = GameObject.FindGameObjectWithTag("Player");
		// Set reference to player combat script 
		m_playerCombatManager = m_player.GetComponent<PlayerCombatManager>();
		// Set reference the audio source component on the bullet instance
		m_audioSource = this.gameObject.GetComponent<AudioSource>();
		m_enemy = GameObject.FindGameObjectWithTag("Enemy");
		if (m_enemy == null) { return; }
		m_enemyComabatManager = m_enemy.GetComponent<EnemySpaceshipInstanceCombatManager>();
	}

	// Start method
	private void Start()
	{
		// Check if audio source property is null and log warning
		if (!m_audioSource) { Debug.Log("ERR: BulletInstanceManager ====== Start() ====== Audio Source Not Found"); return; }
		// Play audio bullet fired audio fx
		m_audioSource.Play();
	}

	// Update method
	private void Update()
	{
		// Call move bullets method
		MoveBullets();
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
			if (!m_playerCombatManager) { Debug.Log("ERR: BulletInstanceManager ====== OnTriggerEnter() ====== Player Combat Script Not Found"); return; }
			m_playerCombatManager.UpdateScore(m_playerCombatManager.Score, 100);

			// Call destroy asteroid fx function to play asteroid destroyed sound and particle fx
			other.GetComponent<AsteroidInstaceManager>().DestroyAsteroidFx();
			// Call destroy asteroid method to destroy asteroid game object
			other.GetComponent<AsteroidInstaceManager>().DestroyAsteroid();
			// Call destroy bullet instance method
			DestroyBulletInstance();
		}
		if (other.gameObject.tag == "Player")
		{
			if (m_player == null) { return; }
			//if (m_enemyComabatManager == null) { return; }
			//if (!m_playerCombatManager) { Debug.Log("ERR: BulletInstanceManager ====== OnTriggerEnter() ====== Player Combat Script Not Found"); return; }
			//m_playerCombatManager.UpdateScore(m_playerCombatManager.Score, 100);
			m_playerCombatManager.TakeDamage(m_enemyComabatManager.CurrentHealth, 5);
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
	// DestroyBulletInstance method
	private void DestroyBulletInstance()
	{
		// Method to destroy bullet instance
		// Set bullet instance game object to false
		this.gameObject.SetActive(false);
		// Destroy the bullet instance
		Destroy(this.gameObject);
	}
	*/
