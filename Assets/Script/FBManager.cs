using Facebook.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FBManager : MonoBehaviour
{

    //var perms = new List<string>() { "public_profile", "email" };
    //FB.LogInWithReadPermissions(perms, AuthCallback);

    LoginPanel LoginPanel;


    public void fbLogin() {
        var perms = new List<string>() { "public_profile", "gaming_profile", "gaming_user_picture" };// { "public_profile", "email" };
        FB.LogInWithReadPermissions(perms, AuthCallback);
    }


    private void AuthCallback(ILoginResult result)
    {
        
        if (FB.IsLoggedIn)
        {
            // AccessToken class will have session details
            var aToken = Facebook.Unity.AccessToken.CurrentAccessToken;
            // Print current access token's User ID
            Debug.Log(aToken.UserId);


            LoginPanel.login_token(aToken.UserId,aToken.TokenString);

            // Print current access token's granted permissions
            foreach (string perm in aToken.Permissions)
            {
                Debug.Log(perm);
            }
        }
        else
        {
            Debug.Log("User cancelled login");
        }
    }




    //FB.Android.RetrieveLoginStatus(LoginStatusCallback);

    private void LoginStatusCallback(ILoginStatusResult result)
    {
        if (!string.IsNullOrEmpty(result.Error))
        {
            Debug.Log("Error: " + result.Error);
        }
        else if (result.Failed)
        {
            Debug.Log("Failure: Access Token could not be retrieved");
        }
        else
        {
            // Successfully logged user in
            // A popup notification will appear that says "Logged in as <User Name>"
            Debug.Log("Success: " + result.AccessToken.UserId);
        }
    }






    // Awake function from Unity's MonoBehavior
    void Awake()
    {

        LoginPanel = GetComponent<LoginPanel>();

        if (!FB.IsInitialized)
        {
            // Initialize the Facebook SDK
            FB.Init(InitCallback, OnHideUnity);
        }
        else
        {
            // Already initialized, signal an app activation App Event
            FB.ActivateApp();
        }
    }

    private void InitCallback()
    {
        if (FB.IsInitialized)
        {
            // Signal an app activation App Event
            FB.ActivateApp();
            // Continue with Facebook SDK
            // ...
        }
        else
        {
            Debug.Log("Failed to Initialize the Facebook SDK");
        }
    }

    private void OnHideUnity(bool isGameShown)
    {
        if (!isGameShown)
        {
            // Pause the game - we will need to hide
            Time.timeScale = 0;
        }
        else
        {
            // Resume the game - we're getting focus again
            Time.timeScale = 1;
        }
    }

    public static void LogInWithReadPermissions(IEnumerable<string> permissions = null, FacebookDelegate<ILoginResult> callback = null) { 
    
    }
}