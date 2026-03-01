using UnityEngine;
using TMPro;
using System.Collections;
using JiPoZhuan.Managers;

namespace JiPoZhuan.UIObjects
{
    /// <summary>
    /// UI������� - ������Ϸ�пɼ�����Ļ���
    /// ʹ��TextMeshPro��3D������չʾ����
    /// </summary>
    [RequireComponent(typeof(BoxCollider))]
    [RequireComponent(typeof(Rigidbody))]
    public abstract class BaseUIObject : MonoBehaviour
    {
        [SerializeField] protected TextMeshPro textMesh;
        protected BoxCollider boxCollider;
        protected Rigidbody rb;

        private float offScreenTimer;
        private const float OffScreenLifetime = 1f;
        private bool isOffScreen;

        protected virtual void Awake()
        {
            if (textMesh == null)
                textMesh = GetComponentInChildren<TextMeshPro>();

            boxCollider = GetComponent<BoxCollider>();

            rb = GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.useGravity = false;
                rb.isKinematic = true;
            }
        }

        public virtual void SetCharacter(string character, float fontSize = 8f)
        {
            if (textMesh != null)
            {
                textMesh.text = character;
                textMesh.fontSize = fontSize;
                textMesh.alignment = TextAlignmentOptions.Center;
            }
        }

        protected void UpdateColliderSize()
        {
            if (textMesh != null && boxCollider != null)
            {
                textMesh.ForceMeshUpdate();
                Vector3 textSize = textMesh.bounds.size;
                float minSize = 0.5f;
                float sx = Mathf.Max(textSize.x + 0.2f, minSize);
                float sy = Mathf.Max(textSize.y + 0.2f, minSize);
                boxCollider.size = new Vector3(sx, sy, 0.5f);
                // textMesh.bounds is in the TextMeshPro component's local space.
                // Since textChild sits at localPosition (0,0,0), its local space
                // equals the parent's local space — use the center directly.
                boxCollider.center = textMesh.bounds.center;
            }
        }

        protected virtual void LateUpdate()
        {
            CheckOffScreen();
        }

        private void CheckOffScreen()
        {
            Camera cam = Camera.main;
            if (cam == null) return;

            Vector3 vp = cam.WorldToViewportPoint(transform.position);
            float buf = GameConstants.ScreenBuffer;
            bool outside = vp.x < -buf || vp.x > 1f + buf || vp.y < -buf || vp.y > 1f + buf;

            if (outside)
            {
                offScreenTimer += Time.deltaTime;
                if (offScreenTimer > OffScreenLifetime)
                {
                    Destroy(gameObject);
                }
            }
            else
            {
                offScreenTimer = 0f;
            }
        }

        /// <summary>
        /// ��ըЧ�������ֱ���͸��������
        /// </summary>
        public void PlayDestroyEffect()
        {
            StartCoroutine(DestroyEffectCoroutine());
        }

        private IEnumerator DestroyEffectCoroutine()
        {
            // ������ײ����ֹ�ظ�����
            if (boxCollider != null)
                boxCollider.enabled = false;

            float duration = 0.3f;
            float elapsed = 0f;
            Vector3 originalScale = transform.localScale;
            Color originalColor = textMesh != null ? textMesh.color : Color.white;

            while (elapsed < duration)
            {
                elapsed += Time.deltaTime;
                float t = elapsed / duration;

                // �Ŵ�
                transform.localScale = originalScale * (1f + t * 1.5f);

                // ����
                if (textMesh != null)
                {
                    Color c = originalColor;
                    c.a = 1f - t;
                    textMesh.color = c;
                }

                yield return null;
            }

            Destroy(gameObject);
        }
    }
}
