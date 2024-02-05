/* ------------------------------------------------------------------------------
<PlayerMovementManager Class>
  This script is the game manager that handles
  1> Accepting keyboard input for the player space ship
  2> Moving player space ship
  3> Rotate the player space ship
  3> Keeping the player space ship on screen
<PlayerMovementManager Class>
--------------------------------------------------------------------------------*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovementManager : MonoBehaviour
{
    // Property accessible from editor, to reference thrust particle system 
    [field: SerializeField] ParticleSystem ThrustParticleSystem;
    // Private property, yet accessible from editor, to control +ve and -ve thrust on player space ship
    [field: SerializeField] private float m_thrust = 10f;

    // Private property, yet accessible from editor, to set backward thrust threshold
    [field: SerializeField] private float m_backwardThrustThreshold = 0.2f;

    // Private property, yet accessible from editor, to set rotationSteer speed of player space ship
    [field: SerializeField] private float m_rotateSteerSpeed = 75f;
    // Private vector to store the composite vector from player controls
    private Vector2 m_moveInupt;
    // Private reference to the rigid body component on the player space ship
    private Rigidbody m_rb;
    // Private reference to the audio source component on the player space ship
    private AudioSource audioSource;
    // Private bool value to represent thrust in upward movement
    private bool isSpaceShipThrusting = false;

    private void Awake()
    {
        // Assign reference of the rigid body
        m_rb = this.gameObject.GetComponent<Rigidbody>();
        // Assign reference of the audio source
        audioSource = this.gameObject.GetComponent<AudioSource>();
    }
    private void Update()
    {
        // Call method to move the player space ship
        MoveSpaceship();
        // Call method to rotate the player space ship
        RotateSpaceship();
        // Call method to keep the player space ship on screen
        KeepShipOnScreen();
    }

    public void OnMove(InputValue input)
    {
        // Read player input of arrow keys to direct movement of the ship
        // If player is pressing up arrow
        // Then set thrusting bool to true
        // This boolean is used to toggle thrust audio and particle fx 
        m_moveInupt = input.Get<Vector2>();
        if (m_moveInupt.y > 0.2f)
        {
            isSpaceShipThrusting = true;
        }
        else
        {
            isSpaceShipThrusting = false;
        }
        ToggleThrustParticleAndAudioFX(isSpaceShipThrusting);
    }

    // Private method to move the spaceship
    private void MoveSpaceship()
    {
        // If rigid body is not referenced, then throw a warning to the console log
        if (!m_rb) { Debug.Log("ERR: SpaceshipMovement ====== MoveSpaceship() ====== Rigid Body Not Found"); return; }

        // If thrust particle system is not set, then throw a warning to the console log
        if (!ThrustParticleSystem) { Debug.Log("ERR: SpaceshipMovement ====== MoveSpaceship() ====== Thrust Particle System Not Found"); return; }

        // Calculate thrust value from up and down arrow
        // Also access the direction of thrust
        float thrustFromInput = m_moveInupt.y;

        // Calculate the movement direction and normalise the vector
        Vector3 m_moveDirection = new Vector3(m_moveInupt.x, m_moveInupt.y).normalized;

        // Rotate the player to look forward in the movement direction
        Vector3 m_rotatedDirection = transform.TransformDirection(m_moveDirection);

        // Check if player is pressing up key
        if (thrustFromInput >= 0)
        {
            // If yes, add force to move the player spaceship forward independent from the frame rate
            m_rb.AddForce(m_rotatedDirection * m_moveInupt.y * m_thrust * Time.deltaTime, ForceMode.Acceleration);

        }
        // Check if player is pressing up key
        if (thrustFromInput < 0 && m_rb.velocity.magnitude >= m_backwardThrustThreshold)
        {
            // If yes, add force to move the player spaceship backwards independent from the frame rate and halt
            m_rb.AddForce(m_rotatedDirection * -m_moveInupt.y * m_thrust * Time.deltaTime, ForceMode.Acceleration);
        }
    }

    // Private method to rotate the spaceship
    private void RotateSpaceship()
    {
        // If rigid body is not referenced, then throw a warning to the console log
        if (!m_rb) { Debug.Log("ERR: SpaceshipMovement ====== RotateSpaceship() ====== Rigid Body Not Found"); return; }

        // Calculate the Euler angle velocity from keyboard input
        Vector3 m_EulerAngleVelocity = new Vector3(0, 0, m_rotateSteerSpeed * -m_moveInupt.x);
        // Rotate the ship independent of the frame rate
        Quaternion deltaRotation = Quaternion.Euler(m_EulerAngleVelocity * Time.deltaTime);
        // Set rotation on the rigid body
        m_rb.MoveRotation(m_rb.rotation * deltaRotation);
    }

    // Private method to keep the spaceship on screen
    private void KeepShipOnScreen()
    {
        // Set screen bounds for X axis and Y axis
        // Float values to define bounds in viewport position 
        float minX = 0.1f, minY = 0.1f;
        float maxX = 0.9f, maxY = 0.9f;

        // Get viewport position from world position of space ship
        Vector3 viewportPos = Camera.main.WorldToViewportPoint(this.gameObject.transform.position);

        // Check player space ship against screen bounds
        // If approaching bounds reduce velocity
        // and not move the space ship beyond bounds
        if (viewportPos.x <= minX ||
        viewportPos.x >= maxX ||
        viewportPos.y < minY ||
        viewportPos.y > maxY)
        {
            this.gameObject.transform.position = new Vector3(this.transform.position.x, this.transform.position.y, 0f);
            m_rb.velocity = Vector3.zero;
        }
    }

    private void ToggleThrustParticleAndAudioFX(bool playThrust)
    {
        if (!playThrust)
        {
            ThrustParticleSystem.Stop();
            audioSource.Stop();
        }
        else
        {
            ThrustParticleSystem.Play();
            audioSource.Play();
        }
    }
}
