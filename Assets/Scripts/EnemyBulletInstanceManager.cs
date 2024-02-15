/* ------------------------------------------------------------------------------
BulletInstanceManager Class
  * This script handles
  1> Playing bullet sound fx
  2> Moving bullets
  3> Destory bullets if out of screen bounds
  4> Play sound fx of firing bullets
--------------------------------------------------------------------------------*/

using System.Collections;
using UnityEngine;

public class EnemyBulletInstanceManager : MonoBehaviour
{
	private GameObject m_player;
	private PlayerCombatManager m_playerCombatManager;
	private Collider m_playerShipCollider;
	private float m_speed = 10f;

	private AudioSource m_audioSource;

	private AudioSource m_shipShot;

	private Rigidbody m_rb;
	private ParticleSystem m_blastParticleSystem;
	private Collider m_collider;
	private MeshRenderer m_meshRenderer;
	private bool m_canMove = true;
	private float m_destroyDelay = 3f;

	private void Awake()
	{
		m_player = GameObject.FindGameObjectWithTag("Player");
		m_playerCombatManager = m_player.GetComponent<PlayerCombatManager>();
		m_playerShipCollider = m_player.GetComponent<CapsuleCollider>();
		m_audioSource = this.gameObject.GetComponent<AudioSource>();
		m_shipShot = GameObject.FindGameObjectWithTag("SoundFx").GetComponent<AudioSource>();
		m_meshRenderer = this.gameObject.GetComponentInChildren<MeshRenderer>();
		m_collider = this.gameObject.GetComponent<Collider>();
		m_blastParticleSystem = this.gameObject.GetComponent<ParticleSystem>();
		m_rb = this.GetComponent<Rigidbody>();
		m_canMove = true;

	}


	private void Start()
	{
		m_audioSource.Play();
		m_blastParticleSystem.Stop();
	}

	public void Update()
	{
		if (m_canMove)
		{
			MoveBullets();
		}
		// Destory bullets if out of screen bounds
		DestroyOnOutOfScreenBounds();
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
			StartCoroutine("DestroyBulletRoutine");
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

	private void DestroyBulletInstance()
	{
		// Method to destroy bullet instance
		// Set bullet instance game object to false
		//this.gameObject.SetActive(false);
		// Destroy the bullet instance
		Destroy(this.gameObject);
	}
}
