using UnityEngine;

public class CameraHolder : MonoBehaviour
{
    public Transform followObject;
    public float rotationOffset;
    private Vector3 _vel;

    public float smoothTime;

    private void FixedUpdate()
    {
        var targetPos = followObject.position + followObject.forward * rotationOffset;
        targetPos.y= 0;
        var followPos = Vector3.SmoothDamp(transform.position, targetPos, ref _vel, smoothTime);
        transform.position = followPos;
    }
}
