using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;

public class Guard : Player {
    [Header("Guard stats")]
    [SerializeField] private float currency = 10;

    private GameObject storeCanvas;
    private TMP_Text currencyText;
    private TMP_Text damageText;
    private TMP_Text doorText;

    [SerializeField] private bool shouldOpenStore;

    public float Currency { get { return currency; } }
    public GameObject StoreCanvas { get { return storeCanvas; } }

    protected override void Awake() {
        base.Awake();

        shouldOpenStore = true;
        storeCanvas = GameObject.Find("StoreCanvas");

        if (photonView.IsMine) {
            Destroy(GameObject.Find("IntruderCanvas"));
        }

        currencyText = GameObject.Find("CurrencyText").GetComponent<TMP_Text>();
        damageText = GameObject.Find("DamageText").GetComponent<TMP_Text>();
        doorText = GameObject.Find("DoorText").GetComponent<TMP_Text>();
    }

    protected override void Start() {
        base.Start();

        inventory.AddItem(new Weapon());
        inventory.AddItem(new RoomTrapItem());
        inventory.AddItem(new RoomTrapItem());
        inventory.AddItem(new DamageTrapItem());

        UpdateUI();
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

    public void UpdateUI() {
        if (!photonView.IsMine) return;

        int roomTrapAmount = inventory.CheckItemAmount(new RoomTrapItem().GetType());
        int damageTrapAmount = inventory.CheckItemAmount(new DamageTrapItem().GetType());

        currencyText.text = "$" + currency.ToString();
        doorText.text = roomTrapAmount.ToString();
        damageText.text = damageTrapAmount.ToString();

    }

    public void OpenStore() {
        if (!photonView.IsMine) return;

        storeCanvas.SetActive(true);
        UnlockMouse();
        sensitivity = 0;
        canMove = false;
    }

    public void CloseStore() {
        if (!photonView.IsMine) return;

        storeCanvas.SetActive(false);
        LockMouse();
        sensitivity = 100;
        canMove = true;
    }
}
