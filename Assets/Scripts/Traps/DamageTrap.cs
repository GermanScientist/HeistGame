using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageTrap : Trap {
    public override void ActivateRoomMission(Intruder _intruder) {
        base.ActivateRoomMission(_intruder);
        if (gameManager == null || _intruder == null) return;
        if (gameManager.currentPlayer == _intruder) _intruder.TakeDamage(10);
    }
}
