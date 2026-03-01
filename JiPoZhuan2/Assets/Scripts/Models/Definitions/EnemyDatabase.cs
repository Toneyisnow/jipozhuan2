using System.Collections.Generic;
using UnityEngine;
using JiPoZhuan.Models;

namespace JiPoZhuan.Models.Definitions
{
    /// <summary>
    /// 敌人数据库 - 所有敌人和子弹定义的静态注册中心
    /// 所有定义全部在代码中预先创建，无需拖拽
    /// </summary>
    public static class EnemyDatabase
    {
        private static Dictionary<string, EnemyDefinition> enemyDefinitions;
        private static Dictionary<string, EnemyBulletDefinition> bulletDefinitions;

        private static bool initialized = false;

        public static void Initialize()
        {
            if (initialized) return;

            bulletDefinitions = new Dictionary<string, EnemyBulletDefinition>();
            enemyDefinitions = new Dictionary<string, EnemyDefinition>();

            RegisterBulletDefinitions();
            RegisterEnemyDefinitions();

            initialized = true;
            Debug.Log("[EnemyDatabase] 初始化完成: " + enemyDefinitions.Count + " 种敌人, " + bulletDefinitions.Count + " 种子弹");
        }

        // ============================================================
        //  子弹定义注册
        // ============================================================
        private static void RegisterBulletDefinitions()
        {
            // 普通子弹 - 直线向左
            Register(new EnemyBulletDefinition(
                id: "bullet_default",
                character: "炮",
                damage: 1,
                trajectory: TrajectoryDefinition.StraightLeft(6f)
            ));

            // 快速子弹
            Register(new EnemyBulletDefinition(
                id: "bullet_fast",
                character: "炮",
                damage: 1,
                trajectory: TrajectoryDefinition.StraightLeft(10f)
            ));

            // 波形子弹 - 正弦轨迹
            Register(new EnemyBulletDefinition(
                id: "bullet_sine",
                character: "炮",
                damage: 1,
                trajectory: TrajectoryDefinition.SineLeft(5f, 1.5f, 4f)
            ));

            // 重型子弹
            Register(new EnemyBulletDefinition(
                id: "bullet_heavy",
                character: "弹",
                damage: 2,
                trajectory: TrajectoryDefinition.StraightLeft(4f)
            ));

            // 瞄准子弹 - 发射时朝向Hero，之后保持直线
            Register(new EnemyBulletDefinition(
                id: "bullet_aimed",
                character: "追",
                damage: 1,
                trajectory: TrajectoryDefinition.StraightLeft(7f)   // Speed only; direction set at spawn
            ));
        }

        // ============================================================
        //  敌人定义注册
        // ============================================================
        private static void RegisterEnemyDefinitions()
        {
            // --- 普通敌人：直线飞行，随机射击 ---
            Register(new EnemyDefinition(
                id: "enemy_basic",
                enemyName: "小兵",
                character: "敌",
                hpMax: 1,
                scoreValue: 100,
                trajectory: TrajectoryDefinition.StraightLeft(3f),
                firePattern: FirePattern.SingleShot(2f, 4f, "bullet_default")
            ));

            // --- 正弦轨迹敌人 ---
            Register(new EnemyDefinition(
                id: "enemy_sine",
                enemyName: "波兵",
                character: "波",
                hpMax: 2,
                scoreValue: 200,
                trajectory: TrajectoryDefinition.SineLeft(2.5f, 2f, 2f),
                firePattern: FirePattern.SingleShot(1.5f, 3.5f, "bullet_default")
            ));

            // --- 锯齿轨迹敌人 ---
            Register(new EnemyDefinition(
                id: "enemy_zigzag",
                enemyName: "折兵",
                character: "折",
                hpMax: 2,
                scoreValue: 200,
                trajectory: TrajectoryDefinition.ZigZagLeft(2.5f, 2f, 3f),
                firePattern: FirePattern.SingleShot(2f, 4f, "bullet_fast")
            ));

            // --- 快速冲锋敌人（不发射子弹） ---
            Register(new EnemyDefinition(
                id: "enemy_rusher",
                enemyName: "冲兵",
                character: "冲",
                hpMax: 1,
                scoreValue: 150,
                trajectory: TrajectoryDefinition.StraightLeft(6f),
                firePattern: FirePattern.None()
            ));

            // --- 精英敌人：慢速但高HP，扇形射击 ---
            Register(new EnemyDefinition(
                id: "enemy_elite",
                enemyName: "将",
                character: "将",
                hpMax: 5,
                scoreValue: 500,
                trajectory: TrajectoryDefinition.StraightLeft(1.5f),
                firePattern: FirePattern.SpreadShot(3, 30f, 2.5f, 4f, "bullet_default")
            ));

            // --- 狙击手：慢速移动，发射瞄准子弹 ---
            Register(new EnemyDefinition(
                id: "enemy_sniper",
                enemyName: "狙",
                character: "狙",
                hpMax: 2,
                scoreValue: 300,
                trajectory: TrajectoryDefinition.StraightLeft(1f),
                firePattern: FirePattern.AimedShot(1.5f, 2.5f, "bullet_aimed")
            ));

            // --- 斜角飞入 ---
            Register(new EnemyDefinition(
                id: "enemy_angled_up",
                enemyName: "斜兵上",
                character: "敌",
                hpMax: 1,
                scoreValue: 100,
                trajectory: TrajectoryDefinition.AngledLeft(20f, 3.5f),
                firePattern: FirePattern.SingleShot(2f, 5f, "bullet_default")
            ));

            Register(new EnemyDefinition(
                id: "enemy_angled_down",
                enemyName: "斜兵下",
                character: "敌",
                hpMax: 1,
                scoreValue: 100,
                trajectory: TrajectoryDefinition.AngledLeft(-20f, 3.5f),
                firePattern: FirePattern.SingleShot(2f, 5f, "bullet_default")
            ));
        }

        // ============================================================
        //  注册 / 查询
        // ============================================================
        private static void Register(EnemyDefinition def)
        {
            enemyDefinitions[def.Id] = def;
        }

        private static void Register(EnemyBulletDefinition def)
        {
            bulletDefinitions[def.Id] = def;
        }

        public static EnemyDefinition GetEnemy(string id)
        {
            Initialize();
            if (enemyDefinitions.TryGetValue(id, out var def))
                return def;

            Debug.LogWarning("[EnemyDatabase] 未找到敌人定义: " + id);
            return null;
        }

        public static EnemyBulletDefinition GetBullet(string id)
        {
            Initialize();
            if (bulletDefinitions.TryGetValue(id, out var def))
                return def;

            Debug.LogWarning("[EnemyDatabase] 未找到子弹定义: " + id);
            return null;
        }

        public static IEnumerable<EnemyDefinition> GetAllEnemies()
        {
            Initialize();
            return enemyDefinitions.Values;
        }

        public static IEnumerable<EnemyBulletDefinition> GetAllBullets()
        {
            Initialize();
            return bulletDefinitions.Values;
        }

        /// <summary>
        /// 获取可用于某关卡的敌人ID列表
        /// </summary>
        public static string[] GetEnemyIdsForLevel(int level)
        {
            Initialize();

            switch (level)
            {
                case 1:
                    return new[] { "enemy_basic" };
                case 2:
                    return new[] { "enemy_basic", "enemy_angled_up", "enemy_angled_down" };
                case 3:
                    return new[] { "enemy_basic", "enemy_sine", "enemy_rusher" };
                case 4:
                    return new[] { "enemy_basic", "enemy_sine", "enemy_zigzag", "enemy_rusher" };
                case 5:
                    return new[] { "enemy_basic", "enemy_sine", "enemy_zigzag", "enemy_rusher", "enemy_elite", "enemy_sniper" };
                case 6:
                    return new[] { "enemy_sine", "enemy_zigzag", "enemy_rusher", "enemy_elite", "enemy_angled_up", "enemy_angled_down", "enemy_sniper" };
                default:
                    return new[] { "enemy_basic" };
            }
        }
    }
}
