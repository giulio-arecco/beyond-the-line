using UnityEngine;

public class UIInventorySlot : MonoBehaviour {
    [SerializeField] private Vector2 itemSpriteLocalScale = new(0.9f, 0.9f);
    
    public Vector2 ItemSpriteLocalScale { get => itemSpriteLocalScale; private set => itemSpriteLocalScale = value; }
    public UIInventoryItem ChildItem { get; set; }
}
