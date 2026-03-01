namespace JiPoZhuan.Models
{
    /// <summary>
    /// 英雄（玩家飞机）数据
    /// </summary>
    public class Hero : BaseObject
    {
        public int HpMax;
        public int HpCurrent;

        public Hero(int id, int hpMax)
            : base(id, new Sinograph("击", 1.5f))
        {
            HpMax = hpMax;
            HpCurrent = hpMax;
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
