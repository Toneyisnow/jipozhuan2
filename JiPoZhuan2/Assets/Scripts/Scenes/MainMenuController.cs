using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

namespace JiPoZhuan.Scenes
{
    /// <summary>
    /// 主菜单场景 - 开场画面
    /// </summary>
    public class MainMenuController : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI titleText;
        [SerializeField] private TextMeshProUGUI startButtonText;
        [SerializeField] private TextMeshProUGUI settingsButtonText;
        [SerializeField] private TextMeshProUGUI quitButtonText;

        private void Start()
        {
            Time.timeScale = 1f;

            if (titleText != null)
                titleText.text = "击 破 传";
        }

        public void OnStartGame()
        {
            if (JiPoZhuan.Managers.GameManager.Instance != null)
            {
                JiPoZhuan.Managers.GameManager.Instance.StartGame(1);
            }
            else
            {
                SceneManager.LoadScene(JiPoZhuan.Managers.GameConstants.SceneMainGame);
            }
        }

        public void OnOpenSettings()
        {
            SceneManager.LoadScene(JiPoZhuan.Managers.GameConstants.SceneSettings);
        }

        public void OnQuitGame()
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }
    }
}
