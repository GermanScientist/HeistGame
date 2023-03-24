using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Store : MonoBehaviour {
    private Guard guard;
    private Inventory inventory;

    public void Start() {
        guard = GameObject.Find("Guard").GetComponent<Guard>();
        inventory = guard.PlayerInventory;
    }

    public void BuyRoomTrap() {
        inventory.AddItem(new RoomTrapItem());
        guard.LoseCurrency(10);
    }

    public void BuyDamageTrap() {
        inventory.AddItem(new DamageTrapItem());
        guard.LoseCurrency(5);
    }
}
