using System;
using JiPoZhuan.Models;

namespace JiPoZhuan.Models.Definitions
{
    /// <summary>
    /// 敌人子弹定义 - 纯数据类
    /// </summary>
    [Serializable]
    public class EnemyBulletDefinition
    {
        public string Id;
        public Sinograph DisplaySinograph;
        public int Damage;
        public TrajectoryDefinition Trajectory;

        public EnemyBulletDefinition(
            string id,
            string character,
            int damage,
            TrajectoryDefinition trajectory)
        {
            Id = id;
            DisplaySinograph = new Sinograph(character);
            Damage = damage;
            Trajectory = trajectory;
        }
    }
}
