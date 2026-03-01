using UnityEngine;
using TMPro;
using JiPoZhuan.Models.Definitions;

namespace JiPoZhuan.Managers
{
    /// <summary>
    /// 游戏初始化器 - 挂载到MainGameScene中
    /// 纯代码驱动：初始化数据库、创建英雄、启动敌人生成器
    /// </summary>
    public class GameInitializer : MonoBehaviour
    {
        [Header("Font")]
        [SerializeField] private TMP_FontAsset chineseFont;

        private void Start()
        {
            if (chineseFont != null)
                PrefabFactory.SetFont(chineseFont);

            EnemyDatabase.Initialize();
            SetupCamera();
            SetupBackdropSpawner();
            SetupBackgroundSpawner();
            SpawnHero();
            SetupEnemySpawner();
        }

        private void SetupCamera()
        {
            Camera cam = Camera.main;
            if (cam != null)
            {
                cam.orthographic = true;
                cam.orthographicSize = 6f;
                cam.transform.position = new Vector3(0f, 0f, -10f);
                cam.backgroundColor = new Color(0.05f, 0.05f, 0.12f);
            }
        }

        private void SpawnHero()
        {
            Vector3 heroStartPos = new Vector3(-6f, 0f, 0f);
            GameObject heroObj = PrefabFactory.CreateHero(heroStartPos);
            heroObj.name = "Hero";
        }

        private void SetupEnemySpawner()
        {
            EnemySpawner spawner = GetComponent<EnemySpawner>();
            if (spawner == null)
            {
                spawner = gameObject.AddComponent<EnemySpawner>();
            }
        }

        private void SetupBackdropSpawner()
        {
            BackdropSpawner bd = GetComponent<BackdropSpawner>();
            if (bd == null)
            {
                bd = gameObject.AddComponent<BackdropSpawner>();
            }
        }

        private void SetupBackgroundSpawner()
        {
            BackgroundSpawner bg = GetComponent<BackgroundSpawner>();
            if (bg == null)
            {
                bg = gameObject.AddComponent<BackgroundSpawner>();
            }
        }
    }
}
