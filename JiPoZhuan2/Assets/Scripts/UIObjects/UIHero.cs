using UnityEngine;
using UnityEngine.InputSystem;
using JiPoZhuan.Managers;
using JiPoZhuan.Models;

namespace JiPoZhuan.UIObjects
{
    /// <summary>
    /// Ӣ��UI - ��"��"�ֱ�ʾ����ҷɻ�
    /// WASD�����ƶ����������ҷ����ӵ�
    /// </summary>
    public class UIHero : BaseUIObject
    {
        [Header("Movement")]
        [SerializeField] private float moveSpeed = 8f;
        [SerializeField] private float boundaryPadding = 0.5f;

        [Header("Shooting")]
        [SerializeField] private float fireRate = 0.3f;

        private float fireTimer;
        private Camera mainCamera;
        private Hero heroModel;
        private Vector3 moveInput;

        protected override void Awake()
        {
            base.Awake();
            mainCamera = Camera.main;
        }

        private void Start()
        {
            SetCharacter("击", 10f);
            heroModel = new Hero(0, 5);
            UpdateColliderSize();
            if (boxCollider != null)
                boxCollider.isTrigger = true;
        }

        private void Update()
        {
            ReadMoveInput();
            HandleShooting();
        }

        private void FixedUpdate()
        {
            ApplyMovement();
        }

        private void ReadMoveInput()
        {
            float horizontal = 0f;
            float vertical = 0f;

            Keyboard kb = Keyboard.current;
            if (kb == null) { moveInput = Vector3.zero; return; }

            if (kb.wKey.isPressed) vertical = 1f;
            if (kb.sKey.isPressed) vertical = -1f;
            if (kb.aKey.isPressed) horizontal = -1f;
            if (kb.dKey.isPressed) horizontal = 1f;

            moveInput = new Vector3(horizontal, vertical, 0f).normalized;
        }

        private void ApplyMovement()
        {
            Vector3 newPos = rb.position + moveInput * moveSpeed * Time.fixedDeltaTime;

            if (mainCamera != null)
            {
                float halfHeight = mainCamera.orthographicSize;
                float halfWidth = halfHeight * mainCamera.aspect;
                newPos.x = Mathf.Clamp(newPos.x, -halfWidth + boundaryPadding, halfWidth - boundaryPadding);
                newPos.y = Mathf.Clamp(newPos.y, -halfHeight + boundaryPadding, halfHeight - boundaryPadding);
                newPos.z = 0f;
            }

            rb.MovePosition(newPos);
        }

        private void HandleShooting()
        {
            fireTimer -= Time.deltaTime;
            if (fireTimer <= 0f)
            {
                FireBullet();
                fireTimer = fireRate;
            }
        }

        private void FireBullet()
        {
            Vector3 spawnPos = transform.position + Vector3.right * 1f;
            PrefabFactory.CreateHeroBullet(spawnPos);
        }

public void TakeDamage(int damage)
        {
            heroModel.TakeDamage(damage);
            if (!heroModel.IsAlive)
            {
                OnDeath();
            }
        }

        public int GetHp() => heroModel.HpCurrent;

        private void OnDeath()
        {
            if (GameManager.Instance != null)
                GameManager.Instance.OnHeroDied();
            PlayDestroyEffect();
        }

        private void OnTriggerEnter(Collider other)
        {
            // Enemy contact damage (UIEnemyBullet damage is handled by UIEnemyBullet.OnTriggerEnter)
            UIEnemy enemy = other.GetComponent<UIEnemy>();
            if (enemy != null)
            {
                TakeDamage(1);
                enemy.TakeDamage(999);
            }
        }

        protected override void LateUpdate()
        {
            // Hero should not be destroyed when off-screen
        }
    }
}

