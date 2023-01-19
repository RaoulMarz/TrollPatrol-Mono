using System;

namespace TrollSmasher.DataTypes
{
	public enum DebugCategoryEnum
	{
		DEBUG_CAT_NONE,
		DEBUG_CAT_ALL,
		DEBUG_CAT_GAME_MOVEMENT,
		DEBUG_CAT_GAME_INPUT,
		DEBUG_CAT_GAME_ALL
	}

	public static class DebugCategory
	{
		static private DebugCategoryEnum appdebugLevel = DebugCategoryEnum.DEBUG_CAT_ALL;

		public static void SetCategory(DebugCategoryEnum debugLevel)
		{
			appdebugLevel = debugLevel;
		}

		static bool ShowDebug(DebugCategoryEnum clientCat)
		{
			bool res = false;
			if (appdebugLevel == DebugCategoryEnum.DEBUG_CAT_ALL)
				res = true;
			else
			{
				if (appdebugLevel == DebugCategoryEnum.DEBUG_CAT_GAME_ALL)
				{

				}
			}
			return res;
		}
	}
}
