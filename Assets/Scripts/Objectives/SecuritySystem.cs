using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SecuritySystem : Objective {
    public override void InitializeRoomMission(Room _room) {
        base.InitializeRoomMission(_room);
        currentRoom.ShowCenterPiece(true);
    }
}
