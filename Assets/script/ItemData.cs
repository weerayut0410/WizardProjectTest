using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "Inventory/Item")]
public class ItemData : ScriptableObject
{
    public string itemName;  // เก็บแค่ชื่อไอเทม
    public int Stack = 0;

    public ItemType itemType; // ประเภทของไอเทม

    // เพิ่ม enum สำหรับประเภทของไอเทม
    public enum ItemType
    {
        Resources,     // ทรัพยากร เช่น ไม้ หิน
        Tools,         // เครื่องมือ เช่น ขวาน พลั่ว
        CraftedObject, // ของคราฟต์ เช่น โต๊ะ เก้าอี้
        Seeds          // เมล็ดพันธุ์
    }
}
