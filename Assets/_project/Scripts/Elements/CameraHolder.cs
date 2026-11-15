using DG.Tweening;
using UnityEngine;

public class CameraHolder : MonoBehaviour
{
    public Transform followObject;
    public float rotationOffset;
    private Vector3 _vel;

    public float smoothTime;
    public Camera mainCamera;
    private Vector3 _originalCameraPos;

    void Start()
    {
        _originalCameraPos = mainCamera.transform.localPosition;
    }
    private void FixedUpdate()
    {
        var targetPos = followObject.position + followObject.forward * rotationOffset;
        targetPos.y= 0;
        var followPos = Vector3.SmoothDamp(transform.position, targetPos, ref _vel, smoothTime);
        transform.position = followPos;
    }

    public void ShakeCamera(float duration, float magnitude)
    {
        mainCamera.transform.DOKill();
        mainCamera.transform.localPosition = _originalCameraPos;
        mainCamera.transform.DOShakePosition(duration, magnitude);
    }
}
