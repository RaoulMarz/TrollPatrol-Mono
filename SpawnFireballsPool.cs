using System;
using System.Collections.Generic;
using TrollPatrolMono.DataTypes;
using Godot;

namespace TrollPatrolMono
{
	public class SpawnFireballsPool
	{
		private int _allocate;
		private bool _recycle;
		private Dictionary<string, PairTimestamp<Fireball>> fireballPoolMap = new Dictionary<string, PairTimestamp<Fireball>>();
		private Dictionary<string, bool> usedSlotMap = new Dictionary<string, bool>();
		const string fireballResource = "res://Fireball.tscn";

		private static Fireball CreateFireball()
		{
			PackedScene fireballScene = (PackedScene)ResourceLoader.Load(fireballResource);
			GD.Print("CreateFireball() started");
			//Diagnostics.PrintNullValueMessage(fireballScene, "fireballScene");
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
				fireballPoolMap.Add(id.ToString(), new PairTimestamp<Fireball>(CreateFireball()) );
			}
		}

		public KeyValuePair<bool, Fireball> SpawnFireball()
		{
			KeyValuePair<bool, Fireball> res = new KeyValuePair<bool, Fireball>(false, null);
			if ( (fireballPoolMap != null) && (fireballPoolMap.Count > 0) )
			{
				//Choose an unused entry in the Map and return that
				foreach (string xkey in fireballPoolMap.Keys)
				{

				}
			}
			return res;
		}
	}
}
