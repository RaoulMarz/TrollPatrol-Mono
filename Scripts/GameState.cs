using System;
using TrollSmasher.Enums;

namespace TrollSmasher
{
    public static class GameState
    {
        public static int PlayerHealth { get; set; }
        public static int CrawliesCollected { get; set; }
        public static GameStatus AppGameStatus { get; set; } = GameStatus.GAME_STATUS_NONE;
        public static MovementEntity movementEntity { get; set; } = MovementEntity.MOVEMENT_ENTITY_NONE;
    }
}
