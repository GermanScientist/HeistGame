using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class Player : Actor {
    //Serialized camera values
    [Header("Camera values")]
    [SerializeField] private float sensitivity = 100;
    [SerializeField] private float cameraClamp = 85f;
    
    //Serialized movement values
    [Header("Movement values")]
    [SerializeField] private float gravity = -9.8f;
    [SerializeField] private float walkingSpeed = 15f;
    [SerializeField] private float jumpRaycastDistance = 1.1f;

    //Non-serialized private properties
    private Transform cameraTransform;
    private float xRotation = 0f;
    private CharacterController characterController;
    private Vector3 velocity;

    //Awake gets called once before Start
    private void Awake() {
        cameraTransform = Camera.main.gameObject.transform;
        characterController = GetComponent<CharacterController>();
    }

    //Start gets called once before Update
    private void Start() {
        LockMouse();
    }

    //Update gets called every frame
    private void Update() {
        MovePlayerCamera();
        MovePlayer();
    }

    //Rotate the player using the player's mouse input
    private void MovePlayerCamera() {
        //Set the mouse values based on the mouse input multiplied by the mouse sensitivity
        float mouseX = Input.GetAxis("Mouse X") * sensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * sensitivity * Time.deltaTime;

        //Set the rotation
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -cameraClamp, cameraClamp);

        //Apply the rotation
        cameraTransform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        transform.Rotate(Vector3.up * mouseX);
    }

    //Move the player character using the player's input
    private void MovePlayer() {
        //Reset the velocity when the player is grounded
        if (IsGrounded() && velocity.y < 0) velocity.y = 0f;

        //Get the x and y values from player's input
        float x = Input.GetAxis("Horizontal");
        float y = Input.GetAxis("Vertical");

        //Apply the movement direction to the player character
        Vector3 moveDirection = transform.right * x + transform.forward * y;
        characterController.Move(moveDirection * walkingSpeed * Time.deltaTime);

        //Apply gravity to the player
        velocity.y += gravity * Time.deltaTime;
        characterController.Move(velocity * Time.deltaTime);
    }

    //Checks whether player is grounded
    private bool IsGrounded() {
        //Sends a downward raycast to see whether the player is grounded
        return (Physics.Raycast(transform.position, Vector3.down, jumpRaycastDistance));
    }

    //Lock the mouse to the center of the screen
    private void LockMouse() {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
}
