namespace JiPoZhuan.Models
{
    /// <summary>
    /// 英雄子弹数据
    /// </summary>
    public class HeroBullet : BaseObject
    {
        public int Damage;
        public float MoveSpeed;

        public HeroBullet(int id, int damage = 1, float moveSpeed = 10f)
            : base(id, new Sinograph("弹", 1f))
        {
            Damage = damage;
            MoveSpeed = moveSpeed;
        }
    }
}
