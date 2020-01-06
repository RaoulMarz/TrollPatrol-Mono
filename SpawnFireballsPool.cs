using System;
using System.Collections.Generic;
using TrollPatrolMono.DataTypes;

namespace TrollPatrolMono
{
	public class SpawnFireballsPool
	{
		private int _allocate;
		private bool _recycle;
		private Dictionary<string, PairTimestamp<Fireball>> fireballPoolMap = new Dictionary<string, PairTimestamp<Fireball>>();

		public SpawnFireballsPool(int allocate = 100, bool recycle = true)
		{
			_allocate = allocate;
			_recycle = recycle;
		}

		public KeyValuePair<bool, Fireball> SpawnFireball()
		{
			KeyValuePair<bool, Fireball> res = new KeyValuePair< bool, Fireball>(false, null);
			if ( (fireballPoolMap != null) && (fireballPoolMap.Count > 0) )
			{

			}
			return res;
		}
	}
}
