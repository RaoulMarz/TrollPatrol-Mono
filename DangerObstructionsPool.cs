using System;
using System.Collections.Generic;
using TrollPatrolMono.DataTypes;
using Godot;

namespace TrollPatrolMono
{
    public class DangerObstructionsPool
    {
        private int _allocate;
        private bool _recycle;
        private Dictionary<string, PairTimestamp<DangerObstruction>> hazardObstructionsPoolMap = new Dictionary<string, PairTimestamp<DangerObstruction>>();
        private Dictionary<string, bool> usedSlotMap = new Dictionary<string, bool>();

        public DangerObstructionsPool(int allocate = 100, bool recycle = true)
        {
            _allocate = allocate;
            _recycle = recycle;
            for (int idx = 0; idx < _allocate; idx++)
            {
                Guid id = Guid.NewGuid();
                //hazardObstructionsPoolMap.Add(id.ToString(), new PairTimestamp<Fireball>(CreateFireball()));
            }
        }
    }
}
