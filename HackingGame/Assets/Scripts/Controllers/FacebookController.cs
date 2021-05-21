using System.Collections;
using System.Collections.Generic;
using Facebook.Unity;
using UnityEngine;

public class FacebookController : MonoBehaviour
{
    public static string FACEBOOK_USER_ID;
    public static FacebookController instance;
    void Awake()
    {
        instance = this;
        if (!FB.IsInitialized)
        {
            FB.Init(InitCallback, OnHideUnity);
        }
        else
        {
            FB.ActivateApp();
        }
    }

    private void InitCallback()
    {
        if (FB.IsInitialized)
        {
            FB.ActivateApp();
        }
        else
        {
            Debug.Log("FACEBOOK FACEBOOK Failed to Initialize the Facebook SDK");
        }
    }

    private void OnHideUnity(bool isGameShown)
    {
        if (!isGameShown)
        {
            Time.timeScale = 0;
        }
        else
        {
            Time.timeScale = 1;
        }
    }

    public void FBLogin()
    {
        if (FB.IsInitialized)
        {
            List<string> perms = new List<string>() { "public_profile", "email" };
            FB.LogInWithReadPermissions(perms, AuthCallback);
        }
    }
    public static StringParametarEvent facebookLogedIn = new StringParametarEvent();
    private void AuthCallback(ILoginResult result)
    {
        if (FB.IsLoggedIn)
        {
            var aToken = Facebook.Unity.AccessToken.CurrentAccessToken;
            FACEBOOK_USER_ID = aToken.UserId;
            FB.API("/me?fields=first_name", Facebook.Unity.HttpMethod.GET, LoginCallback2);
            RewardsManager.instance.RewardUser("facbook_login");
        }
        else
        {
            Debug.Log("FACEBOOK User cancelled login");
        }

    }
    void LoginCallback2(IResult result)
    {
        if (result.Error == null && FB.IsLoggedIn)
        {
            Debug.Log(result.RawResult);
            IDictionary dict = Facebook.MiniJSON.Json.Deserialize(result.RawResult) as IDictionary;
            string fbname = dict["first_name"].ToString();
            facebookLogedIn.Invoke(fbname);
        }
    }
}