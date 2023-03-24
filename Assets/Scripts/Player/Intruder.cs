using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Intruder : Player {
    protected override void Start() {
        base.Start();
        
        inventory.AddItem(new Weapon());
    }
}
