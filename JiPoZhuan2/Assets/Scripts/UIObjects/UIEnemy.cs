using UnityEngine;
using JiPoZhuan.Managers;
using JiPoZhuan.Models;
using JiPoZhuan.Models.Definitions;

namespace JiPoZhuan.UIObjects
{
    /// <summary>
    /// 敌人UI - 通过Enemy模型管理HP/得分等数据逻辑
    /// </summary>
    public class UIEnemy : BaseUIObject
    {
        private Enemy enemyModel;

        private Vector3 startPosition;
        private float elapsedTime;
        private float fireTimer;
        private bool initialized;

        protected override void Awake()
        {
            base.Awake();
        }

        /// <summary>
        /// 从EnemyDefinition初始化
        /// </summary>
        public void Initialize(EnemyDefinition enemyDef)
        {
            enemyModel = new Enemy(GetHashCode(), enemyDef);
            startPosition = transform.position;
            elapsedTime = 0f;

            SetCharacter(enemyModel.DisplaySinograph.Character, 8f);
            UpdateColliderSize();

            if (boxCollider != null)
                boxCollider.isTrigger = true;

            ResetFireTimer();
            initialized = true;
        }

        private void Update()
        {
            if (!initialized) return;

            FirePattern fp = enemyModel.Definition.FirePattern;
            if (fp != null && fp.Type != FirePatternType.None)
            {
                fireTimer -= Time.deltaTime;
                if (fireTimer <= 0f)
                {
                    Fire();
                    ResetFireTimer();
                }
            }
        }

        private void FixedUpdate()
        {
            if (!initialized) return;

            elapsedTime += Time.fixedDeltaTime;
            rb.MovePosition(enemyModel.Definition.Trajectory.Evaluate(startPosition, elapsedTime));
        }

        private void ResetFireTimer()
        {
            FirePattern fp = enemyModel.Definition.FirePattern;
            if (fp == null || fp.Type == FirePatternType.None)
                return;

            fireTimer = Random.Range(fp.FireIntervalMin, fp.FireIntervalMax);
        }

        private void Fire()
        {
            FirePattern fp = enemyModel.Definition.FirePattern;
            EnemyBulletDefinition bulletDef = EnemyDatabase.GetBullet(fp.BulletDefinitionId);
            if (bulletDef == null) return;

            switch (fp.Type)
            {
                case FirePatternType.Single:
                    SpawnBullet(bulletDef, Vector3.left);
                    break;

                case FirePatternType.Aimed:
                    GameObject heroObj = GameObject.FindWithTag("Player");
                    Vector3 aimedDir = heroObj != null
                        ? (heroObj.transform.position - transform.position).normalized
                        : Vector3.left;
                    SpawnBullet(bulletDef, aimedDir);
                    break;

                case FirePatternType.Spread:
                    float totalAngle = fp.SpreadAngle;
                    int count = fp.BulletCount;
                    float step = count > 1 ? totalAngle / (count - 1) : 0f;
                    float startAngle = -totalAngle / 2f;

                    for (int i = 0; i < count; i++)
                    {
                        float angle = startAngle + step * i;
                        Vector3 dir = Quaternion.Euler(0f, 0f, angle) * Vector3.left;
                        SpawnBullet(bulletDef, dir);
                    }
                    break;
            }
        }

        private void SpawnBullet(EnemyBulletDefinition bulletDef, Vector3 direction)
        {
            Vector3 spawnPos = transform.position + Vector3.left * 0.8f;
            GameObject bulletObj = PrefabFactory.CreateEnemyBullet(spawnPos);

            UIEnemyBullet bulletScript = bulletObj.GetComponent<UIEnemyBullet>();
            if (bulletScript != null)
            {
                bulletScript.Initialize(bulletDef, direction);
            }
        }

        public void TakeDamage(int damage)
        {
            enemyModel.TakeDamage(damage);
            if (!enemyModel.IsAlive)
            {
                OnDeath();
            }
        }

        private void OnDeath()
        {
            if (GameManager.Instance != null)
                GameManager.Instance.AddScore(enemyModel.Definition.ScoreValue);
            PlayDestroyEffect();
        }
    }
}
