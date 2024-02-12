using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpaceshipSpawner : MonoBehaviour
{
	[SerializeField] private GameObject[] enemyShipPrefabs;
	[SerializeField] private Vector2 m_forceRange;
	private GameObject m_enemySpaceShipInstance;
	private Rigidbody m_rb;
	public bool IsEnemyChildAlive = false;
	private float spawnAseroidDelay = 3f;

	// Start is called before the first frame update
	private void Update()
	{
		spawnAseroidDelay -= Time.deltaTime;
		if (spawnAseroidDelay <= 0f && !IsEnemyChildAlive)
		{
			SpwanEnemyShip();
			spawnAseroidDelay = 3f;
		}
	}

	public void SpwanEnemyShip()
	{
		int side = Random.Range(0, 4);

		Vector3 spawnPoint = Vector3.zero;
		Vector2 direction = Vector2.zero;

		switch (side)
		{
			case 0:
				//Top
				spawnPoint.x = Random.value;
				spawnPoint.y = 1;
				direction = new Vector2(Random.Range(-1f, 1f), -1f);
				break;
			case 1:
				//Right
				spawnPoint.x = 1;
				spawnPoint.y = Random.value;
				direction = new Vector2(-1f, Random.Range(-1f, 1f));
				break;
			case 2:
				//Bottom
				spawnPoint.x = Random.value;
				spawnPoint.y = 0;
				direction = new Vector2(Random.Range(-1f, 1f), 1f);
				break;
			case 3:
				//Left
				spawnPoint.x = 0;
				spawnPoint.y = Random.value;
				direction = new Vector2(1f, Random.Range(-1f, 1f));
				break;
		}

		Vector3 worldSpawnPoint = Camera.main.ViewportToWorldPoint(spawnPoint);
		worldSpawnPoint.z = 0;

		GameObject selectedEnemyShip = enemyShipPrefabs[Random.Range(0, enemyShipPrefabs.Length)];

		m_enemySpaceShipInstance = Instantiate(
				selectedEnemyShip,
				worldSpawnPoint,
				Quaternion.Euler(-90f, 0f, 0f));

		IsEnemyChildAlive = true;

		m_enemySpaceShipInstance.transform.parent = this.transform;

		// m_rb = m_enemySpaceShipInstance.GetComponent<Rigidbody>();

		// m_rb.velocity = direction.normalized * Random.Range(m_forceRange.x, m_forceRange.y);
	}
}
