using UnityEngine;

public class Obj_NPC : MonoBehaviour
{
    protected Transform player;
    protected UI ui;
    private bool facingRight = true;
    [SerializeField] private Transform NPC;
    [SerializeField] private GameObject interactToolTip;
    [Header("Floating Tooltip Settings")]
    [SerializeField] float floatSpeed = 6f;
    [SerializeField] float floatRange = 0.08f;
    private Vector3 startPos;
    protected virtual void Awake()
    {
        ui = FindFirstObjectByType<UI>();
        startPos = interactToolTip.transform.position;
        interactToolTip.SetActive(false);
    }
    protected virtual void Update()
    {
        HandleNpcFlip();
        HandleToolTipFloat();
    }
    private void HandleToolTipFloat()
    {
        if (interactToolTip.activeSelf)
        {
            float yOffset = Mathf.Sin(Time.time * floatSpeed) * floatRange;
            interactToolTip.transform.position = startPos + new Vector3(0, yOffset, 0);
        }
    }
    private void HandleNpcFlip()
    {
        if(player == null || NPC == null) return;
        
        if(NPC.position.x > player.position.x && facingRight)
        {
            NPC.transform.Rotate(0, 180, 0);
            facingRight = false;
            
        }
        else if(NPC.position.x < player.position.x && facingRight == false)
        {
            NPC.transform.Rotate(0, 180, 0);
            facingRight = true;

        }
        if(interactToolTip.activeSelf)
        {

                interactToolTip.transform.rotation = Quaternion.identity;

        }

    }

    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        player = collision.transform;
        interactToolTip.SetActive(true);
    }
    protected virtual void OnTriggerExit2D(Collider2D collision)
    {
        interactToolTip.SetActive(false);
    }
}
