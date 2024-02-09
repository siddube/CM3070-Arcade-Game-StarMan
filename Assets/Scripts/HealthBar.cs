/* ------------------------------------------------------------------------------
HealthBar Class
  This script handles
  1> Managing health bar of the player ship
--------------------------------------------------------------------------------*/

using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    // Property set from editor, to reference slider used to set health
    [SerializeField] private Slider m_slider;
    // Property set from editor, to reference health gradient
    [SerializeField] private Gradient m_gradient;
    // Property set from editor, to reference fill image
    [SerializeField] private Image m_fillImage;
    // Property set from editor, to reference player combat manager
    [SerializeField] public PlayerCombatManager playerCombatManager;

    // Start method
    private void Start()
    {
        if (m_slider == null) { Debug.Log("ERR: HealthBar ====== Start() ====== Slider Component Not Found"); return; }
        if (m_gradient == null) { Debug.Log("ERR: HealthBar ====== Start() ====== Gradient Component Not Found"); return; }
        if (m_fillImage == null) { Debug.Log("ERR: HealthBar ====== Start() ====== Fill Image Component Not Found"); return; }
        if (playerCombatManager == null) { Debug.Log("ERR: HealthBar ====== Start() ====== Player Combat Manager Not Found"); return; }
    }

    // SetMaxHealth method
    public void SetMaxHealth(int health)
    {
        // Setup intial value on the health slider
        // Set max health value
        m_slider.maxValue = health;
        // Set current health value
        m_slider.value = health;
        // Set fill colour
        m_fillImage.color = m_gradient.Evaluate(1f);
    }

    // SetHealth method
    public void SetHealth(int health)
    {
        // Set current health value
        m_slider.value = health;
        // Set fill colour
        m_fillImage.color = m_gradient.Evaluate(m_slider.normalizedValue);
    }
}
