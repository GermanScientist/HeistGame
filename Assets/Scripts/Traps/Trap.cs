using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Trap : MonoBehaviour {
    protected GameManager gameManager;

    public virtual void Awake() {
        gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();
    }

    public virtual void ActivateTrap(Player _player) {
    }

    public virtual void ActivateTrap(Room _room) {
    }
}
