using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class EnemySpaceshipInstanceCombatManager : MonoBehaviour
{
    private PlayerCombatManager m_playerCombatManager;
    private int m_currentHealth = 100;

    public int CurrentHealth { get => m_currentHealth; set => m_currentHealth = value; }
    private string DelayDestroyEnemyString = "DelayDestroyAsteroidRoutine";

    private float m_destroyDelay = 2.5f;

    // Private property that references collider on the asteroid instance
    private Collider m_collider;
    // Private property that references mesh renderer on the asteroid instance
    private MeshRenderer m_meshRenderer;

    void Start()
    {
        m_currentHealth = 100;
        m_collider = this.gameObject.GetComponent<Collider>();
        m_meshRenderer = this.gameObject.GetComponent<MeshRenderer>();
        m_playerCombatManager = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerCombatManager>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnShoot()
    {

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
            StartCoroutine(DelayDestroyEnemyString);
            this.gameObject.GetComponentInParent<EnemySpaceshipSpawner>().IsEnemyChildAlive = false;
            this.gameObject.GetComponentInParent<EnemySpaceshipSpawner>().SpawnEnemyDelay = 3f;
        }
    }

    IEnumerator DelayDestroyAsteroidRoutine()
    {
        // Set collider off to disable further collisions
        m_collider.enabled = false;
        // Make asteroid invisible
        m_meshRenderer.enabled = false;
        m_playerCombatManager.UpdateScore(m_playerCombatManager.Score, 500);
        yield return new WaitForSeconds(m_destroyDelay);
        DestroyShip();
    }

    public void DestroyShip()
    {
        Destroy(this.gameObject);
    }
}
