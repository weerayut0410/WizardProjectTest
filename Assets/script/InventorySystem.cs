using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class InventorySystem : MonoBehaviour
{
    public int maxSlots = 10;
    public int maxStack = 10;

    public List<InventorySlot> slots;

    public List<TextMeshProUGUI> slotTexts;

    void Awake()
    {
        ClearSlots();
        slots = new List<InventorySlot>(maxSlots);

        // �������ͧ��ҧ
        for (int i = 0; i < maxSlots; i++)
        {
            slots.Add(null); // null = ��ͧ��ҧ
        }
    }

    public bool AddItem(ItemData item, int amount)
    {
        int remaining = amount;


        // 1. �����ͧ������������ǡѹ����ѧ������
        for (int i = 0; i < slots.Count; i++)
        {
            InventorySlot slot = slots[i];

            if (slot != null && slot.item == item && slot.amount < item.Stack)
            {
                int space = item.Stack - slot.amount;
                int addAmount = Mathf.Min(space, remaining);

                slot.amount += addAmount;
                remaining -= addAmount;

                if (remaining <= 0)
                {
                    UpdateUI();
                    Debug.Log($"Picked up {amount}x {item.itemName}");
                    return true; // �����ú����
                }
            }
        }

        // 2. ���㹪�ͧ��ҧ���� (�����)
        for (int i = 0; i < slots.Count; i++)
        {
            if (slots[i] == null)
            {
                int addAmount = Mathf.Min(item.Stack, remaining);
                slots[i] = new InventorySlot(item, addAmount);
                remaining -= addAmount;

                if (remaining <= 0)
                {
                    UpdateUI();
                    Debug.Log($"Picked up {amount}x {item.itemName}");
                    return true; // �����ú����
                }
            }
        }

        // �Ҷ֧��� ����������ú
        int added = amount - remaining;
        UpdateUI();
        Debug.Log($"Picked up only {added}x {item.itemName}. Inventory is full.");
        return false;
    }

    public bool RemoveItem(ItemData item, int amount)
    {
        int remaining = amount;

        // ǹ��ҹ��ͧ�����������Ҫ�ͧ������������ǡѹ
        for (int i = 0; i < slots.Count; i++)
        {
            InventorySlot slot = slots[i];

            if (slot != null && slot.item == item)
            {
                if (slot.amount <= remaining)
                {
                    // ����͡������ �����������ͧ
                    remaining -= slot.amount;
                    slots[i] = null;
                }
                else
                {
                    // ����͡�ҧ��ǹ
                    slot.amount -= remaining;
                    remaining = 0;
                }

                // ź�ú����
                if (remaining <= 0)
                {
                    UpdateUI();
                    Debug.Log($"Removed {amount}x {item.itemName}");
                    return true;
                }
            }
        }

        // ����Ҷ֧�ç��������ź�����ú
        int removed = amount - remaining;
        UpdateUI();
        Debug.LogWarning($"Only removed {removed}x {item.itemName}. Not enough items in inventory.");
        return false;
    }

    public void SortInventory()
    {
        // ����������������ԧ�ҨѴ���§ (������ͧ��ҧ)
        List<InventorySlot> filledSlots = new List<InventorySlot>();
        foreach (var slot in slots)
        {
            if (slot != null && slot.item != null)
            {
                filledSlots.Add(slot);
            }
        }

        // �Ѵ���§������������� (type) ���ǵ������ (itemName)
        filledSlots.Sort((a, b) =>
        {
            int typeCompare = a.item.itemType.CompareTo(b.item.itemType);
            if (typeCompare == 0)
            {
                return a.item.itemName.CompareTo(b.item.itemName);
            }
            return typeCompare;
        });

        // ��ҧ slot ���
        for (int i = 0; i < slots.Count; i++)
        {
            slots[i] = null;
        }

        // ��� slot �������ӴѺ
        for (int i = 0; i < filledSlots.Count; i++)
        {
            slots[i] = filledSlots[i];
        }

        UpdateUI();
        Debug.Log("Inventory sorted.");
    }

    public void ClearSlots()
    {
        for (int i = 0; i < slots.Count; i++)
        {
            slots[i] = null;
        }
        UpdateUI();
        Debug.Log("Inventory cleared.");
    }

    public void UpdateUI()
    {
        for (int i = 0; i < maxSlots; i++)
        {
            if (slotTexts.Count > i && slotTexts[i] != null)
            {
                if (slots.Count > i && slots[i] != null && slots[i].item != null)
                {
                    slotTexts[i].text = $"{slots[i].item.itemName} x{slots[i].amount}";
                }
                else
                {
                    slotTexts[i].text = "";  // ��ҧ ���������������ͪ�ͧ��ҧ
                }
            }

        }
    }

}
