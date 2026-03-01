using UnityEngine;
using JiPoZhuan.Models;
using JiPoZhuan.Models.Definitions;

namespace JiPoZhuan.UIObjects
{
    /// <summary>
    /// �����ӵ�UI - ͨ��EnemyBulletDefinition����
    /// </summary>
    public class UIEnemyBullet : BaseUIObject
    {
        private EnemyBullet enemyBulletModel;
        private TrajectoryDefinition trajectory;
        private Vector3 startPosition;
        private float elapsedTime;
        private bool initialized;

        // ���ڸ��ӵ�һ���Զ��巢�䷽�򣨸��Ƕ����е�Ĭ�Ϸ���
        private Vector3 overrideDirection;
        private float overrideSpeed;

        public int Damage => enemyBulletModel.Damage;

        /// <summary>
        /// ��BulletDefinition�ͷ��䷽���ʼ��
        /// </summary>
        public void Initialize(EnemyBulletDefinition bulletDef, Vector3 direction)
        {
            enemyBulletModel = new EnemyBullet(GetHashCode(), bulletDef);
            overrideDirection = direction.normalized;
            overrideSpeed = bulletDef.Trajectory.Speed;

            // ����һ�����ڷ��䷽��Ĺ켣
            trajectory = new TrajectoryDefinition(
                bulletDef.Trajectory.Type,
                new Vector2(overrideDirection.x, overrideDirection.y),
                overrideSpeed,
                bulletDef.Trajectory.Amplitude,
                bulletDef.Trajectory.Frequency,
                bulletDef.Trajectory.PhaseOffset
            );

            startPosition = transform.position;
            elapsedTime = 0f;

            SetCharacter(bulletDef.DisplaySinograph.Character, 5f);
            UpdateColliderSize();

            if (boxCollider != null)
                boxCollider.isTrigger = true;

            initialized = true;
        }

        private void FixedUpdate()
        {
            if (!initialized) return;

            elapsedTime += Time.fixedDeltaTime;
            rb.MovePosition(trajectory.Evaluate(startPosition, elapsedTime));
        }

        private void OnTriggerEnter(Collider other)
        {
            UIHero hero = other.GetComponent<UIHero>();
            if (hero != null)
            {
                hero.TakeDamage(enemyBulletModel.Damage);
                Destroy(gameObject);
            }
        }
    }
}
