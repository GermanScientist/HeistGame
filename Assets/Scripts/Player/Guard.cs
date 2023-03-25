using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Guard : Player {
    [Header("Guard stats")]
    [SerializeField] private float currency = 10;

    private GameObject storeCanavs;

    [SerializeField] private bool shouldOpenStore;

    public float Currency { get { return currency; } }
    public GameObject StoreCanvas { get { return storeCanavs; } }

    protected override void Awake() {
        base.Awake();

        shouldOpenStore = true;
        storeCanavs = GameObject.Find("StoreCanvas");
    }

    protected override void Start() {
        base.Start();

        Debug.Log(inventory);

        inventory.AddItem(new Weapon());
        inventory.AddItem(new RoomTrapItem());
        inventory.AddItem(new RoomTrapItem());
        inventory.AddItem(new DamageTrapItem());
    }

    protected override void Update() {
        base.Update();

        if(Input.GetKeyDown(KeyCode.Q)) {
            if (shouldOpenStore) OpenStore();
            if (!shouldOpenStore) CloseStore();
            shouldOpenStore = !shouldOpenStore;
        }
            
    }

    //Set the guard's currency
    [PunRPC]
    public void SetCurrency(int _value) {
        if (!photonView.IsMine) return;
        currency = _value;
    }

    //Make the guard lose currency
    [PunRPC]
    public void LoseCurrency(int _value) {
        if (!photonView.IsMine) return;
        currency -= _value;
    }

    //Make the guard gain currency
    [PunRPC]
    public void AddCurrency(int _value) {
        if (!photonView.IsMine) return;
        currency += _value;
    }

    public void OpenStore() {
        storeCanavs.SetActive(true);
        UnlockMouse();
        sensitivity = 0;
        canMove = false;
    }

    public void CloseStore() {
        storeCanavs.SetActive(false);
        LockMouse();
        sensitivity = 100;
        canMove = true;
    }
}
