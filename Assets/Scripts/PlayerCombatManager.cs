using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class PlayerCombatManager : MonoBehaviour
{
    public UnityEvent ShootBullets;
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnShoot(InputValue input)
    {
        ShootBullets.Invoke();
    }
}
