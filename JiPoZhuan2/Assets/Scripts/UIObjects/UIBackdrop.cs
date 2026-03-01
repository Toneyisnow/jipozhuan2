using UnityEngine;

namespace JiPoZhuan.UIObjects
{
    /// <summary>
    /// 底部幕布字符 - 到达左边界时循环回右侧，实现无缝横向滚动
    /// 与 UIBackground 的区别：不销毁，循环复用
    /// </summary>
    public class UIBackdrop : MonoBehaviour
    {
        private float moveSpeed;
        private float rowWidth;   // 一行字符的总跨度，用于循环偏移
        private float leftEdgeX;  // 触发循环的左边界

        /// <summary>
        /// speed: 向左移动速度；rowWidth: 字符行总宽度；leftEdgeX: 循环触发X坐标
        /// </summary>
        public void Initialize(float speed, float rowWidth, float leftEdgeX)
        {
            moveSpeed = speed;
            this.rowWidth = rowWidth;
            this.leftEdgeX = leftEdgeX;
        }

        private void Update()
        {
            transform.position += Vector3.left * moveSpeed * Time.deltaTime;

            // 超出左边界：将自身平移一个行宽，无缝衔接到右侧
            if (transform.position.x < leftEdgeX)
            {
                Vector3 pos = transform.position;
                pos.x += rowWidth;
                transform.position = pos;
            }
        }
    }
}
