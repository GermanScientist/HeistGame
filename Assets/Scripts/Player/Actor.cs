using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

[RequireComponent(typeof(CharacterController))]
public abstract class Actor : MonoBehaviour {
    protected CharacterController characterController;
    protected PhotonView photonView;
    protected Vector3 velocity;
    protected float currentHealth;
    protected float currentStamina;

    [Header("Actor stats")]
    [SerializeField] protected float maxHealth = 100;
    [SerializeField] protected float maxStamina = 100;

    [Header("Actor movement values")]
    [SerializeField] protected float gravity = -9.8f;
    [SerializeField] protected float groundDistance = 1.1f;
    [SerializeField] protected float walkingSpeed = 15f;


    protected virtual void Awake() {
        characterController = GetComponent<CharacterController>();
        photonView = GetComponent<PhotonView>();
    }

    protected virtual void Start() {
        currentHealth = maxHealth;
    }
    
    protected virtual void Update() {
        ApplyGravity(); //Apply gravity to the actor
        MoveActor(); //Move the actor, implementation may differ per actor
        RotateActor(); //Rotate the actor, implementation may differ per actor
    }

    //Set the actor's health
    [PunRPC]
    public virtual void SetHealth(int _value) {
        currentHealth = _value;
        if (currentHealth <= 0) Die();
    }

    //Make the actor take damage
    [PunRPC]
    public virtual void TakeDamage(int _value) {
        currentHealth -= _value;
        if (currentHealth <= 0) Die();
    }

    //Make the actor gain hp
    [PunRPC]
    public virtual void Heal(int _value) {
        currentHealth += _value;
        if (currentHealth <= 0) Die();
    }

    //Set the actor's stamina
    [PunRPC]
    public virtual void SetStamina(int _value) {
        currentStamina = _value;
    }

    //Lower the actor's stamina
    [PunRPC]
    public virtual void LowerStamina(int _value) {
        currentStamina -= _value;
    }

    //Increase the actor's stamina
    [PunRPC]
    public virtual void IncreaseStamina(int _value) {
        currentStamina += _value;
    }

    protected virtual void Die() { } //Kill the actor
    protected virtual void MoveActor() { } //Move the actor
    protected virtual void RotateActor() { } //Rotate the actor

    //Checks whether actor is grounded
    protected bool IsGrounded() {
        //Sends a downward raycast to see whether the actor is grounded
        return (Physics.Raycast(transform.position, Vector3.down, groundDistance));
    }

    //Apply gravity to the actor
    private void ApplyGravity() {
        //Reset the velocity when the actor is grounded
        if (IsGrounded() && velocity.y < 0) velocity.y = 0f;

        //Apply gravity to the actor
        velocity.y += gravity * Time.deltaTime;
        characterController.Move(velocity * Time.deltaTime);
    }
}
