/* ------------------------------------------------------------------------------
EnemySpaceshipSpawner
  * This script handles
  1> Spawning of enemy spaceships at a given time interval
--------------------------------------------------------------------------------*/
using UnityEngine;

public class EnemySpaceshipSpawner : MonoBehaviour
{
	// Property set from editor, to reference enemy ship prefabs array of game objects
	[SerializeField] private GameObject[] m_enemyShipPrefabs;
	// Private Property refencing the current enemy ship instance
	private GameObject m_enemySpaceShipInstance;

	// Public bool property to reference if the enemy spaceship instance is alive
	public bool IsEnemyChildAlive = false;
	// Public bool property to set spawning enemy spaceship delay value 
	public float SpawnEnemyDelay = 3f;

	// Update method
	private void Update()
	{
		// Decay spawn enemy time
		SpawnEnemyDelay -= Time.deltaTime;
		// Spawn enemy if delay time is up and the current spaceship instance is not alive
		if (SpawnEnemyDelay <= 0f && !IsEnemyChildAlive)
		{
			// Spawn enemy spaceship
			// Call SpwanEnemyShip method
			SpwanEnemyShip();
			// Set spawn delay back to 3 seconds
			SpawnEnemyDelay = 3f;
		}
	}

	public void SpwanEnemyShip()
	{
		// Check if asteroid prefabs reference is null
		if (m_enemyShipPrefabs == null) { Debug.Log("ERR: AsteroidsSpawnManager ====== SpawnAsteroid() ====== Asteroid Prefabs Found"); return; }
		// Chooose the side the asteroid is going to enter the sceen
		// 4 values for upside, right side, downwards and left side
		int side = Random.Range(0, 4);
		// Setup spawn position, initially set to a zero vector
		Vector3 spawnPoint = Vector3.zero;

		// Switch case based on the side value generated from random sides
		switch (side)
		{
			case 0:
				// Bottom
				// Set x viewport position to a random value
				// Set y viewport position to appear from bottom of the screen
				spawnPoint.x = Random.value;
				spawnPoint.y = 1;
				break;
			case 1:
				// Right
				// Set x viewport position to appear from right side of the screen
				// Set y viewport position to a random value
				spawnPoint.x = 1;
				spawnPoint.y = Random.value;
				break;
			case 2:
				// Top
				// Set x viewport position to a random value
				// Set y viewport position to appear from top of the screen
				spawnPoint.x = Random.value;
				spawnPoint.y = 0;
				break;
			case 3:
				// Left
				// Set x viewport position to appear from left side of the screen
				// Set y viewport position to a random value
				spawnPoint.x = 0;
				spawnPoint.y = Random.value;
				break;
		}

		// Calculate world spawn position from viewport position
		Vector3 worldSpawnPoint = Camera.main.ViewportToWorldPoint(spawnPoint);
		// Set z postion to be a constant zero
		worldSpawnPoint.z = 0;

		// Select an enemy prefab from range of asteroid prefabs
		GameObject selectedEnemyShip = m_enemyShipPrefabs[Random.Range(0, m_enemyShipPrefabs.Length)];

		// Instantiate enemy spaceship instance from enemy spaceship prefab selected and spawn at the calculated world position
		m_enemySpaceShipInstance = Instantiate(
				selectedEnemyShip,
				worldSpawnPoint,
				Quaternion.Euler(-90f, 0f, 0f));

		// Set IsEnemyChildAlive property to true 
		IsEnemyChildAlive = true;

		// Set parent transform of the newly instantiated spaceship
		m_enemySpaceShipInstance.transform.parent = this.transform;
	}
}
