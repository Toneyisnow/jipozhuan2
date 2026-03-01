namespace JiPoZhuan.Models
{
    /// <summary>
    /// 游戏对象基类
    /// </summary>
    public abstract class BaseObject
    {
        public int Id;
        public Sinograph DisplaySinograph;

        protected BaseObject(int id, Sinograph sinograph)
        {
            Id = id;
            DisplaySinograph = sinograph;
        }
    }
}
