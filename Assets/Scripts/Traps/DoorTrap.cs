using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorTrap : Trap {
    public override void ActivateRoomMission(Player _player) {
        base.ActivateRoomMission(_player);
        currentRoom.SendCloseDoorRequest();
    }
}
