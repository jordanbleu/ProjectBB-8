namespace Assets.Source.Constants
{
    /// <summary>
    /// Hides the magic strings for referencing prefabs
    /// </summary>
    public static class GameObjects
    {
        public static class Actors
        {
            public const string KamikazeEnemy_DO_NOT_USE = "KamikazeEnemy";
            public const string ShooterEnemy_DO_NOT_USE = "ShooterEnemy";
            public const string Shooter = "Shooter_AI";
            public const string Kamikaze = "Kamikaze_AI";
            public const string Player = "Player";
        }

        public static class Projectiles
        {
            public const string PlayerBullet = "PlayerBullet";
            public const string EnemyBullet = "EnemyBullet";
            public const string Asteroid = "Asteroid";

        }

        public static class Formations
        {
            public const string FlyingV = "FlyingVFormation";
        }

        public const string SystemObject = "SystemObject";
        public const string Canvas = "Canvas";
        public const string TextWriter = "TextWriter";
        public const string TextWriterPipeline = "TextWriterPipeline";
    }
}
