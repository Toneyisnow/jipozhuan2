using UnityEngine;
using JiPoZhuan.Managers;

namespace JiPoZhuan.UIObjects
{
    /// <summary>
    /// 背景元素UI - 从右向左匀速移动，模拟飞行纵深效果
    /// 不参与物理碰撞，纯视觉对象
    /// </summary>
    public class UIBackground : MonoBehaviour
    {
        private float moveSpeed;
        private Camera mainCamera;

        public void Initialize(float speed)
        {
            moveSpeed = speed;
            mainCamera = Camera.main;
        }

        private void Update()
        {
            transform.position += Vector3.left * moveSpeed * Time.deltaTime;

            // 离开游戏屏幕左边缘后销毁
            if (mainCamera != null)
            {
                Vector3 vp = mainCamera.WorldToViewportPoint(transform.position);
                if (vp.x < -GameConstants.ScreenBuffer)
                    Destroy(gameObject);
            }
        }
    }
}
