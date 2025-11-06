using UnityEngine;

namespace RPGSystem.Character
{
    /// <summary>
    /// Player controller for RPG character
    /// </summary>
    [RequireComponent(typeof(CharacterController))]
    public class PlayerController : MonoBehaviour
    {
        [Header("Movement")]
        [SerializeField] private float moveSpeed = 5f;
        [SerializeField] private float sprintSpeed = 8f;
        [SerializeField] private float rotationSpeed = 10f;
        [SerializeField] private float gravity = -9.81f;
        [SerializeField] private float jumpHeight = 1.5f;
        
        [Header("Character Stats")]
        [SerializeField] private CharacterStats stats;
        
        private CharacterController controller;
        private Vector3 velocity;
        private bool isGrounded;
        private bool isSprinting;
        
        public CharacterStats Stats => stats;
        
        private void Awake()
        {
            controller = GetComponent<CharacterController>();
            
            if (stats == null)
            {
                stats = new CharacterStats();
                stats.CharacterName = "Player";
            }
            
            stats.Initialize();
        }
        
        private void Update()
        {
            HandleMovement();
            HandleInput();
        }
        
        private void HandleMovement()
        {
            isGrounded = controller.isGrounded;
            
            if (isGrounded && velocity.y < 0)
            {
                velocity.y = -2f;
            }
            
            float horizontal = Input.GetAxis("Horizontal");
            float vertical = Input.GetAxis("Vertical");
            
            Vector3 move = transform.right * horizontal + transform.forward * vertical;
            
            float currentSpeed = isSprinting ? sprintSpeed : moveSpeed;
            controller.Move(move * currentSpeed * Time.deltaTime);
            
            // Rotate character based on mouse movement
            float mouseX = Input.GetAxis("Mouse X") * rotationSpeed;
            transform.Rotate(Vector3.up * mouseX);
            
            // Jump
            if (Input.GetButtonDown("Jump") && isGrounded)
            {
                velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
            }
            
            // Apply gravity
            velocity.y += gravity * Time.deltaTime;
            controller.Move(velocity * Time.deltaTime);
        }
        
        private void HandleInput()
        {
            // Sprint
            isSprinting = Input.GetKey(KeyCode.LeftShift);
            
            // Attack (example)
            if (Input.GetMouseButtonDown(0))
            {
                PerformAttack();
            }
            
            // Use ability (example)
            if (Input.GetKeyDown(KeyCode.Q))
            {
                UseAbility();
            }
        }
        
        private void PerformAttack()
        {
            Debug.Log($"{stats.CharacterName} performs an attack with {stats.AttackPower} power!");
            // Attack logic would go here
        }
        
        private void UseAbility()
        {
            float manaCost = 10f;
            
            if (stats.UseMana(manaCost))
            {
                Debug.Log($"{stats.CharacterName} uses an ability!");
                // Ability logic would go here
            }
            else
            {
                Debug.Log("Not enough mana!");
            }
        }
        
        public void TakeDamage(float damage)
        {
            stats.TakeDamage(damage);
            
            if (stats.IsDead)
            {
                HandleDeath();
            }
        }
        
        private void HandleDeath()
        {
            Debug.Log("Player has died! Game Over.");
            // Death handling logic
        }
    }
}
