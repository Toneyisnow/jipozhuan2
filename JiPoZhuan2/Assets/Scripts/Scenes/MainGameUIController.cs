using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
using JiPoZhuan.Managers;

namespace JiPoZhuan.Scenes
{
    /// <summary>
    /// 主游戏场景UI控制器 - 显示分数、HP等HUD信息
    /// </summary>
    public class MainGameUIController : MonoBehaviour
    {
        [Header("HUD")]
        [SerializeField] private TextMeshProUGUI scoreText;
        [SerializeField] private TextMeshProUGUI hpText;
        [SerializeField] private TextMeshProUGUI levelText;

        [Header("Game Over Panel")]
        [SerializeField] private GameObject gameOverPanel;
        [SerializeField] private TextMeshProUGUI gameOverScoreText;

        [Header("Pause Panel")]
        [SerializeField] private GameObject pausePanel;

        private void Start()
        {
            if (gameOverPanel != null)
                gameOverPanel.SetActive(false);

            if (pausePanel != null)
                pausePanel.SetActive(false);

            UpdateLevelText();

            if (GameManager.Instance != null)
            {
                GameManager.Instance.OnScoreChanged += UpdateScoreText;
                GameManager.Instance.OnGameOver += ShowGameOver;
                GameManager.Instance.OnPauseChanged += ShowPause;
            }
        }

        private void OnDestroy()
        {
            if (GameManager.Instance != null)
            {
                GameManager.Instance.OnScoreChanged -= UpdateScoreText;
                GameManager.Instance.OnGameOver -= ShowGameOver;
                GameManager.Instance.OnPauseChanged -= ShowPause;
            }
        }

        private void Update()
        {
            if (Keyboard.current != null && Keyboard.current.escapeKey.wasPressedThisFrame)
            {
                if (GameManager.Instance != null && !GameManager.Instance.IsGameOver)
                    GameManager.Instance.TogglePause();
            }

            UpdateHpText();
        }

        private void UpdateScoreText(int score)
        {
            if (scoreText != null)
                scoreText.text = "分数: " + score;
        }

        private void UpdateHpText()
        {
            if (hpText == null) return;

            UIObjects.UIHero hero = FindObjectOfType<UIObjects.UIHero>();
            if (hero != null)
            {
                hpText.text = "HP: " + hero.GetHp();
            }
        }

        private void UpdateLevelText()
        {
            if (levelText != null && GameManager.Instance != null)
                levelText.text = "第 " + GameManager.Instance.CurrentLevel + " 关";
        }

        private void ShowGameOver()
        {
            if (gameOverPanel != null)
            {
                gameOverPanel.SetActive(true);
                if (gameOverScoreText != null && GameManager.Instance != null)
                    gameOverScoreText.text = "最终分数: " + GameManager.Instance.Score;
            }
        }

        private void ShowPause(bool paused)
        {
            if (pausePanel != null)
                pausePanel.SetActive(paused);
        }

        // Button callbacks
        public void OnRestartClicked()
        {
            if (GameManager.Instance != null)
                GameManager.Instance.RestartLevel();
        }

        public void OnMainMenuClicked()
        {
            if (GameManager.Instance != null)
                GameManager.Instance.ReturnToMainMenu();
        }

        public void OnResumeClicked()
        {
            if (GameManager.Instance != null)
                GameManager.Instance.TogglePause();
        }
    }
}
