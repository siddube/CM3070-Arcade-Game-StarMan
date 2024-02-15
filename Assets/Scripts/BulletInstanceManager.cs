/* ------------------------------------------------------------------------------
BulletInstanceManager Class
  * This script handles
  1> Playing bullet sound fx
  2> Moving bullets
  3> Destory bullets if out of screen bounds
  4> Play sound fx of firing bullets
--------------------------------------------------------------------------------*/

using System;
using System.Collections;
using UnityEngine;

public class BulletInstanceManager : MonoBehaviour
{

	// Private property to reference the player space ship game object
	private GameObject m_player;
	// Private property to reference the player combat script on the player game object
	private PlayerCombatManager m_playerCombatManager;
	private EnemySpaceshipInstanceCombatManager m_enemyComabatManager;

	//
	private ParticleSystem m_blastParticleSystem;
	// private property to set speed of bullets
	private float m_speed = 10f;
	// Reference to audio source with bullet shot sound fx
	private AudioSource m_audioSource;
	//
	private AudioSource m_shipShot;
	//
	private MeshRenderer m_meshRenderer;
	//
	private Collider m_collider;
	//
	private Rigidbody m_rb;

	private GameObject m_enemy;
	public GameObject Enemy { get => m_enemy; set => m_enemy = value; }

	private string DestroyBulletString = "DestroyBulletRoutine";

	private float m_destroyDelay = 3f;

	private bool m_canMove = true;

	// Awake method
	private void Awake()
	{
		// Set property to player spaceship instance with tag
		m_player = GameObject.FindGameObjectWithTag("Player");
		// Set reference to player combat script 
		m_playerCombatManager = m_player.GetComponent<PlayerCombatManager>();
		m_blastParticleSystem = this.gameObject.GetComponent<ParticleSystem>();
		//
		m_rb = this.GetComponent<Rigidbody>();
		// Set reference the audio source component on the bullet instance
		m_audioSource = this.gameObject.GetComponent<AudioSource>();
		m_meshRenderer = this.gameObject.GetComponentInChildren<MeshRenderer>();
		m_collider = this.gameObject.GetComponent<Collider>();
		m_shipShot = GameObject.FindGameObjectWithTag("SoundFx").GetComponent<AudioSource>();
		Enemy = GameObject.FindGameObjectWithTag("Enemy");
		if (Enemy == null) { return; }
		m_enemyComabatManager = Enemy.GetComponent<EnemySpaceshipInstanceCombatManager>();

		m_canMove = true;

	}

	// Start method
	private void Start()
	{
		// Check if audio source property is null and log warning
		if (!m_audioSource) { Debug.Log("ERR: BulletInstanceManager ====== Start() ====== Audio Source Not Found"); return; }
		// Play audio bullet fired audio fx
		m_audioSource.Play();
		m_blastParticleSystem.Stop();
	}

	// Update method
	private void Update()
	{
		if (m_canMove)
		{
			MoveBullets();
		}
		// Call move bullets method

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
			StartCoroutine(DestroyBulletString);
			// Call destroy bullet instance method
			//DestroyBulletInstance();
		}
		if (other.gameObject.tag == "Enemy")
		{
			if (Enemy == null) { return; }
			//if (m_enemyComabatManager == null) { return; }
			//if (!m_playerCombatManager) { Debug.Log("ERR: BulletInstanceManager ====== OnTriggerEnter() ====== Player Combat Script Not Found"); return; }
			//m_playerCombatManager.UpdateScore(m_playerCombatManager.Score, 100);
			m_enemyComabatManager.TakeDamage(m_enemyComabatManager.CurrentHealth, 40);
			StartCoroutine(DestroyBulletString);
			//DestroyBulletInstance();
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

	IEnumerator DestroyBulletRoutine()
	{

		m_shipShot.Play();
		m_blastParticleSystem.Play();
		m_collider.enabled = false;
		m_meshRenderer.enabled = false;
		m_rb.velocity = Vector3.zero;
		m_canMove = false;
		yield return new WaitForSeconds(m_destroyDelay);
		DestroyBulletInstance();
	}
	// DestroyBulletInstance method
	private void DestroyBulletInstance()
	{
		// Method to destroy bullet instance
		// Set bullet instance game object to false
		//this.gameObject.SetActive(false);
		// Destroy the bullet instance
		Destroy(this.gameObject);
	}
}
