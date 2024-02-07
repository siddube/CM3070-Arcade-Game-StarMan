/* ------------------------------------------------------------------------------
<BulletInstanceManager Class>
  This script handles
  1> Playing bullet sound fx
  2> Moving bullets
  3> Destory bullets if out of screen bounds
<BulletInstanceManager Class>
--------------------------------------------------------------------------------*/
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BulletInstanceManager : MonoBehaviour
{
    private GameObject m_player;
    private PlayerCombatManager m_playerCombatManager;
    //Event emitted when bullet destroys an asteroid
    //public UnityEvent ShotAsteroid;
    // Property to set speed of bullets
    private float m_speed = 10f;
    // Reference to audio source with bullet shot sound fx
    private AudioSource audioSource;


    private void Awake()
    {
        m_player = GameObject.FindGameObjectWithTag("Player");
        m_playerCombatManager = m_player.GetComponent<PlayerCombatManager>();
        // Reference the audio source component
        audioSource = this.gameObject.GetComponent<AudioSource>();
    }

    private void Start()
    {
        if (!audioSource) { Debug.Log("ERR: BulletInstanceManager ====== Start() ====== Audio Source Not Found"); return; }
        // Play audio bullet fired clip
        audioSource.Play();
    }
    private void Update()
    {
        // Call move bullets method
        MoveBullets();
        // Destory bullets if out of screen bounds
        DestroyOnOutOfScreenBounds();
    }

    private void MoveBullets()
    {
        // Move bullet in the forward direction with value of the speed property
        this.gameObject.transform.Translate(Vector3.up * m_speed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        // Trigger enter method
        if (other.gameObject.tag == "Asteroids")
        {
            m_playerCombatManager.UpdateScore(m_playerCombatManager.Score, 100);
            // Asteroid shot
            // Invoke asteroid shot event 
            //ShotAsteroid.Invoke();
            // Set the asteroid instance game object to false
            //other.gameObject.SetActive(false);
            // Destroy the game object
            //Destroy(other.gameObject);
            other.GetComponent<AsteroidInstaceManager>().DestroyAsteroidFx();
            other.GetComponent<AsteroidInstaceManager>().DestroyAsteroid();
            // Call destroy bullet instance method
            DestroyBulletInstance();
            return;
        }
    }

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
        this.gameObject.SetActive(false);
        // Destroy the bullet instance
        Destroy(this.gameObject);
    }
}
