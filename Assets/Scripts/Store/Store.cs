using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Store : MonoBehaviour {
    private Guard guard;
    private Inventory inventory;

    private void Start() {
        guard = GameObject.FindGameObjectWithTag("Guard").GetComponent<Guard>();
        guard.StoreCanvas.SetActive(false);
        inventory = guard.PlayerInventory;
    }

    public void BuyRoomTrap() {
        if(guard.Currency < 10) return; 
        inventory.AddItem(new RoomTrapItem());
        guard.LoseCurrency(10);
    }

    public void BuyDamageTrap() {
        if (guard.Currency < 5) return;

        inventory.AddItem(new DamageTrapItem());
        guard.LoseCurrency(5);
    }
}
