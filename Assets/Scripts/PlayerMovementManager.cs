/* ------------------------------------------------------------------------------
PlayerMovementManager Class
  * This script handles
  1> Accepting keyboard input for the player space ship movement
  2> Moving player space ship
  3> Rotate the player space ship
  4> Keeping the player space ship on screen
  5> To trigger audio and particle fx when player is thrusting the space ship
--------------------------------------------------------------------------------*/
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovementManager : MonoBehaviour
{
	// Property accessible from editor, to reference thrust particle system 
	[SerializeField] ParticleSystem ThrustParticleSystem;
	// Private property, yet accessible from editor, to control +ve and -ve thrust on player space ship
	[SerializeField] private float m_thrust = 10f;
	// Private property, yet accessible from editor, to set backward thrust threshold
	[SerializeField] private float m_backwardThrustThreshold = 0.2f;

	// Private property, yet accessible from editor, to set rotationSteer speed of player space ship
	[SerializeField] private float m_rotateSteerSpeed = 75f;
	// Private vector to store the composite vector from player controls
	private Vector2 m_moveInupt;
	// Private reference to the rigid body component on the player space ship
	private Rigidbody m_rb;
	// Private reference to the audio source component on the player space ship
	private AudioSource m_audioSource;
	// Private bool value to represent thrust in up arrow key press
	private bool m_isSpaceShipThrusting = false;

	// Awake method
	private void Awake()
	{
		// Assign reference of the rigid body
		m_rb = this.gameObject.GetComponent<Rigidbody>();
		// Assign reference of the audio source
		m_audioSource = this.gameObject.GetComponent<AudioSource>();
	}
	// Update method
	private void Update()
	{
		// Call method to move the player space ship
		MoveSpaceship();
		// Call method to rotate the player space ship
		RotateSpaceship();
		// Call method to keep the player space ship on screen
		KeepShipOnScreen();
	}

	// Public method called from input system
	public void OnMove(InputValue input)
	{
		// Read player input of arrow keys to direct movement of the ship
		m_moveInupt = input.Get<Vector2>();
		if (m_moveInupt.y > 0.2f)
		{
			// If player is pressing up arrow
			// This boolean is used to toggle thrust audio and particle fx 
			m_isSpaceShipThrusting = true;
		}
		else
		{
			// Else set the boolean toggle thrust to false
			m_isSpaceShipThrusting = false;
		}
		// Call the method to toggle thrust particle fx and audio fx
		// Pass the boolean value set above as parameter
		ToggleThrustParticleAndAudioFX(m_isSpaceShipThrusting);
	}

	// Private method to move the spaceship
	private void MoveSpaceship()
	{
		// If rigid body is not referenced, then throw a warning to the console log
		if (!m_rb) { Debug.Log("ERR: SpaceshipMovementManager ====== MoveSpaceship() ====== Rigid Body Not Found"); return; }

		// If thrust particle system is not set, then throw a warning to the console log
		if (!ThrustParticleSystem) { Debug.Log("ERR: SpaceshipMovementManager ====== MoveSpaceship() ====== Thrust Particle System Not Found"); return; }

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
		if (!m_rb) { Debug.Log("ERR: SpaceshipMovementManager ====== RotateSpaceship() ====== Rigid Body Not Found"); return; }

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
		// If rigid body is not referenced, then throw a warning to the console log
		if (!m_rb) { Debug.Log("ERR: SpaceshipMovementManager ====== KeepShipOnScreen() ====== Rigid Body Not Found"); return; }
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

	// Method to toggle thrust particle and sound fx
	private void ToggleThrustParticleAndAudioFX(bool playThrust)
	{
		// If thrust particle system is not set, then throw a warning to the console log
		if (!ThrustParticleSystem) { Debug.Log("ERR: SpaceshipMovementManager ====== MoveSpaceship() ====== Thrust Particle System Not Found"); return; }
		// If thrusting is false
		// Stop thrust sound and particle fx
		if (!playThrust)
		{
			ThrustParticleSystem.Stop();
			m_audioSource.Stop();
		}
		// Else if thrusting is true
		// Start thrust sound and particle fx
		else
		{
			ThrustParticleSystem.Play();
			m_audioSource.Play();
		}
	}
}
