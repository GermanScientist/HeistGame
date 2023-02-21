using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Guard : Player {
    [Header("Guard stats")]
    [SerializeField] private float currency = 10;

    protected override void Update() {
        base.Update();
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
}
