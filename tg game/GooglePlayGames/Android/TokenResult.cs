using Com.Google.Android.Gms.Common.Api;
using Google.Developers;
using System;

namespace GooglePlayGames.Android
{
	internal class TokenResult : JavaObjWrapper, Result
	{
		public TokenResult(IntPtr ptr)
			: base(ptr)
		{
		}

		public Status getStatus()
		{
			return new Status(InvokeCall<IntPtr>("getStatus", "()Lcom/google/android/gms/common/api/Status;", Array.Empty<object>()));
		}

		public int getStatusCode()
		{
			return InvokeCall<int>("getStatusCode", "()I", Array.Empty<object>());
		}

		public string getAuthCode()
		{
			return InvokeCall<string>("getAuthCode", "()Ljava/lang/String;", Array.Empty<object>());
		}

		public string getEmail()
		{
			return InvokeCall<string>("getEmail", "()Ljava/lang/String;", Array.Empty<object>());
		}

		public string getIdToken()
		{
			return InvokeCall<string>("getIdToken", "()Ljava/lang/String;", Array.Empty<object>());
		}
	}
}
