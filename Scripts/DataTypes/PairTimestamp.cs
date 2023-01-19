using System;


namespace TrollSmasher.DataTypes
{
    public class PairTimestamp<T>
    {
        public DateTime time { get; set;}
        public T pairX { get; set;}

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
