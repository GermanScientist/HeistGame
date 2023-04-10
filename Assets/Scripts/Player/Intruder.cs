using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Intruder : Player {
    private GameObject intruderCanvas;
    protected override void Start() {
        base.Start();
        intruderCanvas = GameObject.Find("IntruderCanvas");
        inventory.AddItem(new Weapon());

        if (photonView.IsMine) {
            Destroy(GameObject.Find("GuardCanvas"));
            Destroy(GameObject.Find("StoreCanvas"));
        }
    }
}
