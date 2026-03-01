using JiPoZhuan.Models.Definitions;

namespace JiPoZhuan.Models
{
    /// <summary>
    /// 敌人数据
    /// </summary>
    public class Enemy : BaseObject
    {
        public EnemyDefinition Definition;
        public int HpCurrent;

        public Enemy(int id, EnemyDefinition definition)
            : base(id, definition.DisplaySinograph)
        {
            Definition = definition;
            HpCurrent = definition.HpMax;
        }

        public bool IsAlive => HpCurrent > 0;

        public void TakeDamage(int damage)
        {
            HpCurrent -= damage;
            if (HpCurrent < 0)
                HpCurrent = 0;
        }
    }
}
