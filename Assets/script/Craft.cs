using UnityEngine;
using TMPro;

public class Craft : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private InventorySystem inventory;   
    [SerializeField] private TMP_InputField inputCount;   

    [Header("Recipe: Storage Chest = Lumber x (lumberPerChest * จำนวนที่คราฟ)")]
    [SerializeField] private ItemData lumber;             
    [SerializeField] private int lumberPerChest = 10;     
    [SerializeField] private ItemData storageChest;       
    [SerializeField] private int outputPerCraft = 1;      

    [Header("Craft Count (fallback)")]
    [SerializeField] private int craftCount = 1;          

    void Reset()
    {
        if (inventory == null) inventory = FindObjectOfType<InventorySystem>();
    }


    public void OnClickCraft()
    {
        int count = Mathf.Max(1, craftCount);
        TryCraft(count);
    }


    public void OnClickCraftByInput()
    {
        int count = ParseCountFromInput();
        TryCraft(count);
    }


    public void SetCount(int newCount)
    {
        craftCount = Mathf.Max(1, newCount);
    }


    private int ParseCountFromInput()
    {
        if (inputCount == null || string.IsNullOrWhiteSpace(inputCount.text))
            return Mathf.Max(1, craftCount);

        if (!int.TryParse(inputCount.text, out int parsed))
            return Mathf.Max(1, craftCount);

        return Mathf.Max(1, parsed);
    }

    private void TryCraft(int count)
    {
        // ตรวจอ้างอิงให้ครบ
        if (inventory == null || lumber == null || storageChest == null)
        {
            Debug.LogWarning("SimpleCrafter: อ้างอิงไม่ครบ (inventory/lumber/storageChest).");
            return;
        }

        // คำนวณจำนวนที่ต้องใช้
        int need = lumberPerChest * count;
        int have = inventory.GetTotalCount(lumber);

        if (have < need)
        {
            Debug.Log($"❌ วัตถุดิบไม่พอ ต้องใช้ {need} {lumber.itemName} แต่มี {have}.");
            return;
        }

        // หักวัตถุดิบ
        bool removed = inventory.RemoveItem(lumber, need);
        if (!removed)
        {
            Debug.LogWarning("RemoveItem ล้มเหลวแบบไม่คาดคิด (ช่องแตกต่างกัน/ข้อมูลคลังไม่สอดคล้อง).");
            return;
        }

        // ให้ของที่คราฟต์
        int give = outputPerCraft * count;
        bool added = inventory.AddItem(storageChest, give);
        if (!added)
        {
            // คลังเต็ม → rollback วัตถุดิบ
            inventory.AddItem(lumber, need);
            Debug.LogWarning("❌ คลังเต็ม ไม่สามารถรับของที่คราฟต์ → คืนวัตถุดิบแล้ว");
            return;
        }

        Debug.Log($"✅ คราฟต์สำเร็จ: {storageChest.itemName} x{give} (ใช้ {lumber.itemName} x{need})");
    }
}
