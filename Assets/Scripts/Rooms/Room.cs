using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Realtime;

public class Room : MonoBehaviour {
    private enum RoomType { Empty, DamageTrap, DoorTrap, SecuritySystem, Vault }
    [SerializeField] private RoomType roomType = RoomType.Empty;
    [SerializeField] private GameObject[] doors;

    private void Awake() {
        if (roomType == RoomType.DamageTrap) gameObject.AddComponent<DamageTrap>();
        if (roomType == RoomType.DoorTrap) gameObject.AddComponent<DoorTrap>();

        OpenDoors();
    }

    public void OnTriggerEnter(Collider _other) {
        Player player = _other.GetComponent<Player>();
        
        if (player == null) return;
        if (roomType == RoomType.DamageTrap) GetComponent<DamageTrap>().ActivateTrap(player);
        if (roomType == RoomType.DoorTrap) GetComponent<DoorTrap>().ActivateTrap(this);
    }

    public void OpenDoors() {
        SetRoomDoorsActive(false);
    }

    public void CloseDoors() {
        SetRoomDoorsActive(true);
    }

    private void SetRoomDoorsActive(bool _state) {
        foreach (GameObject door in doors) {
            if (door != null) door.SetActive(_state);
        }
    }
}
