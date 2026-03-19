using UnityEngine;

public class Obj_Merchant : Obj_NPC, IInteractable
{
    private Inventory_Player playerInventory;
    private Inventory_Merchant merchantInventory;

    protected override void Awake()
    {
        base.Awake();
        merchantInventory = GetComponent<Inventory_Merchant>();
    }
    protected override void Update()
    {
        base.Update();

        if (Input.GetKeyDown(KeyCode.Z))
        {
            merchantInventory.FillShopList();
        }
    }
    public void Interact()
    {
        ui.merchantUI.SetupMerchantUI(merchantInventory, playerInventory);
        ui.merchantUI.gameObject.SetActive(true);
    }

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        base.OnTriggerEnter2D(collision);
        playerInventory = collision.GetComponent<Inventory_Player>();
        merchantInventory.SetPlayerInventory(playerInventory);
    }
    protected override void OnTriggerExit2D(Collider2D collision)
    {
        base.OnTriggerExit2D(collision);
        ui.SwitchOffAllToolTips();
        ui.merchantUI.gameObject.SetActive(false);
    }
}

