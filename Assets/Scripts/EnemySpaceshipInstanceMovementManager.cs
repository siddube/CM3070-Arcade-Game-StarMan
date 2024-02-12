
using System;
using UnityEngine;

public class EnemySpaceshipMovement : MonoBehaviour
{
	private Transform m_playerTransform;
	private Rigidbody m_rb;
	private bool m_isInCombatRange = false;

	private void Awake()
	{
		m_playerTransform = GameObject.FindObjectOfType<PlayerMovementManager>().transform;
		m_rb = this.gameObject.GetComponent<Rigidbody>();
	}

	private void Update()
	{
		if (!m_isInCombatRange)
		{
			LookAtPlayer();
			MoveTowardsPlayer();
		}
		else
		{
			LookAtPlayer();
		}
	}

	private void MoveTowardsPlayer()
	{
		transform.position = Vector3.MoveTowards(transform.position, m_playerTransform.position, 1f * Time.deltaTime);
	}

	private void LookAtPlayer()
	{
		Vector3 lookAtDirection = m_playerTransform.position - transform.position;
		Quaternion rotation = Quaternion.LookRotation(lookAtDirection, Vector3.back);
		transform.rotation = Quaternion.Lerp(transform.rotation, rotation, 15 * Time.deltaTime);
	}

	private void OnTriggerEnter(Collider other)
	{
		m_isInCombatRange = true;
		m_rb.velocity = Vector3.zero;
	}

	private void OnTriggerExit(Collider other)
	{
		m_isInCombatRange = false;
	}
}