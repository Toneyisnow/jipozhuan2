using UnityEngine;
using JiPoZhuan.Models.Definitions;
using JiPoZhuan.UIObjects;

namespace JiPoZhuan.Managers
{
    /// <summary>
    /// </summary>
    public class EnemySpawner : MonoBehaviour
    {
        [Header("Spawn Settings")]
        [SerializeField] private float spawnIntervalMin = 0.5f;
        [SerializeField] private float spawnIntervalMax = 1.5f;
        [SerializeField] private int spawnCountMin = 1;
        [SerializeField] private int spawnCountMax = 2;

        private float spawnTimer;
        private Camera mainCamera;
        private string[] currentLevelEnemyIds;

        private void Start()
        {
            mainCamera = Camera.main;
            EnemyDatabase.Initialize();

            int level = GameManager.Instance != null ? GameManager.Instance.CurrentLevel : 1;
            currentLevelEnemyIds = EnemyDatabase.GetEnemyIdsForLevel(level);

            ResetSpawnTimer();
        }

        private void Update()
        {
            if (GameManager.Instance != null && GameManager.Instance.IsGameOver)
                return;

            spawnTimer -= Time.deltaTime;
            if (spawnTimer <= 0f)
            {
                int count = Random.Range(spawnCountMin, spawnCountMax + 1);
                for (int i = 0; i < count; i++)
                    SpawnEnemy();
                ResetSpawnTimer();
            }
        }

        private void ResetSpawnTimer()
        {
            spawnTimer = Random.Range(spawnIntervalMin, spawnIntervalMax);
        }

        private void SpawnEnemy()
        {
            if (mainCamera == null || currentLevelEnemyIds == null || currentLevelEnemyIds.Length == 0)
                return;

            string enemyId = currentLevelEnemyIds[Random.Range(0, currentLevelEnemyIds.Length)];
            EnemyDefinition def = EnemyDatabase.GetEnemy(enemyId);
            if (def == null) return;

            // Spawn at the game screen right edge (visible screen + ScreenBuffer on each side).
            // Game screen half-extent = visibleHalf * (1 + 2 * ScreenBuffer).
            float halfHeight = mainCamera.orthographicSize;
            float halfWidth = halfHeight * mainCamera.aspect;
            float buf = GameConstants.ScreenBuffer;
            float spawnX = halfWidth * (1f + 2f * buf);
            float spawnY = Random.Range(-halfHeight * (1f + 2f * buf), halfHeight * (1f + 2f * buf));
            Vector3 spawnPos = new Vector3(spawnX, spawnY, 0f);

            GameObject enemyObj = PrefabFactory.CreateEnemy(spawnPos);
            UIEnemy uiEnemy = enemyObj.GetComponent<UIEnemy>();
            if (uiEnemy != null)
            {
                uiEnemy.Initialize(def);
            }
        }
    }
}
