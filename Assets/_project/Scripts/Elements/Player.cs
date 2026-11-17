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
    public ParticleSystem collectedPS;
    public ParticleSystem coinCollectedPS;
    public Weapon shotgun;
    public Weapon machineGun;
    private WeaponType _currentWeaponType;

    private void Awake()
    {
        _playerMovement = GetComponent<PlayerMovement>();
    }
    public void Update()
    {
        if (transform.position.y < -10f && gameDirector.gameState == GameState.Gameplay)
        {
            gameDirector.LevelFailed();
        }
        if (Keyboard.current.digit1Key.wasPressedThisFrame)
        {
            SwitchToMachineGun();
        }

        if (Keyboard.current.digit2Key.wasPressedThisFrame)
        {
            SwitchToShotgun();
        }

    }

    public void SwitchToMachineGun()
    {
        if (_currentWeaponType != WeaponType.MachineGun)
        {
            machineGun.gameObject.SetActive(true);
            shotgun.gameObject.SetActive(false);
            _playerMovement.ChangeAnimationState("ChangeWeapon");
            _currentWeaponType = WeaponType.MachineGun;
            _playerMovement._isSwitchingWeapon = true;
            _playerMovement.SetUpperBodyLayerWeightTo1();
            Invoke(nameof(SetSwitchingWeaponFalse), 0.5f);
        }


    }

    public void SwitchToShotgun()
    {
        if (_currentWeaponType != WeaponType.Shotgun)
        {
            shotgun.gameObject.SetActive(true);
            machineGun.gameObject.SetActive(false);
            _playerMovement.ChangeAnimationState("ChangeWeapon");
            _currentWeaponType = WeaponType.Shotgun;
            _playerMovement._isSwitchingWeapon = true;
            _playerMovement.SetUpperBodyLayerWeightTo1();
            Invoke(nameof(SetSwitchingWeaponFalse), 0.5f);
        }
    }

    void SetSwitchingWeaponFalse()
    {
        _playerMovement._isSwitchingWeapon = false;
        _playerMovement.SetUpperBodyLayerWeightTo0();
    }

    public void RestartPlayer()
    {
        transform.position = Vector3.zero;
        _currentHealth = startHealth;
        healthBar.SetFillBar(1);
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
        var defenceBonus = gameDirector.upgradeManager.defenceUpgradeCount;
        _currentHealth -= damage - defenceBonus;
        damage = Mathf.Max(damage, 1);
        healthBar.SetFillBar((float)_currentHealth / startHealth);
        gameDirector.uIManager.getHitUI.ShowGetHitGradient();
        gameDirector.audioManager.PlayHitAS();
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

    public void CollectableCollected(CollectableType type)
    {
        if (type == CollectableType.Time)
        {
            collectedPS.Play();
            gameDirector.timerManager.CollectableCollected();
            gameDirector.audioManager.PlayCollectedAS();
        }
        else if (type == CollectableType.Coin)
        {
            coinCollectedPS.Play();
            gameDirector.coinManager.CoinCollected(1);
            gameDirector.audioManager.PlayCollectedAS();
        }

    }

    public Vector3 GetCurrentDirection()
    {
        return _playerMovement.GetCurrentDirection();
    }

}
