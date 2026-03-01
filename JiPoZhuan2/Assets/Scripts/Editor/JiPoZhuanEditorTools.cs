using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;
using JiPoZhuan.Managers;

namespace JiPoZhuan.Editor
{
    /// <summary>
    /// 编辑器工具 - 快速搭建场景
    /// 所有游戏对象都由代码动态生成，此工具仅用于快速创建场景和初始化对象
    /// </summary>
    public class JiPoZhuanEditorTools
    {
        [MenuItem("JiPoZhuan/Setup MainGame Scene")]
        public static void SetupMainGameScene()
        {
            // 创建 GameManager (如果不存在)
            if (Object.FindObjectOfType<GameManager>() == null)
            {
                GameObject gmObj = new GameObject("GameManager");
                gmObj.AddComponent<GameManager>();
                Debug.Log("击破传: GameManager 已创建");
            }

            // 创建 GameInitializer + EnemySpawner
            if (Object.FindObjectOfType<GameInitializer>() == null)
            {
                GameObject initObj = new GameObject("GameInitializer");
                initObj.AddComponent<GameInitializer>();
                initObj.AddComponent<EnemySpawner>();
                Debug.Log("击破传: GameInitializer + EnemySpawner 已创建");
            }

            Debug.Log("击破传: MainGame场景设置完成! 运行即可开始游戏。");
        }

        [MenuItem("JiPoZhuan/Create Empty Scenes")]
        public static void CreateEmptyScenes()
        {
            string[] sceneNames = { "MainMenuScene", "SettingsScene", "MainGameScene" };

            if (!AssetDatabase.IsValidFolder("Assets/Scenes"))
            {
                AssetDatabase.CreateFolder("Assets", "Scenes");
            }

            foreach (string sceneName in sceneNames)
            {
                string path = "Assets/Scenes/" + sceneName + ".unity";
                if (!System.IO.File.Exists(path))
                {
                    var scene = EditorSceneManager.NewScene(NewSceneSetup.DefaultGameObjects, NewSceneMode.Single);
                    EditorSceneManager.SaveScene(scene, path);
                    Debug.Log("击破传: 创建场景 " + path);
                }
            }

            AssetDatabase.Refresh();
            Debug.Log("击破传: 所有场景创建完成!");
        }

        [MenuItem("JiPoZhuan/List All Enemy Definitions")]
        public static void ListAllEnemyDefinitions()
        {
            JiPoZhuan.Models.Definitions.EnemyDatabase.Initialize();

            Debug.Log("=== 击破传 敌人定义列表 ===");
            foreach (var enemy in JiPoZhuan.Models.Definitions.EnemyDatabase.GetAllEnemies())
            {
                Debug.Log(string.Format("[{0}] {1} ('{2}') HP:{3} 分:{4} 轨迹:{5} 射击:{6}",
                    enemy.Id, enemy.EnemyName, enemy.DisplaySinograph.Character,
                    enemy.HpMax, enemy.ScoreValue,
                    enemy.Trajectory.Type, enemy.FirePattern.Type));
            }

            Debug.Log("=== 击破传 子弹定义列表 ===");
            foreach (var bullet in JiPoZhuan.Models.Definitions.EnemyDatabase.GetAllBullets())
            {
                Debug.Log(string.Format("[{0}] '{1}' 伤害:{2} 轨迹:{3} 速度:{4}",
                    bullet.Id, bullet.DisplaySinograph.Character,
                    bullet.Damage, bullet.Trajectory.Type, bullet.Trajectory.Speed));
            }
        }
    }
}
