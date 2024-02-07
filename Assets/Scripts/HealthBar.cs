using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [field: SerializeField] private Slider m_slider;
    [field: SerializeField] private Gradient m_gradient;
    [field: SerializeField] private Image m_fillImage;
    [SerializeField] public PlayerCombatManager playerCombatManager;

    public void SetMaxHealth(int health)
    {
        if (m_slider == null) { Debug.Log("ERR: HealthBar ====== SetMaxHealth() ====== Slider Component Not Found"); return; }
        m_slider.maxValue = health;
        m_slider.value = health;
        m_fillImage.color = m_gradient.Evaluate(1f);
    }

    public void SetHealth(int health)
    {
        m_slider.value = health;
        m_fillImage.color = m_gradient.Evaluate(m_slider.normalizedValue);
    }
}
