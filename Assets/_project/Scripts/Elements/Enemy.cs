using System;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    public HealthBar healthBar;
    public int startHealth;
    private int _currenthealth;
    public float speed;
    public float playerWalkTowardsDistance;
    public float playerAttackDistance;
    private Rigidbody _rb;
    private NavMeshAgent _navMeshAgent;
    private Player _player;

    public LayerMask playerSeeLayerMask;
    private Vector3 _playerLastSeenPosition;

    public ActionState actionState;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        _navMeshAgent = GetComponent<NavMeshAgent>();
    }

    public void StartEnemy(Player player)
    {
        _currenthealth = startHealth;
        healthBar.SetHealthBar(1);
        _player = player;
    }

    private void Update()
    {
        if(actionState == ActionState.Death)
        {
            return;
        }


        //Decide Action State
        if (GetDistanceFromPlayer() < playerAttackDistance)
        {
            actionState = ActionState.Attack;
        }
        else if (GetDistanceFromPlayer() < playerWalkTowardsDistance)
        {
            if (GetIfEnemySeesPlayer())
            {
                actionState = ActionState.WalkTowardsPlayer;
            }
            else if (_playerLastSeenPosition != Vector3.zero)
            {
                actionState = ActionState.WalkTowardsPlayerLastSeenPosition;
            }
        }

        //Action State Logic
        if (actionState == ActionState.WalkTowardsPlayer)
        {
            WalkTowardsPlayer();
        }

        else if (actionState == ActionState.WalkTowardsPlayerLastSeenPosition)
        {
            WalkTowardsPlayerLastSeenPosition();
        }
        else if(actionState == ActionState.Attack)
        {
            AttackPlayer();
        }
        else if (actionState == ActionState.Standing)
        {
            StopEnemy();
        }


    }

    private void AttackPlayer()
    {

    }

    private bool GetIfEnemySeesPlayer()
    {
        if (Physics.Raycast(transform.position + Vector3.up, _player.transform.position - transform.position, playerWalkTowardsDistance, playerSeeLayerMask))
        {
            return false;
        }
        _playerLastSeenPosition = _player.transform.position;
        
        return true;
    }

    private void StopEnemy()
    {
        _rb.linearVelocity = Vector3.zero;
    }

    private float GetDistanceFromPlayer()
    {
        return (_player.transform.position - transform.position).magnitude;
    }

    private void WalkTowardsPlayer()
    {
        _navMeshAgent.SetDestination(_player.transform.position);
    }

    private void WalkTowardsPlayerLastSeenPosition()
    {
        _navMeshAgent.SetDestination(_playerLastSeenPosition);
    }

    public void TakeDamage(int damage)
    {
        _currenthealth -= damage;
        healthBar.SetHealthBar((float)_currenthealth / startHealth);
        if (_currenthealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        Destroy(gameObject);
    }
}

public enum ActionState
{
    Standing,
    WalkTowardsPlayer,
    WalkTowardsPlayerLastSeenPosition,

    Attack,
    Death,
}
