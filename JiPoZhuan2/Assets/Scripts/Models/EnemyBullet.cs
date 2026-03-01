using JiPoZhuan.Models.Definitions;

namespace JiPoZhuan.Models
{
    /// <summary>
    /// 敌人子弹数据
    /// </summary>
    public class EnemyBullet : BaseObject
    {
        public EnemyBulletDefinition Definition;

        public EnemyBullet(int id, EnemyBulletDefinition definition)
            : base(id, definition.DisplaySinograph)
        {
            Definition = definition;
        }

        public int Damage => Definition.Damage;
    }
}
