using System;
using System.Collections.Generic;
using TrollSmasher.DataTypes;
using Godot;

namespace TrollSmasher
{
	public class SpawnFireballsPool
	{
		private int _allocate;
		private bool _recycle;
		private Dictionary<string, PairTimestamp<Fireball>> fireballPoolMap = new Dictionary<string, PairTimestamp<Fireball>>();
		private Dictionary<string, bool> usedSlotMap = new Dictionary<string, bool>();
		const string fireballResource = "res://Scenes/Fireball.tscn";

		private static Fireball CreateFireball()
		{
			PackedScene fireballScene = (PackedScene)ResourceLoader.Load(fireballResource);
			GD.Print("CreateFireball() started");
			if (fireballScene != null)
			{
				return (Fireball)fireballScene.Instance();
			}
			return null;
		}

		public SpawnFireballsPool(int allocate = 100, bool recycle = true)
		{
			_allocate = allocate;
			_recycle = recycle;
			for (int idx = 0; idx < _allocate; idx++)
			{
				Guid id = Guid.NewGuid();
				var pairValue = new PairTimestamp<Fireball>(CreateFireball());
				fireballPoolMap.Add(id.ToString(), pairValue);
				GD.Print($"SpawnFireballsPool() index={idx}, value={pairValue}");
			}
		}

		public bool ItemInUse(string key)
		{
			bool res = false;
			if (usedSlotMap != null)
			{
				res = usedSlotMap.ContainsKey(key);
			}
			return res;
		}

		public List<PairTimestamp<Fireball>> SpawnFireballArray(int allocateCount)
		{
			if (allocateCount <= 0)
				return null;
			List<PairTimestamp<Fireball>> res = new List<PairTimestamp<Fireball>>();
			if ((fireballPoolMap != null) && (fireballPoolMap.Count > 0))
			{
				//Choose an unused entry in the Map and return that
				int icount = 0;
				foreach (string xkey in fireballPoolMap.Keys)
				{
					if (ItemInUse(xkey) == false)
					{
						var itemPair = fireballPoolMap[xkey];
						res.Add(itemPair);
						icount += 1;
						usedSlotMap[xkey] = true;
						if (allocateCount >= icount)
							break;
					}
				}
			}
			return res;
		}

		public PairTimestamp<Fireball> SpawnFireball()
		{
			PairTimestamp<Fireball> res = new PairTimestamp<Fireball>(null);
			if ( (fireballPoolMap != null) && (fireballPoolMap.Count > 0) )
			{
				//Choose an unused entry in the Map and return that
				foreach (string xkey in fireballPoolMap.Keys)
				{
					if (ItemInUse(xkey) == false)
					{
						var fireballItem = fireballPoolMap[xkey];
						if (fireballItem != null)
						{
							usedSlotMap[xkey] = true;
							return res;
						}
					}
				}
				//when an unused entry have been found, add this reference to usedSlotMap and return the entry
			}
			return res;
		}
	}
}
