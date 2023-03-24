using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory {
    private List<Item> items;
    private System.Type selectedItem;

    public System.Type SelectedItem { get { return selectedItem; } }

    public void SelectItem(System.Type _type) {
        selectedItem = _type;
    }

    public void AddItem(Item _item) {
        if (_item == null) return;
        items.Add(_item);
    }

    public void RemoveItem(Item _item) {
        if (_item == null) return;
        items.Remove(_item);
    }

    public int CheckItemAmount(System.Type _type) {
        int amount = 0;
        foreach (Item _item in items) {
            if (_item.GetType() == _type) amount++;
        }

        return amount;
    }
}
