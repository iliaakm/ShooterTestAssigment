using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseLook : MonoBehaviour
{
    [SerializeField]
    float turnSpeed = 90f;
    [SerializeField]
    float headUpperAngleLimit = 85f;
    [SerializeField]
    float headLowerAngleLimit = -80f;
    [SerializeField]
    bool mouseInverted = true;

    float yaw = 0f;
    float pitch = 0f;

    Quaternion bodyStartOrientation;
    Quaternion headStartOrientation;

    Transform head;

    private void Start()
    {
        head = GetComponentInChildren<Camera>().transform;

        bodyStartOrientation = transform.localRotation;
        headStartOrientation = head.localRotation;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void FixedUpdate()
    {
        var horizontal = Input.GetAxis("Mouse X") * Time.deltaTime * turnSpeed;
        var vertical = Input.GetAxis("Mouse Y") * Time.deltaTime * turnSpeed * (mouseInverted ? -1f : 1f);
        
        yaw += horizontal;
        pitch += vertical;

        pitch = Mathf.Clamp(pitch, headLowerAngleLimit, headUpperAngleLimit);

        var bodyRotation = Quaternion.AngleAxis(yaw, Vector3.up);
        var headRotation = Quaternion.AngleAxis(pitch, Vector3.right);

        transform.localRotation = bodyRotation * bodyStartOrientation;
        head.localRotation = headRotation * headStartOrientation;
    }
}
