/* ------------------------------------------------------------------------------
BulletInstanceManager Class
  * This script handles
  1> Playing bullet sound fx
  2> Moving bullets
  3> Destory bullets if out of screen bounds
  4> Play sound fx of firing bullets
--------------------------------------------------------------------------------*/

using UnityEngine;

public class BulletInstanceManager : MonoBehaviour
{
	// Private property to reference the player space ship game object
	private GameObject m_player;
	// Private property to reference the player combat script on the player game object
	private PlayerCombatManager m_playerCombatManager;
	// private property to set speed of bullets
	private float m_speed = 10f;
	// Reference to audio source with bullet shot sound fx
	private AudioSource m_audioSource;

	// Awake method
	private void Awake()
	{
		// Set property to player spaceship instance with tag
		m_player = GameObject.FindGameObjectWithTag("Player");
		// Set reference to player combat script 
		m_playerCombatManager = m_player.GetComponent<PlayerCombatManager>();
		// Set reference the audio source component on the bullet instance
		m_audioSource = this.gameObject.GetComponent<AudioSource>();
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
}
