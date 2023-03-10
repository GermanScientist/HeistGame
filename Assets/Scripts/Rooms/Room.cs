using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Realtime;

public class Room : MonoBehaviour {
    private RoomMission roomMission;
    [SerializeField] private GameObject[] doors;
    [SerializeField] private GameObject centerPiece;

    private void Awake() {
        roomMission = GetComponent<RoomMission>();

        ShowCenterPiece(false);
        OpenDoors();

        if (roomMission != null) roomMission.InitializeRoomMission(this);
    }

    public void OnTriggerEnter(Collider _other) {
        Player player = _other.GetComponent<Player>();
        if (player == null || roomMission == null) return;

        roomMission.ActivateRoomMission(player);
    }

    public void OnTriggerStay(Collider _other) {
        Player player = _other.GetComponent<Player>();
        if (roomMission == null) EmptyRoomUpdate();
        if (player == null || roomMission == null) return;

        roomMission.UpdateRoomMission(player);
    }

    public void OpenDoors() {
        foreach (GameObject door in doors) {
            if (door == null) continue;

            Animator animator = door.transform.parent.GetComponent<Animator>();
            animator.ResetTrigger("CloseDoor");
            animator.SetTrigger("OpenDoor");
        }
    }

    public void CloseDoors() {
        foreach (GameObject door in doors) {
            if (door == null) continue;

            Animator animator = door.transform.parent.GetComponent<Animator>();
            animator.ResetTrigger("CloseDoor");
            animator.SetTrigger("OpenDoor");
        }
    }

    public void ShowCenterPiece(bool _state) {
        centerPiece.SetActive(_state);
    }

    private void EmptyRoomUpdate() {
        if(Input.GetKeyDown(KeyCode.E)) {
            roomMission = gameObject.AddComponent<DoorTrap>();
            roomMission.InitializeRoomMission(this);
        } 
        
        if (Input.GetKeyDown(KeyCode.F)) {
            roomMission = gameObject.AddComponent<DamageTrap>();
            roomMission.InitializeRoomMission(this);
        }
    }
}
