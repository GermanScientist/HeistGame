using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorTrap : Trap {
    public override void ActivateRoomMission(Intruder _intruder) {
        base.ActivateRoomMission(_intruder);
        currentRoom.SendCloseDoorRequest();
    }
}
