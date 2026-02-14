using UnityEngine;

public class Obj_ItemPickup : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;

    [SerializeField] private ItemDataSO itemData;





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
        Inventory_Item itemToAdd = new Inventory_Item(itemData);
        collision.TryGetComponent<Inventory_Player>(out var playerInventory);
        Inventory_Storage storage = playerInventory.storage;

        if(itemData.itemType == ItemType.Matetial)
        {
            storage.AddMaterialToStash(itemToAdd);
            Debug.Log("Picked up: " + itemData.itemName);
            Destroy(gameObject);
            return;
        }

        if (playerInventory.CanAddItem(itemToAdd))
        {
            playerInventory.AddItem(itemToAdd);
            Debug.Log("Picked up: " + itemData.itemName);
            Destroy(gameObject);
        }

    }
}
