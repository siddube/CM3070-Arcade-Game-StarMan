using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BulletSpawnManager : MonoBehaviour
{

    [field: SerializeField] private GameObject BulletPrefab;
    [field: SerializeField] private GameObject BulletParentObject;
    [field: SerializeField] private Transform BulletSpawnTransform;
    private GameObject m_newBullet;


    public void SpawnBullet()
    {

        if (!BulletPrefab) { Debug.Log("ERR: SpaceshipCombat ====== ShootRegular() ====== Bullet Prefab Not Found"); return; }
        if (!BulletParentObject) { Debug.Log("ERR: SpaceshipCombat ====== ShootRegular() ====== Bullet Parent Object Not Found"); return; }

        m_newBullet = Instantiate(BulletPrefab, BulletSpawnTransform.position, BulletSpawnTransform.rotation);
        m_newBullet.transform.parent = BulletParentObject.transform;
    }
}
