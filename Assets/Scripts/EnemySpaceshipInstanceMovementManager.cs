/* ------------------------------------------------------------------------------
EnemySpaceshipMovement Class
  * This script handles enemy ship's
  1> Movement
  2> Rotation to look at the player
  3> Calculate playership combat range
--------------------------------------------------------------------------------*/
using UnityEngine;

public class EnemySpaceshipMovement : MonoBehaviour
{
	// Private property to reference the player transform
	private Transform m_playerTransform;
	// Private bool property to check if player spaceship in combat range
	private bool m_isInCombatRange = false;

	// Awake method
	private void Awake()
	{
		// Set reference to player transform
		m_playerTransform = GameObject.FindObjectOfType<PlayerMovementManager>().transform;
	}

	// Update method
	private void Update()
	{
		// If the player is not in combat range
		if (!m_isInCombatRange)
		{
			// Look at player
			LookAtPlayer();
			// Move towards the player
			MoveTowardsPlayer();
		}
		// If player is combat range
		if (m_isInCombatRange)
		{
			// If in combat range
			// Only look at player without any further movement towards player
			LookAtPlayer();
		}
	}

	// MoveTowardsPlayer method
	private void MoveTowardsPlayer()
	{
		// Check if player transform property is null and log warning
		if (m_playerTransform == null) { Debug.Log("ERR: EnemySpaceshipMovement ====== LookAtPlayer() ====== Transform destoryed"); return; }
		// Move the player with Vector3 MoveTowards method passing in enemy spaceship position, player spaceship position and a step of 1 unit
		this.gameObject.transform.position = Vector3.MoveTowards(this.gameObject.transform.position, m_playerTransform.position, 1f * Time.deltaTime);
	}

	// LookAtPlayer method
	private void LookAtPlayer()
	{
		// Check if player transform property is null and log warning
		if (m_playerTransform == null) { Debug.Log("ERR: EnemySpaceshipMovement ====== LookAtPlayer() ====== Transform destoryed"); return; }
		// Get the direction to look at by vector addition with player transform vector and negative of spaceship
		Vector3 lookAtDirection = m_playerTransform.position - this.gameObject.transform.position;
		// Calculate the querternion equivalent of lookAtDirection vector
		Quaternion rotation = Quaternion.LookRotation(lookAtDirection, Vector3.back);
		// Use quaternion Lerp method to rotate the enemy spaceship with a rotate value of 2
		this.gameObject.transform.rotation = Quaternion.Lerp(this.gameObject.transform.rotation, rotation, 2 * Time.deltaTime);
	}

	// OnTriggerEnter method
	private void OnTriggerEnter(Collider other)
	{
		// If enemy is under player spaceship combat range on trigger enter
		if (other.gameObject.tag == "Player")
		{
			// Then enemy ship is in combat range
			// Then set the m_isInCombatRange property to true
			m_isInCombatRange = true;
		}
	}

	// OnTriggerExit method
	private void OnTriggerExit(Collider other)
	{
		// If enemy is under player spaceship combat range
		if (other.gameObject.tag == "Player")
		{
			// Then enemy ship is out if combat range
			// Then set the m_isInCombatRange property to false
			m_isInCombatRange = false;
		}
	}
}
