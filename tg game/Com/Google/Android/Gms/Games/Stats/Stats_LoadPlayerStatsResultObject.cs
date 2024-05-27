using Com.Google.Android.Gms.Common.Api;
using Google.Developers;
using System;

namespace Com.Google.Android.Gms.Games.Stats
{
	public class Stats_LoadPlayerStatsResultObject : JavaObjWrapper, Stats_LoadPlayerStatsResult, Result
	{
		private const string CLASS_NAME = "com/google/android/gms/games/stats/Stats$LoadPlayerStatsResult";

		public Stats_LoadPlayerStatsResultObject(IntPtr ptr)
			: base(ptr)
		{
		}

		public PlayerStats getPlayerStats()
		{
			return new PlayerStatsObject(InvokeCall<IntPtr>("getPlayerStats", "()Lcom/google/android/gms/games/stats/PlayerStats;", Array.Empty<object>()));
		}

		public Status getStatus()
		{
			return new Status(InvokeCall<IntPtr>("getStatus", "()Lcom/google/android/gms/common/api/Status;", Array.Empty<object>()));
		}
	}
}
