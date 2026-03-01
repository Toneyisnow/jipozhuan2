using System;

namespace JiPoZhuan.Models
{
    /// <summary>
    /// 汉字信息 - 用于描述游戏中显示的文字属性
    /// </summary>
    [Serializable]
    public class Sinograph
    {
        public string Character;
        public float Weight;
        public string Font;

        public Sinograph(string character, float weight = 1f, string font = null)
        {
            Character = character;
            Weight = weight;
            Font = font;
        }
    }
}
