using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Room : MonoBehaviour {
    private RoomMission roomMission;
    [SerializeField] private GameObject[] doors;
    [SerializeField] private GameObject centerPiece;

    private void Awake() {
        roomMission = GetComponent<RoomMission>();

        ShowCenterPiece(false);
        SendOpenDoorRequest();

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

    public void SendOpenDoorRequest() {
        if (gameObject.GetPhotonView() == null) return; 
        gameObject.GetPhotonView().RPC("OpenDoors", RpcTarget.All);
    }

    public void SendCloseDoorRequest() {
        if (gameObject.GetPhotonView() == null) return;
        gameObject.GetPhotonView().RPC("CloseDoors", RpcTarget.All);
    }

    [PunRPC]
    public void OpenDoors() {
        foreach (GameObject door in doors) {
            if (door == null) continue;

            Animator animator = door.transform.parent.GetComponent<Animator>();
            animator.ResetTrigger("CloseDoor");
            animator.SetTrigger("OpenDoor");
        }
    }

    [PunRPC]
    public void CloseDoors() {
        foreach (GameObject door in doors) {
            if (door == null) continue;

            Animator animator = door.transform.parent.GetComponent<Animator>();
            animator.ResetTrigger("OpenDoor");
            animator.SetTrigger("CloseDoor");
        }
    }

    public void ShowCenterPiece(bool _state) {
        centerPiece.SetActive(_state);
    }

    private void EmptyRoomUpdate() {
        if(Input.GetKeyDown(KeyCode.F)) {
            roomMission = gameObject.AddComponent<DoorTrap>();
            roomMission.InitializeRoomMission(this);
        } 
        
        if (Input.GetKeyDown(KeyCode.G)) {
            roomMission = gameObject.AddComponent<DamageTrap>();
            roomMission.InitializeRoomMission(this);
        }
    }
}
