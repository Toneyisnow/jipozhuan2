using UnityEngine;
using TMPro;
using JiPoZhuan.UIObjects;

namespace JiPoZhuan.Managers
{
    /// <summary>
    /// Prefab工厂 - 纯代码创建所有游戏对象，无需拖拽Prefab
    /// </summary>
    public static class PrefabFactory
    {
        private static TMP_FontAsset cachedFont;
        private static Material bitmapMaterial;

        /// <summary>
        /// 可选：设置自定义中文字体
        /// </summary>
        public static void SetFont(TMP_FontAsset font)
        {
            cachedFont = font;

            // 创建使用 TextMeshPro/Bitmap shader 的材质（无背景）
            Shader bitmapShader = Shader.Find("TextMeshPro/Bitmap");
            if (bitmapShader != null)
            {
                bitmapMaterial = new Material(bitmapShader);
                bitmapMaterial.SetTexture("_MainTex", font.atlasTexture);
            }
        }

        /// <summary>
        /// 创建英雄对象
        /// </summary>
        public static GameObject CreateHero(Vector3 position)
        {
            GameObject hero = CreateTextObject("Hero", "击", 10f, Color.cyan, position);
            hero.AddComponent<UIHero>();
            hero.tag = "Player";

            SetupPhysics(hero);
            return hero;
        }

        /// <summary>
        /// 创建英雄子弹
        /// </summary>
        public static GameObject CreateHeroBullet(Vector3 position)
        {
            GameObject bullet = CreateTextObject("HeroBullet", "弹", 6f, Color.yellow, position);
            bullet.AddComponent<UIHeroBullet>();

            SetupPhysics(bullet);
            return bullet;
        }

        /// <summary>
        /// 创建敌人对象
        /// </summary>
        public static GameObject CreateEnemy(Vector3 position)
        {
            GameObject enemy = CreateTextObject("Enemy", "", 8f, Color.red, position);
            enemy.AddComponent<UIEnemy>();

            SetupPhysics(enemy);
            return enemy;
        }

        /// <summary>
        /// 创建敌人子弹
        /// </summary>
        public static GameObject CreateEnemyBullet(Vector3 position)
        {
            GameObject bullet = CreateTextObject("EnemyBullet", "", 5f, new Color(1f, 0.5f, 0f), position);
            bullet.AddComponent<UIEnemyBullet>();

            SetupPhysics(bullet);
            return bullet;
        }

        /// <summary>
        /// 创建背景元素（无物理组件，TMP直接挂在根对象上以支持alpha透明）
        /// </summary>
        public static GameObject CreateBackground(Vector3 position, string character, float fontSize, Color color, int sortingOrder)
        {
            GameObject obj = new GameObject("BG_" + character);
            obj.transform.position = position;

            TextMeshPro tmp = obj.AddComponent<TextMeshPro>();
            tmp.text = character;
            tmp.fontSize = fontSize;
            tmp.alignment = TextAlignmentOptions.Center;
            tmp.color = color;
            tmp.enableWordWrapping = false;
            tmp.overflowMode = TextOverflowModes.Overflow;
            tmp.sortingOrder = sortingOrder;

            // 使用自定义字体；保留默认SDF材质使alpha透明正常工作
            if (cachedFont != null)
                tmp.font = cachedFont;

            // 使用 Bitmap 材质，去除背景色块
            if (bitmapMaterial != null)
            {
                tmp.fontMaterial = bitmapMaterial;
            }

            tmp.ForceMeshUpdate();
            obj.AddComponent<UIBackground>();
            return obj;
        }

        /// <summary>
        /// 创建幕布字符（"云"，UIBackdrop循环滚动，无物理组件）
        /// </summary>
        public static GameObject CreateBackdropChar(Vector3 position, float fontSize, Color color, int sortingOrder)
        {
            GameObject obj = new GameObject("Backdrop_云");
            obj.transform.position = position;

            TextMeshPro tmp = obj.AddComponent<TextMeshPro>();
            tmp.text = "云";
            tmp.fontSize = fontSize;
            tmp.alignment = TextAlignmentOptions.Center;
            tmp.color = color;
            tmp.enableWordWrapping = false;
            tmp.overflowMode = TextOverflowModes.Overflow;
            tmp.sortingOrder = sortingOrder;

            if (cachedFont != null)
                tmp.font = cachedFont;
            if (bitmapMaterial != null)
                tmp.fontMaterial = bitmapMaterial;

            tmp.ForceMeshUpdate();
            obj.AddComponent<UIBackdrop>();
            return obj;
        }

        /// <summary>
        /// 创建带TextMeshPro的基础游戏对象
        /// </summary>
        private static GameObject CreateTextObject(string name, string text, float fontSize, Color color, Vector3 position)
        {
            GameObject obj = new GameObject(name);
            obj.transform.position = position;

            // 添加BoxCollider（BaseUIObject要求）
            BoxCollider col = obj.AddComponent<BoxCollider>();
            col.isTrigger = true;

            // 创建TextMeshPro子对象
            GameObject textChild = new GameObject("Text");
            textChild.transform.SetParent(obj.transform);
            textChild.transform.localPosition = Vector3.zero;

            TextMeshPro tmp = textChild.AddComponent<TextMeshPro>();
            tmp.text = text;
            tmp.fontSize = fontSize;
            tmp.alignment = TextAlignmentOptions.Center;
            tmp.color = color;
            tmp.enableWordWrapping = false;
            tmp.overflowMode = TextOverflowModes.Overflow;
            tmp.sortingOrder = 1;

            if (cachedFont != null)
                tmp.font = cachedFont;

            // 使用 Bitmap 材质，去除背景色块
            if (bitmapMaterial != null)
            {
                tmp.fontMaterial = bitmapMaterial;
            }

            // 强制更新后收紧RectTransform到文字实际大小，消除背景色块
            tmp.ForceMeshUpdate();
            tmp.rectTransform.sizeDelta = tmp.GetPreferredValues(text);

            return obj;
        }

        private static void SetupPhysics(GameObject obj)
        {
            // Rigidbody is auto-added by BaseUIObject's RequireComponent
            // Just ensure settings are correct
            Rigidbody rb = obj.GetComponent<Rigidbody>();
            if (rb == null)
                rb = obj.AddComponent<Rigidbody>();
            rb.useGravity = false;
            rb.isKinematic = true;
        }
    }
}
