using UnityEngine;

public class UIButtonAddItems : MonoBehaviour
{
    public InventorySystem inventory;
    public ItemData itemToAdd;
    public int amount = 1;

    public void AddItem()
    {
        inventory.AddItem(itemToAdd, amount);
    }
    public void ClearItem()
    {
        inventory.ClearSlots();
    }

    public void SortItem()
    {
        inventory.SortInventory();
    }
}
