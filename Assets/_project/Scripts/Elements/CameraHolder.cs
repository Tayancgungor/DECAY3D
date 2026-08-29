using UnityEngine;

public class CameraHolder : MonoBehaviour
{
    public Transform followObject;
    public float rotationOffset;
    private Vector3 _vel;

    public float smoothTime;

    private void FixedUpdate()
    {
        var followPos = Vector3.SmoothDamp(transform.position, followObject.position + followObject.forward * rotationOffset, ref _vel, smoothTime);
        transform.position = followPos;
    }
}
