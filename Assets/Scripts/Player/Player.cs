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

    private GameManager gameManager;
    protected bool canMove;
    [SerializeField] protected Inventory inventory;

    [Header("Player camera values")]
    [SerializeField] protected float sensitivity = 100;
    [SerializeField] private float cameraClamp = 85f;

    public Inventory PlayerInventory { get { return inventory; } }

    protected override void Awake() {
        base.Awake();
        if(photonView.IsMine) cameraTransform = GameObject.Find("Main Camera").transform;
        if(photonView.IsMine) cameraContainerTransform = transform.Find("MainCameraPosition");

        gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();
        healthbar = GameObject.Find("Healthbar").GetComponent<Slider>();
        staminabar = GameObject.Find("Staminabar").GetComponent<Slider>();

        inventory = new Inventory();

        canMove = true;

        if (photonView.IsMine) gameManager.currentPlayer = this;
    }

    protected override void Start() {
        base.Start();
        LockMouse();
    }

    protected override void Update() {
        base.Update();

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
        if(!canMove) return;
        Vector3 moveDirection = transform.right * x + transform.forward * y;
        characterController.Move(moveDirection * walkingSpeed * Time.deltaTime);
    }

    //Lock the mouse to the center of the screen
    protected void LockMouse() {
        if (!photonView.IsMine) return;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    protected void UnlockMouse() {
        if (!photonView.IsMine) return;

        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;
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
        UnlockMouse();

        GameObject.Find("Game Manager").GetComponent<GameManager>().LeaveRoom();
    }

    [PunRPC]
    protected virtual void HitPlayer() {
        if (!photonView.IsMine) return;

        RaycastHit hit;
        if (Physics.Raycast(cameraContainerTransform.position, cameraContainerTransform.forward, out hit, 100)) {
            GameObject target = hit.transform.gameObject;
            
            if (target.tag == "Intruder" || target.tag == "Guard") {
                if(target.GetPhotonView() != null) target.GetPhotonView().RPC("TakeDamage", RpcTarget.All, 10);
            }
        }
    }

    [PunRPC]
    private void OpenDoorRequest(Animator _animator) {
        _animator.ResetTrigger("CloseDoor");
        _animator.SetTrigger("OpenDoor");
    }

    private void OpenDoor() {
        GameObject door = GetRaycastGO().tag == "Door" ? GetRaycastGO() : null;
        if (door == null) return;

        Animator animator = door.transform.parent.GetComponent<Animator>();
        if (animator == null) return;

        animator.ResetTrigger("CloseDoor");
        animator.SetTrigger("OpenDoor");

        //if (gameObject.GetPhotonView() != null) 
        //    gameObject.GetPhotonView().RPC("OpenDoorRequest", RpcTarget.All, animator);
    }

    protected GameObject GetRaycastGO() {
        RaycastHit hit;
        if (Physics.Raycast(cameraContainerTransform.position, cameraContainerTransform.forward, out hit, 100))
            return hit.transform.gameObject;
        return null;
    }
}
