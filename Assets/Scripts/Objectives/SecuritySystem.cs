using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SecuritySystem : Objective {
    [SerializeField] private float timeRequired = 50;
    [SerializeField] private float timeRemaining;

    public override void InitializeRoomMission(Room _room) {
        base.InitializeRoomMission(_room);
        currentRoom.ShowCenterPiece(true);

        timeRemaining = timeRequired;
    }

    public override void UpdateRoomMission(Player _player) {
        base.UpdateRoomMission(_player);

        if(Input.GetKeyDown(KeyCode.E)) {
            started = true;
        }

        if (started) {
            timeRemaining -= 1 * Time.deltaTime;
        }

        if(timeRemaining <= 0) {
            completed = true;
        }
    }
}
