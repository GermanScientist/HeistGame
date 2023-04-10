using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class SecuritySystem : Objective {

    public override void InitializeRoomMission(Room _room) {
        base.InitializeRoomMission(_room);
        currentRoom.ShowCenterPiece(true);
    }

    public override void UpdateRoomMission(Intruder _intruder) {
        base.UpdateRoomMission(_intruder);

        if(Input.GetKeyDown(KeyCode.E) && !completed) {
            completed = true;
            gameManager.photonView.RPC("CompleteObjective", RpcTarget.All);
        }
    }
}
