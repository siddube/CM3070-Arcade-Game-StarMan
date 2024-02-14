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
