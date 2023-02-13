using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Guard : Player {
    [Header("Guard stats")]
    [SerializeField] private float currency = 10;

    protected override void Update() {
        base.Update();
    }

    //Set the guard's currency
    public void SetCurrency(int _value) {
        currency = _value;
    }

    //Make the guard lose currency
    public void LoseCurrency(int _value) {
        currency -= _value;
    }

    //Make the guard gain currency
    public void AddCurrency(int _value) {
        currency += _value;
    }
}
