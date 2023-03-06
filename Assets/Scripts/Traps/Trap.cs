using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Trap : MonoBehaviour {
    protected GameManager gameManager;

    public virtual void Awake() {
        gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();
    }

    public virtual void ActivateTrap(Room _room, Player _player) {
        _room.ShowCenterPiece(true);
    }

    public virtual void ActivateTrap(Room _room) {
        _room.ShowCenterPiece(true);
    }
}
