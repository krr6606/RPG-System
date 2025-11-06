using UnityEngine;
using UnityEngine.SceneManagement;

namespace RPGSystem
{
    /// <summary>
    /// Main game manager for the RPG system
    /// </summary>
    public class GameManager : MonoBehaviour
    {
        [Header("Game Settings")]
        [SerializeField] private bool isPaused = false;
        [SerializeField] private float gameTime = 0f;
        
        public static GameManager Instance { get; private set; }
        
        public bool IsPaused => isPaused;
        public float GameTime => gameTime;
        
        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }
        
        private void Start()
        {
            InitializeGame();
        }
        
        private void Update()
        {
            if (!isPaused)
            {
                gameTime += Time.deltaTime;
            }
            
            HandleInput();
        }
        
        private void InitializeGame()
        {
            Debug.Log("Game initialized");
            
            // Set target frame rate
            Application.targetFrameRate = 60;
            
            // Initialize game systems
            if (Combat.CombatSystem.Instance == null)
            {
                GameObject combatSystem = new GameObject("CombatSystem");
                combatSystem.AddComponent<Combat.CombatSystem>();
            }
            
            if (Inventory.InventorySystem.Instance == null)
            {
                GameObject inventorySystem = new GameObject("InventorySystem");
                inventorySystem.AddComponent<Inventory.InventorySystem>();
            }
            
            if (Quest.QuestManager.Instance == null)
            {
                GameObject questManager = new GameObject("QuestManager");
                questManager.AddComponent<Quest.QuestManager>();
            }
        }
        
        private void HandleInput()
        {
            // Pause/Unpause
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                TogglePause();
            }
        }
        
        public void TogglePause()
        {
            isPaused = !isPaused;
            Time.timeScale = isPaused ? 0f : 1f;
            
            Debug.Log(isPaused ? "Game Paused" : "Game Resumed");
        }
        
        public void LoadScene(string sceneName)
        {
            SceneManager.LoadScene(sceneName);
        }
        
        public void RestartGame()
        {
            Time.timeScale = 1f;
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
        
        public void QuitGame()
        {
            Debug.Log("Quitting game...");
            Application.Quit();
            
            #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
            #endif
        }
        
        public void SaveGame()
        {
            Debug.Log("Game saved");
            // Implement save system here
        }
        
        public void LoadGame()
        {
            Debug.Log("Game loaded");
            // Implement load system here
        }
    }
}
