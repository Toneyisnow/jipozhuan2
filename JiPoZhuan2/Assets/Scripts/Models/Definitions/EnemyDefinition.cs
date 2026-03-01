using System;
using JiPoZhuan.Models;

namespace JiPoZhuan.Models.Definitions
{
    /// <summary>
    /// 敌人定义 - 纯数据类，包含敌人的所有属性
    /// </summary>
    [Serializable]
    public class EnemyDefinition
    {
        public string Id;
        public string EnemyName;
        public Sinograph DisplaySinograph;
        public int HpMax;
        public int ScoreValue;
        public TrajectoryDefinition Trajectory;
        public FirePattern FirePattern;

        public EnemyDefinition(
            string id,
            string enemyName,
            string character,
            int hpMax,
            int scoreValue,
            TrajectoryDefinition trajectory,
            FirePattern firePattern)
        {
            Id = id;
            EnemyName = enemyName;
            DisplaySinograph = new Sinograph(character);
            HpMax = hpMax;
            ScoreValue = scoreValue;
            Trajectory = trajectory;
            FirePattern = firePattern;
        }
    }
}
