using UnityEngine;

[System.Serializable]
public class InventoryItem
{
    public ItemData data;
    public int quantity;

    public InventoryItem(ItemData data, int qty)
    {
        this.data = data;
        this.quantity = qty;
    }
}
