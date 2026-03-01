using System;

namespace JiPoZhuan.Models.Definitions
{
    /// <summary>
    /// 射击模式
    /// </summary>
    public enum FirePatternType
    {
        None,
        Single,
        Spread,
        Aimed,   // 发射时朝向Hero，之后直线飞行
    }

    /// <summary>
    /// 射击定义 - 描述敌人如何发射子弹
    /// </summary>
    [Serializable]
    public class FirePattern
    {
        public FirePatternType Type;
        public float FireIntervalMin;
        public float FireIntervalMax;
        public int BulletCount;
        public float SpreadAngle;
        public string BulletDefinitionId;

        public FirePattern(
            FirePatternType type,
            float fireIntervalMin,
            float fireIntervalMax,
            string bulletDefinitionId,
            int bulletCount = 1,
            float spreadAngle = 0f)
        {
            Type = type;
            FireIntervalMin = fireIntervalMin;
            FireIntervalMax = fireIntervalMax;
            BulletDefinitionId = bulletDefinitionId;
            BulletCount = bulletCount;
            SpreadAngle = spreadAngle;
        }

        // ---------- 常用预设 ----------

        public static FirePattern None()
        {
            return new FirePattern(FirePatternType.None, 0f, 0f, null);
        }

        public static FirePattern SingleShot(float intervalMin = 1.5f, float intervalMax = 4f, string bulletId = "default")
        {
            return new FirePattern(FirePatternType.Single, intervalMin, intervalMax, bulletId);
        }

        public static FirePattern SpreadShot(int count = 3, float angle = 30f, float intervalMin = 2f, float intervalMax = 5f, string bulletId = "default")
        {
            return new FirePattern(FirePatternType.Spread, intervalMin, intervalMax, bulletId, count, angle);
        }

        public static FirePattern AimedShot(float intervalMin = 1.5f, float intervalMax = 3f, string bulletId = "bullet_aimed")
        {
            return new FirePattern(FirePatternType.Aimed, intervalMin, intervalMax, bulletId);
        }
    }
}
