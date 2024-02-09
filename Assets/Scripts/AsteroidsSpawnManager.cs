/* ------------------------------------------------------------------------------
AsteroidsSpawnManager
  * This script handles
  1> Spawning of asteroids at a given time interval
--------------------------------------------------------------------------------*/
using UnityEngine;

public class AsteroidsSpawnManager : MonoBehaviour
{
	// Property set from editor, to reference asteroid prefabs array of game objects
	[SerializeField] private GameObject[] m_asteroidPrefabs;
	// Property set from editor, to reference seconds to wait before spawning new asteroid
	[SerializeField] private float m_secondsBetweenAsteroids;
	// Property set from editor, to reference force range to be applied on asteroids
	[SerializeField] private Vector2 m_forceRange;
	// Property set from editor, to reference minimum asteroid size
	[SerializeField] private float m_minAsteroidSize = 20f;
	// Property set from editor, to reference maximum asteroid size
	[SerializeField] private float m_maxAsteroidSize = 30f;
	// Timer property, ticked on every frame to help spawn new asteroids after set interval
	private float m_timer;

	// Start method
	private void Start()
	{
		// Start timer at max value of seconds between asteroid property
		m_timer = m_secondsBetweenAsteroids;
	}

	// Update method
	private void Update()
	{
		// Call handle the spawn timer
		HandleSpawnTimer();
	}

	// HandleSpawmTimer method
	private void HandleSpawnTimer()
	{
		// This method helps spawn new asteroids after set interval
		// After the timer is up
		// We spawn new asteroid
		// And reset timer property
		m_timer -= Time.deltaTime;
		// If timer is below zero, that is time between asteroid spawning is up
		if (m_timer <= 0)
		{
			// Then spawn new asteroid
			SpawnAsteroid();
			// Reset timer
			m_timer += m_secondsBetweenAsteroids;
		}
	}

	// SpawnAsteroid method
	private void SpawnAsteroid()
	{
		// This method spawns new asteroid from available asteroid prefabs
		// Check if asteroid prefabs reference is null
		if (m_asteroidPrefabs == null) { Debug.Log("ERR: AsteroidsSpawnManager ====== SpawnAsteroid() ====== Asteroid Prefabs Found"); return; }
		// Chooose the side the asteroid is going to enter the sceen
		// 4 values for upside, right side, downwards and left side
		int side = Random.Range(0, 4);
		// Setup spawn position, initially set to a zero vector
		Vector3 spawnPoint = Vector3.zero;
		// Setup movement direction, initially set to a zero vector
		Vector2 direction = Vector2.zero;

		// Switch case based on the side value generated from random sides
		switch (side)
		{
			case 0:
				// Bottom
				// Set x viewport position to a random value
				// Set y viewport position to appear from bottom of the screen
				// Set force vector to move up and at a random angle
				spawnPoint.x = Random.value;
				spawnPoint.y = 1;
				direction = new Vector2(Random.Range(-1f, 1f), -1f);
				break;
			case 1:
				// Right
				// Set x viewport position to appear from right side of the screen
				// Set y viewport position to a random value
				// Set force vector to move left and at a random angle
				spawnPoint.x = 1;
				spawnPoint.y = Random.value;
				direction = new Vector2(-1f, Random.Range(-1f, 1f));
				break;
			case 2:
				// Top
				// Set x viewport position to a random value
				// Set y viewport position to appear from top of the screen
				// Set force vector to move down and at a random angle
				spawnPoint.x = Random.value;
				spawnPoint.y = 0;
				direction = new Vector2(Random.Range(-1f, 1f), 1f);
				break;
			case 3:
				// Left
				// Set x viewport position to appear from left side of the screen
				// Set y viewport position to a random value
				// Set force vector to move right and at a random angle
				spawnPoint.x = 0;
				spawnPoint.y = Random.value;
				direction = new Vector2(1f, Random.Range(-1f, 1f));
				break;
		}


		// Calculate world spawn position from viewport position
		Vector3 worldSpawnPoint = Camera.main.ViewportToWorldPoint(spawnPoint);
		// Set z postion to be a constant zero
		worldSpawnPoint.z = 0;

		// Select an asteroid from range of asteroid prefabs
		GameObject selectedAsteroid = m_asteroidPrefabs[Random.Range(0, m_asteroidPrefabs.Length)];

		// Instantiate asteroid instance from asteroid prefab selected and spawn at the calculated world position
		GameObject asteroidInstance = Instantiate(
				selectedAsteroid,
				worldSpawnPoint,
				Quaternion.Euler(0f, 0f, Random.Range(0, 360)));

		// Get random size value between minimum and maximum astroid size
		float selectedAsteroidSize = Random.Range(m_minAsteroidSize, m_maxAsteroidSize);

		// Create a vector 3 from float values to set asteroid size
		Vector3 selectedAsteroidSizeVector = new Vector3(selectedAsteroidSize, selectedAsteroidSize, selectedAsteroidSize);

		// Call method that sets selected size value to the asteroid instance
		SetLocalSize(asteroidInstance, selectedAsteroidSizeVector);

		// Set asteroid spawn object as the parent of the created asteroid instance
		asteroidInstance.transform.parent = this.transform;

		// Get reference of the rigid body of the asteroid
		Rigidbody rb = asteroidInstance.GetComponent<Rigidbody>();

		// Add velocity to the rigid body with a force withing force range property
		// Make the velocity frame rate independent
		rb.velocity = direction.normalized * Random.Range(m_forceRange.x, m_forceRange.y);
	}

	// SetLocalSize method
	private void SetLocalSize(GameObject instace, Vector3 size)
	{
		// Method to set asteroid local scale
		Transform asteroidTransform = instace.transform;
		asteroidTransform.localScale = size;
	}
}
