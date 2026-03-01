using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

namespace JiPoZhuan.Scenes
{
    /// <summary>
    /// 设置场景控制器
    /// </summary>
    public class SettingsController : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI titleText;

        private void Start()
        {
            if (titleText != null)
                titleText.text = "设 置";
        }

        public void OnBackToMenu()
        {
            SceneManager.LoadScene(JiPoZhuan.Managers.GameConstants.SceneMainMenu);
        }

        public void SetVolume(float volume)
        {
            AudioListener.volume = volume;
        }
    }
}
