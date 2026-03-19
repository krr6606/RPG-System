using UnityEngine;

public class Obj_Blacksmith : Obj_NPC, IInteractable
{
    private Animator animator;
    private Inventory_Player playerInventory;
    private Inventory_Storage storage;
    protected override void Awake()
    {
        base.Awake();
        storage = GetComponent<Inventory_Storage>();
        animator = GetComponentInChildren<Animator>();
        animator.SetBool("isBlacksmith", true);
    }
    public void Interact()
    {
        ui.storageUI.SetupStorageUI(storage);
        ui.craftUI.SetupCraftUI(storage);
        ui.storageUI.gameObject.SetActive(true);
        //ui.craftUI.gameObject.SetActive(true);
    }
    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        base.OnTriggerEnter2D(collision);
        playerInventory = player.GetComponent<Inventory_Player>();
        storage.SetInventory(playerInventory);
    }
    protected override void OnTriggerExit2D(Collider2D collision)
    {
        base.OnTriggerExit2D(collision);
        ui.SwitchOffAllToolTips();
        ui.storageUI.gameObject.SetActive(false);
        ui.craftUI.gameObject.SetActive(false);
    }
}
