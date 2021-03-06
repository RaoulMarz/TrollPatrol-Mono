﻿using System;


namespace TrollPatrolMono.DataTypes
{
    public class PairTimestamp<T>
    {
        DateTime time;
        T pairX;

        public PairTimestamp(DateTime aTime, T attachedPair)
        {
            time = aTime;
            pairX = attachedPair;
        }

        public PairTimestamp(T attachedPair)
        {
            time = DateTime.Now;
            pairX = attachedPair;
        }
    }
}
