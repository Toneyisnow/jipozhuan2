namespace JiPoZhuan.Managers
{
    /// <summary>
    /// </summary>
    public static class GameConstants
    {
        public const string SceneMainMenu = "MainMenuScene";
        public const string SceneSettings = "SettingsScene";
        public const string SceneMainGame = "MainGameScene";

        // Game Screen: visible screen + 20% buffer on each side (40% larger total).
        // Viewport range: [-ScreenBuffer, 1+ScreenBuffer] on both axes.
        // World-space half-extent = visibleHalfExtent * (1 + 2 * ScreenBuffer).
        public const float ScreenBuffer = 0.2f;

        public const string TagHero = "Player";
        public const string TagEnemy = "Enemy";
        public const string TagHeroBullet = "HeroBullet";
        public const string TagEnemyBullet = "EnemyBullet";
    }
}
