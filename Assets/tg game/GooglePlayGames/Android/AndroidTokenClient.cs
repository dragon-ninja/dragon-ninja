using Com.Google.Android.Gms.Common.Api;
using GooglePlayGames.OurUtils;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace GooglePlayGames.Android
{
	internal class AndroidTokenClient : TokenClient
	{
		private const string TokenFragmentClass = "com.google.games.bridge.TokenFragment";

		private const string FetchTokenSignature = "(Landroid/app/Activity;ZZZZLjava/lang/String;Z[Ljava/lang/String;ZLjava/lang/String;)Lcom/google/android/gms/common/api/PendingResult;";

		private const string FetchTokenMethod = "fetchToken";

		private const string GetAnotherAuthCodeMethod = "getAnotherAuthCode";

		private const string GetAnotherAuthCodeSignature = "(Landroid/app/Activity;ZLjava/lang/String;)Lcom/google/android/gms/common/api/PendingResult;";

		private bool requestEmail;

		private bool requestAuthCode;

		private bool requestIdToken;

		private List<string> oauthScopes;

		private string webClientId;

		private bool forceRefresh;

		private bool hidePopups;

		private string accountName;

		private string email;

		private string authCode;

		private string idToken;

		public static IntPtr CreateInvisibleView()
		{
			object[] args = new object[1];
			jvalue[] array = AndroidJNIHelper.CreateJNIArgArray(args);
			try
			{
				using (AndroidJavaClass androidJavaClass = new AndroidJavaClass("com.google.games.bridge.TokenFragment"))
				{
					using (AndroidJavaObject androidJavaObject = GetActivity())
					{
						IntPtr staticMethodID = AndroidJNI.GetStaticMethodID(androidJavaClass.GetRawClass(), "createInvisibleView", "(Landroid/app/Activity;)Landroid/view/View;");
						array[0].l = androidJavaObject.GetRawObject();
						return AndroidJNI.CallStaticObjectMethod(androidJavaClass.GetRawClass(), staticMethodID, array);
					}
				}
			}
			catch (Exception ex)
			{
				GooglePlayGames.OurUtils.Logger.e("Exception creating invisible view: " + ex.Message);
				GooglePlayGames.OurUtils.Logger.e(ex.ToString());
			}
			finally
			{
				AndroidJNIHelper.DeleteJNIArgArray(args, array);
			}
			return IntPtr.Zero;
		}

		public static AndroidJavaObject GetActivity()
		{
			using (AndroidJavaClass androidJavaClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
			{
				return androidJavaClass.GetStatic<AndroidJavaObject>("currentActivity");
			}
		}

		public void SetRequestAuthCode(bool flag, bool forceRefresh)
		{
			requestAuthCode = flag;
			this.forceRefresh = forceRefresh;
		}

		public void SetRequestEmail(bool flag)
		{
			requestEmail = flag;
		}

		public void SetRequestIdToken(bool flag)
		{
			requestIdToken = flag;
		}

		public void SetWebClientId(string webClientId)
		{
			this.webClientId = webClientId;
		}

		public void SetHidePopups(bool flag)
		{
			hidePopups = flag;
		}

		public void SetAccountName(string accountName)
		{
			this.accountName = accountName;
		}

		public void AddOauthScopes(params string[] scopes)
		{
			if (scopes != null)
			{
				if (oauthScopes == null)
				{
					oauthScopes = new List<string>();
				}
				oauthScopes.AddRange(scopes);
			}
		}

		public void Signout()
		{
			authCode = null;
			email = null;
			idToken = null;
			PlayGamesHelperObject.RunOnGameThread(delegate
			{
				UnityEngine.Debug.Log("Calling Signout in token client");
				new AndroidJavaClass("com.google.games.bridge.TokenFragment").CallStatic("signOut", GetActivity());
			});
		}

		public void FetchTokens(bool silent, Action<int> callback)
		{
			PlayGamesHelperObject.RunOnGameThread(delegate
			{
				DoFetchToken(silent, callback);
			});
		}

		internal void DoFetchToken(bool silent, Action<int> callback)
		{
			object[] args = new object[10];
			jvalue[] array = AndroidJNIHelper.CreateJNIArgArray(args);
			try
			{
				using (AndroidJavaClass androidJavaClass = new AndroidJavaClass("com.google.games.bridge.TokenFragment"))
				{
					using (AndroidJavaObject androidJavaObject = GetActivity())
					{
						IntPtr staticMethodID = AndroidJNI.GetStaticMethodID(androidJavaClass.GetRawClass(), "fetchToken", "(Landroid/app/Activity;ZZZZLjava/lang/String;Z[Ljava/lang/String;ZLjava/lang/String;)Lcom/google/android/gms/common/api/PendingResult;");
						array[0].l = androidJavaObject.GetRawObject();
						array[1].z = silent;
						array[2].z = requestAuthCode;
						array[3].z = requestEmail;
						array[4].z = requestIdToken;
						array[5].l = AndroidJNI.NewStringUTF(webClientId);
						array[6].z = forceRefresh;
						array[7].l = AndroidJNIHelper.ConvertToJNIArray(oauthScopes.ToArray());
						array[8].z = hidePopups;
						array[9].l = AndroidJNI.NewStringUTF(accountName);
						new PendingResult<TokenResult>(AndroidJNI.CallStaticObjectMethod(androidJavaClass.GetRawClass(), staticMethodID, array)).setResultCallback(new TokenResultCallback(delegate(int rc, string authCode, string email, string idToken)
						{
							this.authCode = authCode;
							this.email = email;
							this.idToken = idToken;
							callback(rc);
						}));
					}
				}
			}
			catch (Exception ex)
			{
				GooglePlayGames.OurUtils.Logger.e("Exception launching token request: " + ex.Message);
				GooglePlayGames.OurUtils.Logger.e(ex.ToString());
			}
			finally
			{
				AndroidJNIHelper.DeleteJNIArgArray(args, array);
			}
		}

		public string GetEmail()
		{
			return email;
		}

		public string GetAuthCode()
		{
			return authCode;
		}

		public void GetAnotherServerAuthCode(bool reAuthenticateIfNeeded, Action<string> callback)
		{
			object[] args = new object[3];
			jvalue[] array = AndroidJNIHelper.CreateJNIArgArray(args);
			try
			{
				using (AndroidJavaClass androidJavaClass = new AndroidJavaClass("com.google.games.bridge.TokenFragment"))
				{
					using (AndroidJavaObject androidJavaObject = GetActivity())
					{
						IntPtr staticMethodID = AndroidJNI.GetStaticMethodID(androidJavaClass.GetRawClass(), "getAnotherAuthCode", "(Landroid/app/Activity;ZLjava/lang/String;)Lcom/google/android/gms/common/api/PendingResult;");
						array[0].l = androidJavaObject.GetRawObject();
						array[1].z = reAuthenticateIfNeeded;
						array[2].l = AndroidJNI.NewStringUTF(webClientId);
						new PendingResult<TokenResult>(AndroidJNI.CallStaticObjectMethod(androidJavaClass.GetRawClass(), staticMethodID, array)).setResultCallback(new TokenResultCallback(delegate(int rc, string authCode, string email, string idToken)
						{
							this.authCode = authCode;
							callback(authCode);
						}));
					}
				}
			}
			catch (Exception ex)
			{
				GooglePlayGames.OurUtils.Logger.e("Exception launching auth code request: " + ex.Message);
				GooglePlayGames.OurUtils.Logger.e(ex.ToString());
			}
			finally
			{
				AndroidJNIHelper.DeleteJNIArgArray(args, array);
			}
		}

		public string GetIdToken()
		{
			return idToken;
		}
	}
}
