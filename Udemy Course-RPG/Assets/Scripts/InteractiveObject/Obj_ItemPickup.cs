using UnityEngine;

public class Obj_ItemPickup : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    [SerializeField] private ItemDataSO itemData;

    private Inventory_Item itemToAdd;
    public Inventory_Base playerInventory;

    private void Start()
    {
        itemToAdd = new Inventory_Item(itemData);
    }

    private void OnValidate()
    {
        if (itemData == null)
            return;
        if (spriteRenderer == null)
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
        }
            spriteRenderer.sprite = itemData.icon;
            gameObject.name = "Obj_ItemPickup - " + itemData.itemName;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        playerInventory = collision.GetComponent<Inventory_Base>();
        if (playerInventory == null || !playerInventory.CanAddItem())
            return;

        playerInventory.AddItem(itemToAdd);
        Debug.Log("Picked up: " + itemData.itemName);
        Destroy(gameObject);
    }
}
