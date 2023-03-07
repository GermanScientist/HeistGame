using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmptyRoomMission : RoomMission {
    public override void InitializeRoomMission(Room _room) {
        base.InitializeRoomMission(_room);
        currentRoom.ShowCenterPiece(false);
    }

    public override void UpdateRoomMission(Player _player) {
        base.UpdateRoomMission(_player);

        if(Input.GetKeyDown(KeyCode.E)) {
            DoorTrap doorTrap = gameObject.AddComponent<DoorTrap>();
            doorTrap.InitializeRoomMission(currentRoom);
            Destroy(this);
        }
    }
}
