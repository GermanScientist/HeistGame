using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class Player : Actor {
    private Transform cameraTransform;
    private float xRotation = 0f;

    private Slider healthbar;
    private Slider staminabar;

    [Header("Player camera values")]
    [SerializeField] private float sensitivity = 100;
    [SerializeField] private float cameraClamp = 85f;

    protected override void Awake() {
        base.Awake();
        cameraTransform = Camera.main.gameObject.transform;
        healthbar = GameObject.Find("Healthbar").GetComponent<Slider>();
        staminabar = GameObject.Find("Staminabar").GetComponent<Slider>();
    }

    protected override void Start() {
        base.Start();
        LockMouse();
    }

    protected override void Update() {
        base.Update();
    }

    //Rotate the actor using the player's mouse input
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

    //Move the actor character using the player's input
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

    //Update the UI when the player's health or stamina gets updated
    public override void SetHealth(int _value) {
        base.SetHealth(_value);
        healthbar.value = (float)currentHealth / (float)maxHealth;
    }

    public override void TakeDamage(int _value) {
        base.TakeDamage(_value);
        healthbar.value = (float)currentHealth / (float)maxHealth;
    }

    public override void Heal(int _value) {
        base.Heal(_value);
        healthbar.value = (float)currentHealth / (float)maxHealth;
    }

    public override void SetStamina(int _value) {
        base.SetStamina(_value);
        staminabar.value = (float)currentStamina / (float)maxStamina;
    }

    public override void LowerStamina(int _value) {
        base.LowerStamina(_value);
        staminabar.value = (float)currentStamina / (float)maxStamina;
    }

    public override void IncreaseStamina(int _value) {
        base.IncreaseStamina(_value);
        staminabar.value = (float)currentStamina / (float)maxStamina;
    }
}
