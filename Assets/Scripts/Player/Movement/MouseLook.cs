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
    [SerializeField]
    Transform head;

    float yaw = 0f;
    float pitch = 0f;

    Quaternion bodyStartOrientation;
    Quaternion headStartOrientation;


    private void Start()
    {
        bodyStartOrientation = transform.localRotation;
        headStartOrientation = head.localRotation;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        if (Application.isEditor)
            if (Time.time < GameConfig.Editor.mouseInput)      //start up filter
            {
                return;
            }

        var horizontal = Input.GetAxis(GameConfig.Axis.AxisMouseHorizontal) * Time.deltaTime * turnSpeed;
        var vertical = Input.GetAxis(GameConfig.Axis.AxisMouseVertical) * Time.deltaTime * turnSpeed * (mouseInverted ? -1f : 1f);

        yaw += horizontal;
        pitch += vertical;

        pitch = Mathf.Clamp(pitch, headLowerAngleLimit, headUpperAngleLimit);

        var bodyRotation = Quaternion.AngleAxis(yaw, Vector3.up);
        var headRotation = Quaternion.AngleAxis(pitch, Vector3.right);

        transform.localRotation = bodyRotation * bodyStartOrientation;
        head.localRotation = headRotation * headStartOrientation;
    }
}
