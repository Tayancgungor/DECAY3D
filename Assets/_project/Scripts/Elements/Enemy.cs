using NUnit.Framework;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;
//using static FreeZombie_EyesGlow;

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
    private Animator _animator;

    public LayerMask playerSeeLayerMask;
    private Vector3 _playerLastSeenPosition;
    private bool _isAttackInProgress;
    private CapsuleCollider _capsuleCollider;

    private Coroutine _attackCoroutine;
    public GameObject shadow;
    public Light mainLight;

    private HitFlash _hitFlash;
    public Transform chestBone;
    private bool _didSeePlayer;
    public ActionState actionState;
    public AnimationState currentAnimationState;
    private AnimationState _animationStateBeforeTakeDamage;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        _navMeshAgent = GetComponent<NavMeshAgent>();
        _animator = GetComponentInChildren<Animator>();
        _capsuleCollider = GetComponent<CapsuleCollider>();
        _hitFlash = GetComponent<HitFlash>();
    }

    public void StartEnemy(Player player)
    {
        _currenthealth = startHealth;
        healthBar.SetHealthBar(1);
        _player = player;
    }

    private void Update()
    {
        if(actionState == ActionState.Death || _player.gameDirector.gameState != GameState.Gameplay)
        {
            return;
        }


        //Decide Action State
        if (GetDistanceFromPlayer() < playerAttackDistance)
        {
            actionState = ActionState.Attack;
        }
        else if (GetDistanceFromPlayer() < playerWalkTowardsDistance && !_isAttackInProgress)
        {
            if (GetIfEnemySeesPlayer())
            {
                actionState = ActionState.WalkTowardsPlayer;
            }
            else if (_playerLastSeenPosition != Vector3.zero)
            {
                if ((transform.position- _playerLastSeenPosition).magnitude < 1f)
                {
                    actionState = ActionState.Standing;
                }
                else
                {
                    actionState = ActionState.WalkTowardsPlayerLastSeenPosition;

                }
            }
        }

        //Action State Logic
        if (actionState == ActionState.WalkTowardsPlayer)
        {
            WalkTowardsPlayer();
            if (!_didSeePlayer)
            {
                _didSeePlayer = true;
                _player.gameDirector.audioManager.PlayZombieScreamAS();
            }
        }

        else if (actionState == ActionState.WalkTowardsPlayerLastSeenPosition)
        {
            WalkTowardsPlayerLastSeenPosition();
        }
        else if(actionState == ActionState.Attack)
        {
            AttackPlayer();
        }
        else if (actionState== ActionState.Standing)
        {
            SwitchAnimation(AnimationState.Idle);
            _navMeshAgent.isStopped = true;
        }
    }

    private void AttackPlayer()
    {
        if (!_isAttackInProgress)
        {
            _isAttackInProgress = true;
            _navMeshAgent.isStopped = true;
            SwitchAnimation(AnimationState.Attack, true);
            _attackCoroutine =StartCoroutine(AttackCooldown(1.2f));
        }
    }

    IEnumerator AttackCooldown(float delay)
    {
        yield return new WaitForSeconds(delay);

        if (GetDistanceFromPlayer() < playerAttackDistance)
        {
            _player.TakeDamage(1);
        }
        _isAttackInProgress = false;
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

    public void SetPlayerDead()
    {
        if (_attackCoroutine !=null)
        {
            StopCoroutine(_attackCoroutine);
        }

        _navMeshAgent.isStopped = true;
        SwitchAnimation(AnimationState.Idle);
    }

    private float GetDistanceFromPlayer()
    {
        return (_player.transform.position - transform.position).magnitude;
    }

    private void WalkTowardsPlayer()
    {
        if (currentAnimationState != AnimationState.GetHit)
        {
            _navMeshAgent.SetDestination(_player.transform.position);
            _navMeshAgent.isStopped = false;
            SwitchAnimation(AnimationState.Walk);
        }
    }

    private void WalkTowardsPlayerLastSeenPosition()
    {
        if (currentAnimationState != AnimationState.GetHit)
        {
            _navMeshAgent.SetDestination(_playerLastSeenPosition);
            _navMeshAgent.isStopped = false;
            SwitchAnimation(AnimationState.Walk);
        }
    }

    private void SwitchAnimation(AnimationState desiredAnimationState, bool forcePlayAnimation = false)
    {
        if (desiredAnimationState == AnimationState.Walk && (currentAnimationState != AnimationState.Walk || forcePlayAnimation))
        {
            _animator.ResetTrigger("Idle");
            _animator.CrossFade("Walk", .1f);
            currentAnimationState = AnimationState.Walk;
        }
        else if (desiredAnimationState == AnimationState.Idle && (currentAnimationState != AnimationState.Idle || forcePlayAnimation))
        {
            _animator.SetTrigger("Idle");
            currentAnimationState = AnimationState.Idle;
        }
        else if (desiredAnimationState == AnimationState.GetHit && (currentAnimationState != AnimationState.GetHit || forcePlayAnimation))
        {
            _animator.SetTrigger("GetHit");
            currentAnimationState = AnimationState.GetHit;
            StartCoroutine(UpperBodyMaskCoroutine(.3f, .5f));
        }
        else if (desiredAnimationState == AnimationState.Attack && (currentAnimationState != AnimationState.Attack || forcePlayAnimation))
        {
            _animator.SetTrigger("Attack");
            currentAnimationState = AnimationState.Attack;
        }
        else if (desiredAnimationState == AnimationState.Death && (currentAnimationState != AnimationState.Death || forcePlayAnimation))
        {
            _animator.CrossFade("Death", .1f);
            currentAnimationState = AnimationState.Death;
        }
    }

    IEnumerator UpperBodyMaskCoroutine(float delay, float amount)
    {
        _animator.SetLayerWeight(1, amount);
        yield return new WaitForSeconds(delay);
        _animator.SetLayerWeight(1, 0);
    }
    public void TakeDamage(int damage)
    {
        _currenthealth -= damage;
        StartCoroutine(PlayTakeDamageCoroutine());
        healthBar.SetHealthBar((float)_currenthealth / startHealth);
        _player.gameDirector.fxManager.PlayFloatingText(damage, transform.position);
        _hitFlash.PlayHitFlash();
        _player.gameDirector.audioManager.PlayZombieImpactAS();

        if (actionState == ActionState.Standing)
        {
            actionState = ActionState.WalkTowardsPlayer;
        }

        if (_currenthealth <= 0)
        {
            Die();
        }
    }

    IEnumerator PlayTakeDamageCoroutine()
    {
        CancelAttack();

        if (currentAnimationState != AnimationState.GetHit)
        {
            _animationStateBeforeTakeDamage = currentAnimationState;

        }
        _navMeshAgent.isStopped = true;
        SwitchAnimation(AnimationState.GetHit);
        yield return new WaitForSeconds(.3f);
        SwitchAnimation(_animationStateBeforeTakeDamage);
    }


    private void Die()
    {
        CancelAttack();
        actionState = ActionState.Death;
        _animationStateBeforeTakeDamage = AnimationState.Death;
        _navMeshAgent.isStopped = true;
        _capsuleCollider.enabled = false;
        SwitchAnimation(AnimationState.Death);
        shadow.SetActive(false);
        mainLight.enabled = false;
        _player.gameDirector.fxManager.PlayZombieDestroyPSDelayed(2.7f , chestBone);
        GetComponentInParent<Level>().EnemyDestroyed(this);
        Destroy(gameObject, 3f);
    }

    private void CancelAttack()
    {
        _isAttackInProgress = false;
        if (_attackCoroutine != null)
        {
            StopCoroutine(_attackCoroutine);
        }
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

public enum AnimationState
{
    Idle,
    Walk,
    Attack,
    GetHit,
    Death,
}