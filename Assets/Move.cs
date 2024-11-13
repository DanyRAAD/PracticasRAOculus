using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class Move : MonoBehaviour
{

    public float speed;
    public float runMultiplier;
    public float gravity = -9.81f;
    public float jumHeigth;
    public float rotationSpeed;

    private CharacterController charaController;
    private Vector3 velocity;
    private bool isRunning = false;
    private bool isGrounded;
    private PlayerInputActions inputActions;
    private Vector2 moveInput;
    private float rotateInput;

    private void Awake()
    {
        charaController = GetComponent<CharacterController>();
        inputActions = new PlayerInputActions();
        inputActions.Player.Move.performed += ctx => moveInput = ctx.ReadValue<Vector2>();
        inputActions.Player.Move.canceled += ctx => moveInput = Vector2.zero;
        inputActions.Player.Run.performed += ctx => isRunning = true;
        inputActions.Player.Run.canceled += ctx => isRunning = false;
        inputActions.Player.Jump.performed += ctx => Jump();

        inputActions.Player.Rotation.performed += ctx => rotateInput = ctx.ReadValue<float>();
        inputActions.Player.Rotation.canceled += ctx => rotateInput = 0;

    }

    void OnEnable()
    {
        inputActions.Enable();
    }

    void OnDisable()
    {
        inputActions.Disable();   
    }

    private void Jump()
    {
        if(isGrounded) 
        {
            velocity.y = Mathf.Sqrt(jumHeigth * -2f * gravity);
        
        }
    }


    // Update is called once per frame
    void Update()
    {
        isGrounded = charaController.isGrounded;
        if(isGrounded && velocity.y < 0 )
        {
            velocity.y = -2;
        }

        Vector3 move = new Vector3(moveInput.x, 0, moveInput.y);
        move = transform.TransformDirection(move);
        float currentSpeed = isRunning ? speed*runMultiplier :speed;
        charaController.Move(move*currentSpeed*Time.deltaTime);
        float rotation = rotateInput * rotationSpeed * Time.deltaTime;
        transform.Rotate(0, rotation, 0);


        velocity.y += gravity * Time.deltaTime;
        charaController.Move(velocity * Time.deltaTime);
    }
}
