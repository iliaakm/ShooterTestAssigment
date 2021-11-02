using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField]
    float moveSpeed = 6;
    [SerializeField]
    float jumpHeight = 2;
    [SerializeField]
    float gravity = 20;
    [Range(0, 10), SerializeField]
    float airControl = 5;

    Vector3 moveDirection = Vector3.zero;
    CharacterController controller;

    private void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    private void FixedUpdate()
    {
        var input = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        input *= moveSpeed;
        input = transform.TransformDirection(input);

        if(controller.isGrounded)
        {
            moveDirection = input;

            if(Input.GetButton("Jump"))
            {
                moveDirection.y = Mathf.Sqrt(2 * gravity * jumpHeight);
            }
            else
            {
                moveDirection.y = 0;
            }    
        }
        else
        {
            input.y = moveDirection.y;
            moveDirection = Vector3.Lerp(moveDirection, input, airControl * Time.deltaTime);
        }

        moveDirection.y -= gravity * Time.deltaTime;
        controller.Move(moveDirection  * Time.deltaTime);
    }
}
