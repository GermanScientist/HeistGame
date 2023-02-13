using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public abstract class Actor : MonoBehaviour {
    protected CharacterController characterController;
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
    public virtual void SetHealth(int _value) {
        currentHealth = _value;
    }

    //Make the actor take damage
    public virtual void TakeDamage(int _value) {
        currentHealth -= _value;
    }

    //Make the actor gain hp
    public virtual void Heal(int _value) {
        currentHealth += _value;
    }

    //Set the actor's stamina
    public virtual void SetStamina(int _value) {
        currentStamina = _value;
    }

    //Lower the actor's stamina
    public virtual void LowerStamina(int _value) {
        currentStamina -= _value;
    }

    //Increase the actor's stamina
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
