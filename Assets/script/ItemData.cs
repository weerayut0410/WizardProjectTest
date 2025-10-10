using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "Inventory/Item")]
public class ItemData : ScriptableObject
{
    public string itemName;  // �����������
    public int Stack = 0;

    public ItemType itemType; // �������ͧ����

    // ���� enum ����Ѻ�������ͧ����
    public enum ItemType
    {
        Resources,     // ��Ѿ�ҡ� �� ��� �Թ
        Tools,         // ����ͧ��� �� ��ҹ �����
        CraftedObject, // �ͧ��ҿ�� �� ��� ������
        Seeds          // ���紾ѹ���
    }
}
