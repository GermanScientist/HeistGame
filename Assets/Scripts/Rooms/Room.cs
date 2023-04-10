using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Room : MonoBehaviour {
    private RoomMission roomMission;
    [SerializeField] private GameObject[] doors;
    [SerializeField] private GameObject centerPiece;
    [SerializeField] private bool hasEntrance;

    public bool HasEntrance { get { return hasEntrance; } }

    private void Awake() {
        roomMission = GetComponent<RoomMission>();

        ShowCenterPiece(false);

        if (roomMission != null) roomMission.InitializeRoomMission(this);
    }

    public void OnTriggerEnter(Collider _other) {
        Intruder intruder = _other.GetComponent<Intruder>();
        if (intruder == null || roomMission == null) return;

        roomMission.ActivateRoomMission(intruder);
    }

    public void OnTriggerStay(Collider _other) {
        Intruder intruder= _other.GetComponent<Intruder>();
        if (roomMission == null) EmptyRoomUpdate(_other);
        if (intruder == null || roomMission == null) return;
        if (_other.gameObject.tag == "Intruder") roomMission.UpdateRoomMission(intruder);
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
            if (animator == null) return;
            animator.ResetTrigger("CloseDoor");
            animator.SetTrigger("OpenDoor");
        }
    }

    [PunRPC]
    public void CloseDoors() {
        foreach (GameObject door in doors) {
            if (door == null) continue;

            Animator animator = door.transform.parent.GetComponent<Animator>();
            if (animator == null) return;
            animator.ResetTrigger("OpenDoor");
            animator.SetTrigger("CloseDoor");
        }
    }

    public void ShowCenterPiece(bool _state) {
        centerPiece.SetActive(_state);
    }

    private void EmptyRoomUpdate(Collider _collider) {
        if (_collider.gameObject.tag != "Guard") return;
        Guard guard = _collider.GetComponent<Guard>();
        
        if(Input.GetKeyDown(KeyCode.F)) {
            int doorTrapAmount = guard.PlayerInventory.CheckItemAmount(new RoomTrapItem().GetType()); ;
            if(doorTrapAmount >= 1) {
                guard.PlayerInventory.RemoveItem(new RoomTrapItem().GetType());
                guard.UpdateUI();
                gameObject.GetPhotonView().RPC("CreateDoorTrap", RpcTarget.All);
            }
        } 
        
        if (Input.GetKeyDown(KeyCode.G)) {
            guard.PlayerInventory.RemoveItem(new DamageTrapItem().GetType());
            guard.UpdateUI();
            gameObject.GetPhotonView().RPC("CreateDamageTrap", RpcTarget.All);
        }
    }

    [PunRPC]
    private void CreateDoorTrap() {
        roomMission = gameObject.AddComponent<DoorTrap>();
        roomMission.InitializeRoomMission(this);
    }

    [PunRPC]
    private void CreateDamageTrap() {
        roomMission = gameObject.AddComponent<DamageTrap>();
        roomMission.InitializeRoomMission(this);
    }
}
