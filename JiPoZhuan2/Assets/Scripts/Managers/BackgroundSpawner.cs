using UnityEngine;
using JiPoZhuan.UIObjects;

namespace JiPoZhuan.Managers
{
    /// <summary>
    /// 背景生成器 - 三个纵深层，模拟近大远小的飞行视差效果
    ///   远层 (z=6): 细小云字，缓慢飘移，极淡
    ///   中层 (z=3): 中等云字，中速移动，半透明
    ///   近层 (z=1): 大型雾字，快速掠过，极透明，叠在游戏对象前方
    /// </summary>
    public class BackgroundSpawner : MonoBehaviour
    {
        private struct BgLayer
        {
            public string Character;
            public float FontSize;
            public float Speed;
            public Color Color;
            public int SortingOrder;
            public float ZPos;
            public float SpawnIntervalMin;
            public float SpawnIntervalMax;
        }

        // sortingOrder: 游戏对象为1，远背景为负数，近雾为2（叠前）
        private static readonly BgLayer[] Layers =
        {
            // 远层: 小字、慢速、极淡，营造远天感
            new BgLayer
            {
                Character = "雾", FontSize = 5f, Speed = 1.2f,
                Color = new Color(0.75f, 0.88f, 1f, 0.24f),
                SortingOrder = -3, ZPos = 6f,
                SpawnIntervalMin = 1.5f, SpawnIntervalMax = 3.5f
            },
            // 中层: 中等字号，中速
            new BgLayer
            {
                Character = "雾", FontSize = 8f, Speed = 3.2f,
                Color = new Color(0.85f, 0.92f, 1f, 0.32f),
                SortingOrder = -2, ZPos = 3f,
                SpawnIntervalMin = 1.2f, SpawnIntervalMax = 2.8f
            },
            // 近层: 大字、快速、极透明，叠在游戏对象前方制造纵深感
            new BgLayer
            {
                Character = "雾", FontSize = 18f, Speed = 6.5f,
                Color = new Color(0.78f, 0.88f, 1f, 0.12f),
                SortingOrder = 2, ZPos = 1f,
                SpawnIntervalMin = 2.0f, SpawnIntervalMax = 4.5f
            },
        };

        private float[] timers;

        private void Start()
        {
            timers = new float[Layers.Length];
            // 各层稍微错开，避免第一帧全部同时出现
            for (int i = 0; i < timers.Length; i++)
                timers[i] = i * 0.4f;
        }

        private void Update()
        {
            for (int i = 0; i < Layers.Length; i++)
            {
                timers[i] -= Time.deltaTime;
                if (timers[i] <= 0f)
                {
                    SpawnLayer(i);
                    timers[i] = Random.Range(Layers[i].SpawnIntervalMin, Layers[i].SpawnIntervalMax);
                }
            }
        }

        private void SpawnLayer(int index)
        {
            BgLayer layer = Layers[index];
            Camera cam = Camera.main;
            if (cam == null) return;

            float halfHeight = cam.orthographicSize;
            float halfWidth = halfHeight * cam.aspect;
            float buf = GameConstants.ScreenBuffer;

            // 从游戏屏幕右边缘外侧生成，Y轴随机
            float spawnX = halfWidth * (1f + buf);
            float spawnY = Random.Range(-halfHeight, halfHeight);
            Vector3 spawnPos = new Vector3(spawnX, spawnY, layer.ZPos);

            GameObject bgObj = PrefabFactory.CreateBackground(
                spawnPos, layer.Character, layer.FontSize, layer.Color, layer.SortingOrder);
            bgObj.GetComponent<UIBackground>().Initialize(layer.Speed);
        }
    }
}
