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

        roomMission.InitializeRoomMission(this);
    }

    public void OnTriggerEnter(Collider _other) {
        Player player = _other.GetComponent<Player>();
        if (player == null) return;

        roomMission.ActivateRoomMission(player);
    }

    public void OnTriggerStay(Collider _other) {
        Player player = _other.GetComponent<Player>();
        if (player == null) return;

        roomMission.UpdateRoomMission(player);
    }

    public void OpenDoors() {
        SetRoomDoorsActive(false);
    }

    public void CloseDoors() {
        SetRoomDoorsActive(true);
    }

    public void ShowCenterPiece(bool _state) {
        centerPiece.SetActive(_state);
    }

    private void SetRoomDoorsActive(bool _state) {
        foreach (GameObject door in doors) {
            if (door != null) door.SetActive(_state);
        }
    }
}
