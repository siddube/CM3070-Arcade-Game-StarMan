/* ------------------------------------------------------------------------------
<AsteroidInstanceManager Class>
  This script handles
  1> Asteroid and player space ship collision
  2> Destory asteroid if it collides with player space ship
  3> Inflict damage on player space ship
<AsteroidInstanceManager Class>
--------------------------------------------------------------------------------*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AsteroidInstaceManager : MonoBehaviour
{
    // Event emitted when asteroid hits player spaceship
    public UnityEvent OnAsteroidPlayerCollision;
    // Private property that references player game object
    private GameObject m_player;

    private void Awake()
    {
        // Set reference to player game object
        m_player = GameObject.FindGameObjectWithTag("Player");
    }
    public void OnTriggerEnter(Collider other)
    {
        // Collider Trigger Method
        // Check if the object collided with is player
        if (other.gameObject == m_player)
        {
            // If yes, then invoke asteroid player spaceship collision
            OnAsteroidPlayerCollision.Invoke();
            // Call destory asteroid
            DestroyAsteroid();
        }
    }

    public void DestroyAsteroid()
    {
        // Method to destroy asteroid instance
        // If called destroy the asteroid instance
        Destroy(this.gameObject);
    }
}
