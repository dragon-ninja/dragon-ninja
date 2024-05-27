using System;

namespace GooglePlayGames.BasicApi.Multiplayer
{
	public class Participant : IComparable<Participant>
	{
		public enum ParticipantStatus
		{
			NotInvitedYet,
			Invited,
			Joined,
			Declined,
			Left,
			Finished,
			Unresponsive,
			Unknown
		}

		private string mDisplayName = string.Empty;

		private readonly string mParticipantId = string.Empty;

		private ParticipantStatus mStatus = ParticipantStatus.Unknown;

		private Player mPlayer;

		private bool mIsConnectedToRoom;

		public string DisplayName => mDisplayName;

		public string ParticipantId => mParticipantId;

		public ParticipantStatus Status => mStatus;

		public Player Player => mPlayer;

		public bool IsConnectedToRoom => mIsConnectedToRoom;

		public bool IsAutomatch => mPlayer == null;

		internal Participant(string displayName, string participantId, ParticipantStatus status, Player player, bool connectedToRoom)
		{
			mDisplayName = displayName;
			mParticipantId = participantId;
			mStatus = status;
			mPlayer = player;
			mIsConnectedToRoom = connectedToRoom;
		}

		public override string ToString()
		{
			return string.Format("[Participant: '{0}' (id {1}), status={2}, player={3}, connected={4}]", mDisplayName, mParticipantId, mStatus.ToString(), (mPlayer == null) ? "NULL" : mPlayer.ToString(), mIsConnectedToRoom);
		}

		public int CompareTo(Participant other)
		{
			return string.Compare(mParticipantId, other.mParticipantId, StringComparison.Ordinal);
		}

		public override bool Equals(object obj)
		{
			if (obj == null)
			{
				return false;
			}
			if (this == obj)
			{
				return true;
			}
			if (obj.GetType() != typeof(Participant))
			{
				return false;
			}
			Participant participant = (Participant)obj;
			return mParticipantId.Equals(participant.mParticipantId);
		}

		public override int GetHashCode()
		{
			if (mParticipantId == null)
			{
				return 0;
			}
			return mParticipantId.GetHashCode();
		}
	}
}
