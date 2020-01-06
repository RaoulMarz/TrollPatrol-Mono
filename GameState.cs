using System;
using TrollPatrolMono.Enums;

namespace TrollPatrolMono
{
    public static class GameState
    {
        public static int PlayerHealth { get; set; }
        public static int CrawliesCollected { get; set; }
        public static GameStatus AppGameStatus { get; set; } = GameStatus.GAME_STATUS_NONE;
    }
}
