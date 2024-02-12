
using UnityEngine;

public class EnemySpaceshipMovement : MonoBehaviour
{
	private Transform m_playerTransform;
	private Rigidbody m_rb;
	private float m_angle = 0.0f;
	private bool m_isInCombatRange = false;


	private void Awake()
	{
		m_playerTransform = GameObject.FindObjectOfType<PlayerMovementManager>().transform;
		m_rb = this.gameObject.GetComponent<Rigidbody>();
	}

	private void Update()
	{
		//Debug.Log(m_playerTransform.position);
		//float x = m_player.transform.position.x + 3f * Mathf.Cos(angle);
		//float y = m_player.transform.position.y + 3f * Mathf.Sin(angle);
		//transform.position = Vector3.Lerp(transform.position, new Vector3(x, y, 0), 2f);
		//angle -= 1f * Time.deltaTime;
		if (m_isInCombatRange == true) { return; }

		Vector3 lookAtDirection = m_playerTransform.position - transform.position;
		Quaternion rotation = Quaternion.LookRotation(lookAtDirection, Vector3.back);
		transform.position = Vector3.MoveTowards(transform.position, m_playerTransform.position, 2f * Time.deltaTime);
		transform.rotation = Quaternion.Lerp(transform.rotation, rotation, 15 * Time.deltaTime);
	}

	private void OnTriggerEnter(Collider other)
	{
		m_isInCombatRange = true;
		m_rb.velocity = Vector3.zero;
		/* if (other.gameObject.tag == "Player")
		{
			float x = m_playerTransform.position.x + 5f * Mathf.Cos(m_angle);
			float y = m_playerTransform.position.y + 5f * Mathf.Sin(m_angle);
			transform.position = Vector3.Lerp(transform.position, new Vector3(x, y, 0), 2f);
			m_angle -= 1f * Time.deltaTime;
		} */
	}
}
