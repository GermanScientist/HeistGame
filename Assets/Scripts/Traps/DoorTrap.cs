using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorTrap : Trap {
    public override void ActivateTrap(Room _room) {
        if (_room == null) return;
        _room.CloseDoors();
    }
}
