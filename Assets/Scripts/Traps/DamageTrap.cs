using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageTrap : Trap {
    public override void ActivateTrap(Player _player) {
        if (gameManager == null || _player == null) return;
        if (gameManager.currentPlayer == _player) _player.TakeDamage(10);
    }
}
