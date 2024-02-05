/* ------------------------------------------------------------------------------
<PlayerCombatManager Class>
  This script handles
  1> Accepting keyboard input for the firing a bullet from the space ship
<PlayerMovementManager Class>
--------------------------------------------------------------------------------*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class PlayerCombatManager : MonoBehaviour
{
  //Event emitted when player presses spacebar to shoot
  public UnityEvent ShootBullets;

  public void OnShoot(InputValue input)
  {
    // OnShoot method is called by player input
    // Invoke event to fire bullets
    // The bullet spawner method is triggered in the editor 
    ShootBullets.Invoke();
  }
}
