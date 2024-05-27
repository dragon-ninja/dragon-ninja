using System.Collections.Generic;

namespace GooglePlayGames.BasicApi.Multiplayer
{
	public class MatchOutcome
	{
		public enum ParticipantResult
		{
			Unset = -1,
			None,
			Win,
			Loss,
			Tie
		}

		public const uint PlacementUnset = 0u;

		private List<string> mParticipantIds = new List<string>();

		private Dictionary<string, uint> mPlacements = new Dictionary<string, uint>();

		private Dictionary<string, ParticipantResult> mResults = new Dictionary<string, ParticipantResult>();

		public List<string> ParticipantIds => mParticipantIds;

		public void SetParticipantResult(string participantId, ParticipantResult result, uint placement)
		{
			if (!mParticipantIds.Contains(participantId))
			{
				mParticipantIds.Add(participantId);
			}
			mPlacements[participantId] = placement;
			mResults[participantId] = result;
		}

		public void SetParticipantResult(string participantId, ParticipantResult result)
		{
			SetParticipantResult(participantId, result, 0u);
		}

		public void SetParticipantResult(string participantId, uint placement)
		{
			SetParticipantResult(participantId, ParticipantResult.Unset, placement);
		}

		public ParticipantResult GetResultFor(string participantId)
		{
			if (!mResults.ContainsKey(participantId))
			{
				return ParticipantResult.Unset;
			}
			return mResults[participantId];
		}

		public uint GetPlacementFor(string participantId)
		{
			if (!mPlacements.ContainsKey(participantId))
			{
				return 0u;
			}
			return mPlacements[participantId];
		}

		public override string ToString()
		{
			string str = "[MatchOutcome";
			foreach (string mParticipantId in mParticipantIds)
			{
				str += $" {mParticipantId}->({GetResultFor(mParticipantId)},{GetPlacementFor(mParticipantId)})";
			}
			return str + "]";
		}
	}
}
