using UnityEngine;

[CreateAssetMenu(fileName = "ItemData", menuName = "Scriptable Objects/ItemData")]
public class ItemDataSO : ScriptableObject {
    public Sprite sprite;
    public string id;
    public string itemName;
    public string description;
}
