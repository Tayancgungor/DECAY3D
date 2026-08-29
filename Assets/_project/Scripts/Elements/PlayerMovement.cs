using System;
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

    private Rigidbody _rb;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
    }
    private void Update()
    {
        var direction = Vector3.zero;
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
        


        if(Keyboard.current.spaceKey.wasPressedThisFrame && (CheckIfGrounded()))
        {
            Jump();

        }

        MovePlayer(direction, speed);
        LookAtMouse();
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
        if (Physics.Raycast(transform.position+ Vector3.up * 0.1f, Vector3.down, 0.3f, jumpLayers))
        {
            return true;
        }

        return false;
    }

    private void Jump()
    {
        _rb.AddForce(Vector3.up * jumpForce);
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

        _rb.linearVelocity = dir.normalized * speed + yVelocity;
    }

}
