using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class EnemySpaceshipInstanceCombatManager : MonoBehaviour
{
    private PlayerCombatManager m_playerCombatManager;
    private int m_currentHealth = 100;

    public int CurrentHealth { get => m_currentHealth; set => m_currentHealth = value; }
    private string DelayDestroyEnemyString = "DelayDestroyEnemyRoutine";

    private float m_destroyDelay = 2.5f;
    // Private property that references asteroid particle system fx
    private ParticleSystem m_particleSystemInstance;
    // Private property that references asteroid audio fx
    private AudioSource m_audioSource;
    // Private property that references collider on the asteroid instance
    private Collider m_collider;
    // Private property that references mesh renderer on the asteroid instance
    private MeshRenderer m_meshRenderer;
    private BulletSpawnManager m_enemyBulletSpawnManager;

    private float m_shootBulletCoolDownPeriod = 3f;
    private bool m_isShipAlive;

    void Start()
    {
        m_currentHealth = 100;
        m_collider = this.gameObject.GetComponent<Collider>();
        m_meshRenderer = this.gameObject.GetComponent<MeshRenderer>();
        m_playerCombatManager = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerCombatManager>();
        m_enemyBulletSpawnManager = this.gameObject.GetComponentInChildren<BulletSpawnManager>();
        m_audioSource = this.gameObject.GetComponent<AudioSource>();
        // Set reference to particle system on the asteroid instace
        m_particleSystemInstance = this.gameObject.GetComponent<ParticleSystem>();
        m_particleSystemInstance.Stop();
        m_isShipAlive = true;
        this.gameObject.GetComponentInParent<EnemySpaceshipSpawner>().SpawnEnemyDelay = 3f;

    }

    // Update is called once per frame
    void Update()
    {

        m_shootBulletCoolDownPeriod -= Time.deltaTime;
        if (m_shootBulletCoolDownPeriod <= 0 && m_isShipAlive)
        {
            OnShoot();
            m_shootBulletCoolDownPeriod = 3f + (float)Random.Range(0, 1); ;
        }

    }

    public void OnShoot()
    {
        m_enemyBulletSpawnManager.SpawnBullet();
    }

    public void TakeDamage(int currentHealth, int damageValue)
    {
        m_currentHealth = currentHealth - damageValue;
        IsShipDamaged();
    }

    private void IsShipDamaged()
    {
        if (m_currentHealth <= 0)
        {
            DestroyEnemyFx();
            StartCoroutine(DelayDestroyEnemyString);
        }
    }

    private void DestroyEnemyFx()
    {
        m_particleSystemInstance.Play();
        m_audioSource.Play();
    }

    IEnumerator DelayDestroyEnemyRoutine()
    {
        // Set collider off to disable further collisions
        m_collider.enabled = false;
        // Make asteroid invisible
        m_meshRenderer.enabled = false;
        this.gameObject.GetComponentInParent<EnemySpaceshipSpawner>().IsEnemyChildAlive = false;
        this.gameObject.GetComponentInParent<EnemySpaceshipSpawner>().SpawnEnemyDelay = 3f;
        m_isShipAlive = false;
        m_playerCombatManager.UpdateScore(m_playerCombatManager.Score, 500);
        yield return new WaitForSeconds(m_destroyDelay);
        DestroyShip();
    }

    public void DestroyShip()
    {
        Destroy(this.gameObject);
    }
}
