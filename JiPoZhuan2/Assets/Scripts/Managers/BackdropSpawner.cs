using UnityEngine;
using JiPoZhuan.UIObjects;

namespace JiPoZhuan.Managers
{
    /// <summary>
    /// 底部幕布生成器 - 在游戏启动时一次性生成 5 层密集"云"字幕布
    ///
    /// 布局：
    ///   · 占屏幕底部 2/5 高度
    ///   · 5 层水平分布，层间距按近大远小插值（NearLayerStep → FarLayerStep），归一化到 BackdropFrac
    ///   · 每层字符间距按近大远小比例插值（NearCharSpacing → FarCharSpacing）
    ///   · 字符到达左边界后无缝循环回右侧
    ///
    /// 速度：
    ///   · 底层（近）= NearSpeed = 6.5f，与 BackgroundSpawner 近雾层一致
    ///   · 顶层（远）= FarSpeed  = 1.2f，与 BackgroundSpawner 远云层一致
    ///   · 中间层线性插值
    ///
    /// 视觉：
    ///   · 底层字体最大、最不透明，营造"近处地表"感
    ///   · 顶层字体最小、最透明，营造"远处天际"感
    /// </summary>
    public class BackdropSpawner : MonoBehaviour
    {
        // 速度端点与 BackgroundSpawner 层对齐
        private const float NearSpeed    = 6.5f;
        private const float FarSpeed     = 1.2f;

        private const int   LayerCount      = 5;
        private const float BackdropFrac   = 0.4f;   // 屏幕高度占比 2/5
        private const float NearCharSpacing = 4.5f;  // 底层（近/大）字符间距
        private const float FarCharSpacing  = 0.8f;  // 顶层（远/小）字符间距
        private const float NearLayerStep   = 2.0f;  // 底层间距权重（大）
        private const float FarLayerStep    = 1.0f;  // 顶层间距权重（小）

        private const float NearFontSize = 24f;
        private const float FarFontSize  = 4f;
        private const float NearAlpha    = 0.48f;
        private const float FarAlpha     = 0.18f;

        private void Start()
        {
            Camera cam = Camera.main;
            if (cam == null) return;

            float halfHeight     = cam.orthographicSize;                     // 6
            float halfWidth      = halfHeight * cam.aspect;                  // ~10.67
            float buf            = GameConstants.ScreenBuffer;               // 0.2

            float screenBottom   = -halfHeight;                              // -6
            float backdropHeight = halfHeight * 2f * BackdropFrac;          // 4.8 units

            // 预计算各层Y坐标：累加lerp步长，再归一化到backdropHeight
            // 底层步长大（NearLayerStep），顶层步长小（FarLayerStep）
            float[] layerYs = new float[LayerCount];
            layerYs[0] = 0f;
            for (int i = 0; i < LayerCount - 1; i++)
            {
                float tStep = i / (LayerCount - 2f);
                layerYs[i + 1] = layerYs[i] + Mathf.Lerp(NearLayerStep, FarLayerStep, tStep);
            }
            float totalRaw = layerYs[LayerCount - 1];
            for (int i = 0; i < LayerCount; i++)
                layerYs[i] = screenBottom + layerYs[i] / totalRaw * backdropHeight;

            float leftEdgeX      = -halfWidth * (1f + buf);                 // 游戏屏幕左边界
            float rightEdgeX     =  halfWidth * (1f + buf);                 // 游戏屏幕右边界
            float gameScreenW    = rightEdgeX - leftEdgeX;

            for (int layerIdx = 0; layerIdx < LayerCount; layerIdx++)
            {
                // t: 0 = 底层（近/快/大），1 = 顶层（远/慢/小）
                float t = layerIdx / (LayerCount - 1f);

                float layerY      = layerYs[layerIdx];
                float speed       = Mathf.Lerp(NearSpeed,       FarSpeed,       t);
                float fontSize    = Mathf.Lerp(NearFontSize,    FarFontSize,    t);
                float alpha       = Mathf.Lerp(NearAlpha,       FarAlpha,       t);
                float charSpacing = Mathf.Lerp(NearCharSpacing, FarCharSpacing, t);
                // sortingOrder: 近层（-4）在远层（-8）之上；均低于游戏对象（1）和背景云（-2/-3）
                int   sortOrder   = Mathf.RoundToInt(Mathf.Lerp(-4f, -8f, t));
                float zPos        = Mathf.Lerp(5f, 9f, t);

                // charCount 和 rowWidth 按本层间距计算，保证无缝循环
                int   charCount = Mathf.CeilToInt(gameScreenW / charSpacing) + 1;
                float rowWidth  = charCount * charSpacing;

                Color color = new Color(0.75f, 0.88f, 1f, alpha);

                for (int charIdx = 0; charIdx < charCount; charIdx++)
                {
                    float charX = leftEdgeX + charIdx * charSpacing;
                    Vector3 pos = new Vector3(charX, layerY, zPos);

                    GameObject go = PrefabFactory.CreateBackdropChar(pos, fontSize, color, sortOrder);
                    go.GetComponent<UIBackdrop>().Initialize(speed, rowWidth, leftEdgeX);
                }
            }
        }
    }
}
