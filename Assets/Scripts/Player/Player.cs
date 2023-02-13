using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Player : Actor {
    private Transform cameraTransform;
    private float xRotation = 0f;

    [Header("Player camera values")]
    [SerializeField] private float sensitivity = 100;
    [SerializeField] private float cameraClamp = 85f;

    //Awake gets called once before Start
    protected override void Awake() {
        base.Awake();
        cameraTransform = Camera.main.gameObject.transform;
    }

    //Start gets called once before Update
    protected override void Start() {
        base.Start();
        LockMouse();
    }

    //Update gets called every frame
    protected override void Update() {
        base.Update();
    }

    //Rotate the player using the player's mouse input
    protected override void RotateActor() {
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
    protected override void MoveActor() {
        //Get the x and y values from player's input
        float x = Input.GetAxis("Horizontal");
        float y = Input.GetAxis("Vertical");

        //Apply the movement direction to the player character
        Vector3 moveDirection = transform.right * x + transform.forward * y;
        characterController.Move(moveDirection * walkingSpeed * Time.deltaTime);
    }

    //Lock the mouse to the center of the screen
    private void LockMouse() {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
}
