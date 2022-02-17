using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damageable : MonoBehaviour
{
    [SerializeField] private HealthBar m_healthBar;
    [SerializeField] private int m_maxHealth = 100;

    private int m_currentHealth;

    void Start()
    {
        m_healthBar.SetMaxHealth(m_maxHealth);
        m_currentHealth = m_maxHealth;
    }

    public void TakeDamage(int damage)
    {
        m_currentHealth -= damage;
        m_healthBar.SetHealth(m_currentHealth);
    }
}
