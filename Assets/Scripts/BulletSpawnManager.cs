/* ------------------------------------------------------------------------------
BulletSpawnManager Class
  * This script handles
  1> Spawning of new bullets
--------------------------------------------------------------------------------*/

using UnityEngine;

public class BulletSpawnManager : MonoBehaviour
{
  // Private property accessible from editor, to reference bullet prefab
  [SerializeField] private GameObject BulletPrefab;
  // Private property accessible from editor, to reference bullet prefab's parent object
  [SerializeField] private GameObject BulletParentObject;
  // Private property accessible from editor, to reference the transform where new bullets are instantiated
  [SerializeField] private Transform BulletSpawnTransform;
  // Private property that references the new bullet instantiated
  private GameObject m_newBullet;

  public void SpawnBullet()
  {
    // Spawn new bullets when the shoot event is invoked from player combat manager class
    // If bullet prefab is not referenced, then throw a warning to the console log
    if (!BulletPrefab) { Debug.Log("ERR: SpaceshipCombat ====== ShootRegular() ====== Bullet Prefab Not Found"); return; }
    // If bullet prefab parent object is not referenced, then throw a warning to the console log
    if (!BulletParentObject) { Debug.Log("ERR: SpaceshipCombat ====== ShootRegular() ====== Bullet Parent Object Not Found"); return; }

    // Else, instantiate a new bullet from bullet prefab
    // Using bullet spawn transform
    m_newBullet = Instantiate(BulletPrefab, BulletSpawnTransform.position, BulletSpawnTransform.rotation);
    // Set the bullet parent object to the transform of the spawn position
    m_newBullet.transform.parent = BulletParentObject.transform;
  }
}
