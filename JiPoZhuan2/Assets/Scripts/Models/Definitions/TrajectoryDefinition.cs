using System;
using UnityEngine;

namespace JiPoZhuan.Models.Definitions
{
    /// <summary>
    /// 轨迹类型
    /// </summary>
    public enum TrajectoryType
    {
        Linear,
        Sine,
        Cosine,
        ZigZag,
        Circular,
    }

    /// <summary>
    /// 轨迹定义 - 使用数学函数描述运动轨迹
    /// </summary>
    [Serializable]
    public class TrajectoryDefinition
    {
        public TrajectoryType Type;
        public Vector2 BaseDirection;
        public float Speed;
        public float Amplitude;
        public float Frequency;
        public float PhaseOffset;

        public TrajectoryDefinition(
            TrajectoryType type,
            Vector2 baseDirection,
            float speed,
            float amplitude = 0f,
            float frequency = 0f,
            float phaseOffset = 0f)
        {
            Type = type;
            BaseDirection = baseDirection.normalized;
            Speed = speed;
            Amplitude = amplitude;
            Frequency = frequency;
            PhaseOffset = phaseOffset;
        }

        /// <summary>
        /// 根据经过的时间计算当前位置偏移量
        /// startPosition: 初始世界坐标
        /// elapsedTime: 从出生开始经过的时间
        /// 返回: 当前世界坐标
        /// </summary>
        public Vector3 Evaluate(Vector3 startPosition, float elapsedTime)
        {
            float t = elapsedTime;
            float dist = Speed * t;

            // 沿主方向的位移
            Vector3 baseOffset = new Vector3(BaseDirection.x, BaseDirection.y, 0f) * dist;

            // 垂直于主方向的偏移（用于波形轨迹）
            Vector3 perpendicular = new Vector3(-BaseDirection.y, BaseDirection.x, 0f);
            float lateralOffset = 0f;

            switch (Type)
            {
                case TrajectoryType.Linear:
                    break;

                case TrajectoryType.Sine:
                    lateralOffset = Amplitude * Mathf.Sin(Frequency * t + PhaseOffset);
                    break;

                case TrajectoryType.Cosine:
                    lateralOffset = Amplitude * Mathf.Cos(Frequency * t + PhaseOffset);
                    break;

                case TrajectoryType.ZigZag:
                    lateralOffset = Amplitude * Mathf.PingPong(Frequency * t + PhaseOffset, 2f) - Amplitude;
                    break;

                case TrajectoryType.Circular:
                    float angle = Frequency * t + PhaseOffset;
                    baseOffset += perpendicular * (Amplitude * Mathf.Sin(angle));
                    baseOffset += new Vector3(BaseDirection.x, BaseDirection.y, 0f) * (Amplitude * (Mathf.Cos(angle) - 1f));
                    return startPosition + baseOffset;
            }

            return startPosition + baseOffset + perpendicular * lateralOffset;
        }

        // ---------- 常用预设 ----------

        public static TrajectoryDefinition StraightLeft(float speed = 3f)
        {
            return new TrajectoryDefinition(TrajectoryType.Linear, Vector2.left, speed);
        }

        public static TrajectoryDefinition StraightRight(float speed = 10f)
        {
            return new TrajectoryDefinition(TrajectoryType.Linear, Vector2.right, speed);
        }

        public static TrajectoryDefinition SineLeft(float speed = 3f, float amplitude = 2f, float frequency = 2f)
        {
            return new TrajectoryDefinition(TrajectoryType.Sine, Vector2.left, speed, amplitude, frequency);
        }

        public static TrajectoryDefinition ZigZagLeft(float speed = 3f, float amplitude = 1.5f, float frequency = 3f)
        {
            return new TrajectoryDefinition(TrajectoryType.ZigZag, Vector2.left, speed, amplitude, frequency);
        }

        public static TrajectoryDefinition AngledLeft(float angleDegrees, float speed = 3f)
        {
            float rad = angleDegrees * Mathf.Deg2Rad;
            Vector2 dir = new Vector2(-Mathf.Cos(rad), Mathf.Sin(rad));
            return new TrajectoryDefinition(TrajectoryType.Linear, dir, speed);
        }
    }
}
