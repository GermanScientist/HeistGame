using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Store : MonoBehaviour {
    private Guard guard;
    private Inventory inventory;

    private void Start() {
        guard = GameObject.FindGameObjectWithTag("Guard").GetComponent<Guard>();
        if (guard == null) return;
        if (!guard.gameObject.GetPhotonView().IsMine) return;
        
        guard.StoreCanvas.SetActive(false);
        inventory = guard.PlayerInventory;
    }

    public void BuyRoomTrap() {
        if(guard.Currency < 10) return; 
        inventory.AddItem(new RoomTrapItem());
        guard.LoseCurrency(10);
        guard.UpdateUI();
    }

    public void BuyDamageTrap() {
        if (guard.Currency < 5) return;

        inventory.AddItem(new DamageTrapItem());
        guard.LoseCurrency(5);
        guard.UpdateUI();
    }
}
