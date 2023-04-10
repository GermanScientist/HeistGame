using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class RoomMission : MonoBehaviour {
    protected GameManager gameManager;
    protected Room currentRoom;

    protected virtual void Awake() {
        gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();
    }

    public virtual void InitializeRoomMission(Room _room) {
        currentRoom = _room;
    }

    public virtual void ActivateRoomMission(Intruder intruder) {

    }

    public virtual void UpdateRoomMission(Intruder _intruder) {

    }

    public virtual void EndRoomMission(Intruder intruder) {

    }
}
