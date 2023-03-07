using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageTrap : Trap {
    public override void ActivateRoomMission(Player _player) {
        base.ActivateRoomMission(_player);
        if (gameManager == null || _player == null) return;
        if (gameManager.currentPlayer == _player) _player.TakeDamage(10);
    }
}
