using UnityEngine;
using JiPoZhuan.Managers;
using JiPoZhuan.Models;

namespace JiPoZhuan.UIObjects
{
    /// <summary>
    /// Ӣ���ӵ�UI - ��"��"�ֱ�ʾ�����ҷ���
    /// </summary>
    public class UIHeroBullet : BaseUIObject
    {
        [SerializeField] private float moveSpeed = 12f;
        [SerializeField] private int damage = 1;

        private HeroBullet heroBulletModel;

        public int Damage => heroBulletModel.Damage;

        private void Start()
        {
            heroBulletModel = new HeroBullet(0, damage, moveSpeed);
            SetCharacter(heroBulletModel.DisplaySinograph.Character, 6f);
            UpdateColliderSize();

            if (boxCollider != null)
                boxCollider.isTrigger = true;
        }

        private void FixedUpdate()
        {
            rb.MovePosition(rb.position + Vector3.right * heroBulletModel.MoveSpeed * Time.fixedDeltaTime);
        }

        private void OnTriggerEnter(Collider other)
        {
            UIEnemy enemy = other.GetComponent<UIEnemy>();
            if (enemy != null)
            {
                enemy.TakeDamage(heroBulletModel.Damage);
                Destroy(gameObject);
            }
        }

        protected override void LateUpdate()
        {
            // Destroy at the game screen right edge (same boundary enemies spawn at)
            // so bullets cannot travel into the spawn zone and hit off-screen enemies.
            Camera cam = Camera.main;
            if (cam == null) return;
            Vector3 vp = cam.WorldToViewportPoint(transform.position);
            if (vp.x > 1f + GameConstants.ScreenBuffer)
                Destroy(gameObject);
        }
    }
}
