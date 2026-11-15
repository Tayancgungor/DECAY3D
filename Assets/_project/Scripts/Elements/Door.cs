using DG.Tweening;
using UnityEngine;
using UnityEngine.InputSystem;

public class Door : MonoBehaviour
{
    public Transform leftWing;
    public Transform rightWing;
    bool _isDoorOpen;
    bool _isPlayerinRange;
    private void Update()
    {
        if (Keyboard.current.eKey.wasPressedThisFrame && _isPlayerinRange)
        {
            if (GetComponentInParent<Level>().IsLevelCleared())
            {
                if (_isDoorOpen)
                {
                    CloseDoor();
                }
                else
                {
                    OpenDoor();

                }
            }
            else
            {
                GetComponentInParent<Level>().ShowClearLevelMessage();
            }
        }
    }

    public void OpenDoor()
    {
        _isDoorOpen = true;
        leftWing.DOLocalMoveZ(1.4f, 1f);
        rightWing.DOLocalMoveZ(-2.7f, 1f);
    }
    public void CloseDoor() 
    {
        _isDoorOpen = false;
        leftWing.DOLocalMoveZ(0f, 1f);
        rightWing.DOLocalMoveZ(-1.43f, 1f);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _isPlayerinRange = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _isPlayerinRange = false;
        }
    }
}
