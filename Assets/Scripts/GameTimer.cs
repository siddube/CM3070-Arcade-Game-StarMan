/* ------------------------------------------------------------------------------
GameTimerManager Class
  This script handles
  1> Managing game score
--------------------------------------------------------------------------------*/
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class GameTimerManager : MonoBehaviour
{
    // Public Property set from editor, to reference score text
    [SerializeField] public TMP_Text ScoreText;
    // Public Property set from editor, to reference time text
    [SerializeField] public TMP_Text LevelTimeText;
    // Public Property set from editor, to reference game time duration
    [SerializeField] public float LevelTime;
    // Private property to hold reference game duration timer
    private float m_timer;
    // Event to invoke once the game duration is over
    public UnityEvent OnTimeUp;

    // Awake method
    private void Awake()
    {
        m_timer = LevelTime;
    }

    // Update method
    private void Update()
    {
        LevelTimeText.GetComponent<TextMeshProUGUI>().text = "Time: " + ((int)m_timer).ToString();
        // Call TickTimer every second with Time.deltaTime property
        TickTimer(Time.deltaTime);
    }

    // TiclTimer method
    private void TickTimer(float dt)
    {
        // Reduce time by one second with constant frame rate
        m_timer -= 1 * dt;
        // Check if game time is up
        CheckOutOfTime();
    }

    private void CheckOutOfTime()
    {
        // Check if game timer is up if it is negative or equal to zero
        if (m_timer <= 0f)
        {
            // Invoke on time up event
            OnTimeUp.Invoke();
        }
    }
}
