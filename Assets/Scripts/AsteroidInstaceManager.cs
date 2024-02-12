/* ------------------------------------------------------------------------------
AsteroidInstanceManager Class
  This script handles
  1> Asteroid and player space ship collision
  2> Destory asteroid if it collides with player space ship
  3> Inflict damage on player space ship
--------------------------------------------------------------------------------*/

using System.Collections;
using UnityEngine;

public class AsteroidInstaceManager : MonoBehaviour
{

	// Private property that references player game object
	private GameObject m_player;
	// Private property that references player combat manager
	private PlayerCombatManager m_playerCombatManager;
	// Private property that references player spaceship body collider
	private Collider m_playerShipCollider;
	// Private property that references asteroid particle system fx
	private ParticleSystem m_particleSystemInstance;
	// Private property that references asteroid audio fx
	private AudioSource m_audioSource;
	// Private property that references collider on the asteroid instance
	private Collider m_collider;
	// Private property that references mesh renderer on the asteroid instance
	private MeshRenderer m_meshRenderer;
	// Private property to reference the delay destroy co-routine
	private string DelayDestroyAsteroidString = "DelayDestroyAsteroidRoutine";
	// Private property to reference the delay in destroying asteroid game object 
	private float m_destroyDelay = 0.7f;

	// Awake method
	private void Awake()
	{
		// Set reference to player game object
		m_player = GameObject.FindGameObjectWithTag("Player");
		// Set reference to player combat script on the player game object 
		m_playerCombatManager = m_player.GetComponent<PlayerCombatManager>();
		m_playerShipCollider = m_player.GetComponent<CapsuleCollider>();
		// Set reference to audio source on the asteroid instace 
		m_audioSource = this.gameObject.GetComponent<AudioSource>();
		// Set reference to collider on the asteroid instace 
		m_collider = this.gameObject.GetComponent<Collider>();
		// Set reference to particle system on the asteroid instace
		m_particleSystemInstance = this.gameObject.GetComponent<ParticleSystem>();
		// Set reference to mesh renderer on the asteroid instace
		m_meshRenderer = this.gameObject.GetComponent<MeshRenderer>();
		// Set particle system to stop by default 
		m_particleSystemInstance.Stop();
	}

	// OnTriggerEnter method
	public void OnTriggerEnter(Collider other)
	{
		if (m_playerCombatManager == null) { Debug.Log("ERR: AsteroidInstaceManager ====== OnTriggerEnter() ====== Player Combat Script Not Found"); return; }
		// Collider Trigger Method
		// Check if the object collided with is player
		if (other == m_playerShipCollider)
		{
			// Yes, then set damage to player
			m_playerCombatManager.TakeDamage(m_playerCombatManager.CurrentHealth, 20);
			// Call destory to play asteroid collision particle fx and sound fx
			DestroyAsteroidFx();
			// Call Destroy asteroid method 
			DestroyAsteroid();
		}
	}

	// DestroyAsteroid method
	public void DestroyAsteroid()
	{
		// Method to destroy asteroid instance
		// If called destroy the asteroid instance
		StartCoroutine(DelayDestroyAsteroidString);
	}

	// DelayDestroyAsteroidRoutine method
	IEnumerator DelayDestroyAsteroidRoutine()
	{
		// Check if any properties are still set to null and make no reference
		// Check if collider is null
		if (m_collider == null) { Debug.Log("ERR: AsteroidInstaceManager ====== DelayDestroyAsteroidRoutine() ====== Collider Not Found"); }
		if (m_meshRenderer == null) { Debug.Log("ERR: AsteroidInstaceManager ====== DelayDestroyAsteroidRoutine() ====== Mesh Renderer Not Found"); }
		// Set collider off to disable further collisions
		m_collider.enabled = false;
		// Make asteroid invisible
		m_meshRenderer.enabled = false;
		// Wait for asteroid collision fx to play
		yield return new WaitForSeconds(m_destroyDelay);
		// Destroy asteroid
		Destroy(this.gameObject);
	}

	// DestroyAsteroidFx
	public void DestroyAsteroidFx()
	{
		if (m_particleSystemInstance == null) { Debug.Log("ERR: AsteroidInstaceManager ====== DestroyAsteroidFx() ====== Particle System Not Found"); }
		if (m_audioSource == null) { Debug.Log("ERR: AsteroidInstaceManager ====== DestroyAsteroidFx() ====== Audio Source Not Found"); }
		// Play asteroid collision particle system fx
		m_particleSystemInstance.Play();
		// Play asteroid collision sound fx
		m_audioSource.Play();
	}
}
