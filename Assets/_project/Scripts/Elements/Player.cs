using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    public GameDirector gameDirector;
    public int startHealth;
    private int _currentHealth;

    public HealthBar healthBar;
    private PlayerMovement _playerMovement;
    public bool isDead;

    public GameObject shadow;

    private void Awake()
    {
        _playerMovement = GetComponent<PlayerMovement>();
    }
    private void Update()
    {
        if (transform.position.y < -10f && gameDirector.gameState == GameState.Gameplay)
        {
            gameDirector.LevelFailed();
        }
    }
    public void RestartPlayer()
    {
        transform.position = Vector3.zero;
        _currentHealth = startHealth;
        healthBar.SetHealthBar(1);
        _playerMovement.RestartPlayerMovement();
        isDead = false;
        EnableShadow();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Potion"))
        {
            other.gameObject.SetActive(false);
            _playerMovement.ChangeAnimationState("Win");
            gameDirector.fxManager.PlayPotionCollectPS(other.transform.position);
            gameDirector.LevelCompleted();

        }

    }
    public void TakeDamage(int damage)
    {
        if (isDead)
        {
            return;
        }
        _currentHealth -= damage;
        healthBar.SetHealthBar((float)_currentHealth / startHealth);
        if (_currentHealth <= 0)
        {
            Die();
        }
    }
    private void Die()
    {
        isDead = true;
        _playerMovement.ChangeAnimationState("Die");
        gameDirector.LevelFailed();
        DisableShadow();
    }

    public void DisableShadow()
    {
        shadow.SetActive(false);
    }

    internal void EnableShadow()
    {
        shadow.SetActive(true);
    }

    public void PlayAlternativeFailAnimation()
    {
        _playerMovement.PlayAlternativeFailAnimation();
    }
}
