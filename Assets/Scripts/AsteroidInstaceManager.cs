/* ------------------------------------------------------------------------------
<AsteroidInstanceManager Class>
  This script handles
  1> Asteroid and player space ship collision
  2> Destory asteroid if it collides with player space ship
  3> Inflict damage on player space ship
<AsteroidInstanceManager Class>
--------------------------------------------------------------------------------*/
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AsteroidInstaceManager : MonoBehaviour
{
    // Private property that references player game object
    private GameObject m_player;
    private PlayerCombatManager m_playerCombatManager;

    private ParticleSystem m_particleSystemInstance;
    private AudioSource m_audioSource;
    private Collider m_collider;
    private MeshRenderer m_meshRenderer;

    private void Awake()
    {
        // Set reference to player game object
        m_player = GameObject.FindGameObjectWithTag("Player");
        m_playerCombatManager = m_player.GetComponent<PlayerCombatManager>();
        m_audioSource = this.gameObject.GetComponent<AudioSource>();
        m_collider = this.gameObject.GetComponent<Collider>();
        m_particleSystemInstance = this.gameObject.GetComponent<ParticleSystem>();
        m_meshRenderer = this.gameObject.GetComponent<MeshRenderer>();
        m_particleSystemInstance.Stop();
    }
    public void OnTriggerEnter(Collider other)
    {
        // Collider Trigger Method
        // Check if the object collided with is player
        if (other.gameObject == m_player)
        {
            // If yes, then invoke asteroid player spaceship collision
            m_playerCombatManager.TakeDamage(m_playerCombatManager.CurrentHealth, 20);
            // Call destory asteroid
            DestroyAsteroidFx();
            DestroyAsteroid();
            return;
        }
    }

    public void DestroyAsteroid()
    {
        // Method to destroy asteroid instance
        // If called destroy the asteroid instance
        StartCoroutine("DelayDestroyAsteroid");

    }

    IEnumerator DelayDestroyAsteroid()
    {

        m_collider.enabled = false;
        m_meshRenderer.enabled = false;
        yield return new WaitForSeconds(0.7f);
        Destroy(this.gameObject);
    }

    public void DestroyAsteroidFx()
    {
        m_particleSystemInstance.Play();
        m_audioSource.Play();
    }
}
