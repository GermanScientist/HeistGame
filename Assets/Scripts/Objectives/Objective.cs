using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Objective : RoomMission {
    public override void InitializeRoomMission(Room _room) {
        base.InitializeRoomMission(_room);
        currentRoom.ShowCenterPiece(true);
    }
}
