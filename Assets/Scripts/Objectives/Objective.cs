using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Objective : MonoBehaviour {
    protected Room currentRoom;

    public virtual void InitializeObjective(Room _room) {
        currentRoom = _room;
        currentRoom.ShowCenterPiece(true);
    }

    public virtual void UpdateObjective(Player _player) {
        Debug.Log("Updating");
    }
}
