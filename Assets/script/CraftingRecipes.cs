using UnityEngine;

[System.Serializable]
public struct Ingredient
{
    public ItemData item;
    public int amount; // ⇦ ใส่จำนวนที่ต้องใช้ต่อไอเท็ม
}

[CreateAssetMenu(fileName = "Recipe", menuName = "Crafting/Recipe")]
public class CraftingRecipes : ScriptableObject
{
    public ItemData output;
    public int outputAmount = 1;
    public Ingredient[] ingredients; // ใส่สูตรหลายชนิดได้
}
