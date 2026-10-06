using System;
using TMPro;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    public Camera mainCamera;
    public float walkSpeed;
    public float runSpeed;
    public float jumpForce;

    public float fallSpeedBonus;
    public LayerMask jumpLayers;
    public LayerMask lookLayers;

    private Animator _animator;

    private bool _isJumping;
    private Rigidbody _rb;
    private Player _player;


    private void Awake()
    {
        _player = GetComponent<Player>();
        _rb = GetComponent<Rigidbody>();
        _animator = GetComponentInChildren<Animator>();
    }

    public void RestartPlayerMovement()
    {
        _rb.constraints = RigidbodyConstraints.FreezeRotation;
        ChangeAnimationState("Idle");
    }
    private void Update()
    {
        var direction = Vector3.zero;

        if (_player.gameDirector.gameState != GameState.Gameplay || _player.isDead)
        {
            _rb.linearVelocity = Vector3.zero;
            return;
        }

        if (Keyboard.current.wKey.isPressed)
        {
            direction += Vector3.forward;
        }
        if (Keyboard.current.sKey.isPressed)
        {
            direction += Vector3.back;
        }
        if (Keyboard.current.aKey.isPressed)
        {
            direction += Vector3.left;
        }
        if (Keyboard.current.dKey.isPressed)
        {
            direction += Vector3.right;
        }

        var speed = walkSpeed;

        if(Keyboard.current.leftShiftKey.isPressed)
        {
            speed = runSpeed;
        }

        _isJumping = !CheckIfGrounded();

        if(Keyboard.current.spaceKey.wasPressedThisFrame && (CheckIfGrounded()))
        {
            Jump();
        }

        MovePlayer(direction, speed);
        LookAtMouse();

        SetWalkDirection(Vector3.SignedAngle(transform.forward, direction, Vector3.up));
    }

    void SetWalkDirection(float angle) 
    {
        _animator.SetFloat("WalkDirection", angle);
    }
    private void LookAtMouse()
    {
        var ray = mainCamera.ScreenPointToRay(Mouse.current.position.ReadValue());
        if (Physics.Raycast(ray, out var hit, 50, lookLayers))
        {
            var lookPos = hit.point;
            lookPos.y = transform.position.y;
            transform.LookAt(lookPos);
        }
    }

    private bool CheckIfGrounded()
    {
        if (Physics.Raycast(transform.position + Vector3.up * 0.1f, Vector3.down, out RaycastHit hit, 2f, jumpLayers))
        {
            _player.EnableShadow();
            _player.shadow.transform.position = hit.point + Vector3.up * 0.01f;
        }
        else
        {
            _player.DisableShadow();
        }
        if (Physics.Raycast(transform.position+ Vector3.up * 0.1f, Vector3.down, 0.3f, jumpLayers))
        {
            return true;
        }

        return false;
    }

    private void Jump()
    {
        _rb.AddForce(Vector3.up * jumpForce);
        _isJumping = true;
        ChangeAnimationState("Jump");

    }

    void MovePlayer(Vector3 dir, float speed) 
    {
        var yVelocity = _rb.linearVelocity;

        yVelocity.x =0;
        yVelocity.z = 0;

        if (yVelocity.y < 0) 
        {
            yVelocity.y -= fallSpeedBonus * Time.deltaTime;
        }

        if (!_isJumping)
        {
            if (dir.magnitude > 0)
            {
                ChangeAnimationState("Run");
            }
            else
            {
                ChangeAnimationState("Idle");
            }
        }

        _rb.linearVelocity = dir.normalized * speed + yVelocity;

    }

    public void ChangeAnimationState(string key)
    {
        _animator.SetBool("Idle", false);
        _animator.SetBool("Run", false);
        _animator.SetBool("Jump", false);
        _animator.SetBool("Die", false);
        _animator.SetBool("Win", false);
        _animator.SetBool(key, true);
    }

    public void PlayAlternativeFailAnimation()
    {
        ChangeAnimationState("Die2");
    }
}
