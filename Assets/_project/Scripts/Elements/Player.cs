using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    public int startHealth;
    private int _currentHealth;

    public HealthBar healthBar;

    private void Update()
    {
        if ((Keyboard.current.hKey.wasPressedThisFrame))
        {
            TakeDamage(1);
        }

    }
    public void RestartPlayer()
    {
        transform.position = Vector3.zero;
        _currentHealth = startHealth;
        gameObject.SetActive(true);

    }
    public void TakeDamage(int damage)
    {
        _currentHealth -= damage;
        healthBar.SetHealthBar((float)_currentHealth / startHealth);
        if (_currentHealth <= 0)
        {
            Die();
        }
    }
    private void Die()
    {
        gameObject.SetActive(false);
    }
}
