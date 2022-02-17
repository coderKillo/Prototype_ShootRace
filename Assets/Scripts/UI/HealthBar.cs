using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField] private Slider m_slider;

    public void SetMaxHealth(int value)
    {
        m_slider.maxValue = value;
        m_slider.value = value;
    }

    public void SetHealth(int value)
    {
        m_slider.value = value;
    }
}
