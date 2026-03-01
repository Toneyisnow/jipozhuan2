using UnityEngine;
using UnityEngine.SceneManagement;

namespace JiPoZhuan.Managers
{
    /// <summary>
    /// 游戏管理器 - 控制游戏全局状态
    /// </summary>
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance { get; private set; }

        [Header("Game State")]
        [SerializeField] private int currentLevel = 1;
        [SerializeField] private int totalLevels = 6;

        private int score;
        private bool isGameOver;
        private bool isPaused;

        public int Score => score;
        public int CurrentLevel => currentLevel;
        public bool IsGameOver => isGameOver;
        public bool IsPaused => isPaused;

        public event System.Action<int> OnScoreChanged;
        public event System.Action OnGameOver;
        public event System.Action<bool> OnPauseChanged;

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
            DontDestroyOnLoad(gameObject);
        }

        public void StartGame(int level = 1)
        {
            currentLevel = level;
            score = 0;
            isGameOver = false;
            isPaused = false;
            Time.timeScale = 1f;
            SceneManager.LoadScene(GameConstants.SceneMainGame);
        }

        public void AddScore(int points)
        {
            if (isGameOver) return;

            score += points;
            OnScoreChanged?.Invoke(score);
        }

        public void OnHeroDied()
        {
            isGameOver = true;
            Time.timeScale = 0f;
            OnGameOver?.Invoke();
        }

        public void TogglePause()
        {
            isPaused = !isPaused;
            Time.timeScale = isPaused ? 0f : 1f;
            OnPauseChanged?.Invoke(isPaused);
        }

        public void ReturnToMainMenu()
        {
            Time.timeScale = 1f;
            isGameOver = false;
            SceneManager.LoadScene(GameConstants.SceneMainMenu);
        }

        public void LoadNextLevel()
        {
            currentLevel++;
            if (currentLevel > totalLevels)
            {
                // 游戏通关
                ReturnToMainMenu();
                return;
            }

            StartGame(currentLevel);
        }

        public void RestartLevel()
        {
            StartGame(currentLevel);
        }
    }
}
