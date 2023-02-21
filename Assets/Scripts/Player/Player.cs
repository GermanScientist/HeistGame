using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using UnityEngine.SceneManagement;

public abstract class Player : Actor {
    private Transform cameraTransform;
    private Transform cameraContainerTransform;
    private float xRotation = 0f;

    private Slider healthbar;
    private Slider staminabar;

    [Header("Player camera values")]
    [SerializeField] private float sensitivity = 100;
    [SerializeField] private float cameraClamp = 85f;

    protected override void Awake() {
        base.Awake();
        if(photonView.IsMine) cameraTransform = GameObject.Find("Main Camera").transform;
        if(photonView.IsMine) cameraContainerTransform = transform.Find("MainCameraPosition");
        Debug.Log(cameraTransform);

        healthbar = GameObject.Find("Healthbar").GetComponent<Slider>();
        staminabar = GameObject.Find("Staminabar").GetComponent<Slider>();
    }

    protected override void Start() {
        base.Start();
        LockMouse();
    }

    protected override void Update() {
        base.Update();

        if (Input.GetKeyDown(KeyCode.K)) TakeDamage(10);
        if (Input.GetMouseButtonDown(0)) HitPlayer();
    }

    //Rotate the actor using the player's mouse input
    protected override void RotateActor() {
        if (!photonView.IsMine) return;
        if (cameraTransform == null && cameraContainerTransform) return;

        //Set the mouse values based on the mouse input multiplied by the mouse sensitivity
        float mouseX = Input.GetAxis("Mouse X") * sensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * sensitivity * Time.deltaTime;

        //Set the rotation
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -cameraClamp, cameraClamp);

        //Apply the rotation
        cameraContainerTransform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        transform.Rotate(Vector3.up * mouseX);

        cameraTransform.position = cameraContainerTransform.position;
        cameraTransform.rotation = cameraContainerTransform.rotation;
    }

    //Move the actor character using the player's input
    protected override void MoveActor() {
        if (!photonView.IsMine) return;

        //Get the x and y values from player's input
        float x = Input.GetAxis("Horizontal");
        float y = Input.GetAxis("Vertical");

        //Apply the movement direction to the player character
        Vector3 moveDirection = transform.right * x + transform.forward * y;
        characterController.Move(moveDirection * walkingSpeed * Time.deltaTime);
    }

    //Lock the mouse to the center of the screen
    private void LockMouse() {
        if (!photonView.IsMine) return;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    //Update the UI when the player's health or stamina gets updated
    [PunRPC]
    public override void SetHealth(int _value) {
        if (!photonView.IsMine) return;

        base.SetHealth(_value);
        healthbar.value = (float)currentHealth / (float)maxHealth;
    }

    [PunRPC]
    public override void TakeDamage(int _value) {
        if (!photonView.IsMine) return;

        base.TakeDamage(_value);
        healthbar.value = (float)currentHealth / (float)maxHealth;
    }

    [PunRPC]
    public override void Heal(int _value) {
        if (!photonView.IsMine) return;

        base.Heal(_value);
        healthbar.value = (float)currentHealth / (float)maxHealth;
    }

    [PunRPC]
    public override void SetStamina(int _value) {
        if (!photonView.IsMine) return;

        base.SetStamina(_value);
        staminabar.value = (float)currentStamina / (float)maxStamina;
    }

    [PunRPC]
    public override void LowerStamina(int _value) {
        if (!photonView.IsMine) return;

        base.LowerStamina(_value);
        staminabar.value = (float)currentStamina / (float)maxStamina;
    }

    [PunRPC]
    public override void IncreaseStamina(int _value) {
        if (!photonView.IsMine) return;

        base.IncreaseStamina(_value);
        staminabar.value = (float)currentStamina / (float)maxStamina;
    }

    protected override void Die() {
        if (!photonView.IsMine) return;
        Debug.Log("died");
        PhotonNetwork.LeaveRoom();
        SceneManager.LoadScene("LobbyNavigator");
    }

    [PunRPC]
    protected virtual void HitPlayer() {
        if (!photonView.IsMine) return;

        Debug.DrawRay(cameraContainerTransform.position, cameraContainerTransform.forward);

        RaycastHit hit;
        if (Physics.Raycast(cameraContainerTransform.position, cameraContainerTransform.forward, out hit, 100)) {
            GameObject target = hit.transform.gameObject;
            
            if (target.tag == "Player") {
                if(target.GetPhotonView() != null) target.GetPhotonView().RPC("TakeDamage", RpcTarget.All, 10);
            }
            Debug.Log(hit.transform.name);
        }
    }
}
