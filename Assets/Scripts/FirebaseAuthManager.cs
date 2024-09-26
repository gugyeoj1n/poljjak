using System;
using System.Collections;
using UnityEngine;
using GooglePlayGames;
using GooglePlayGames.BasicApi;
using Firebase.Auth;
using UnityEngine.iOS;
using UnityEngine.SocialPlatforms.GameCenter;
using System.Threading.Tasks;
using Firebase.Extensions;
using TMPro;
 
public class FirebaseAuthManager : MonoBehaviour
{
    private FirebaseAuth auth;
    public TMP_Text LogText;
    public TMP_Text FbText;
 
    void Start()
    {
        PlayGamesPlatform.InitializeInstance(new PlayGamesClientConfiguration.Builder()
            .RequestIdToken()
            .RequestEmail()
            .Build());
        PlayGamesPlatform.DebugLogEnabled = true;
        PlayGamesPlatform.Activate();
        // 구글 플레이 게임 활성화
 
        auth = FirebaseAuth.DefaultInstance; // Firebase 액세스
    }
 
    public void GamecenterLogin()
    {
        if (Social.localUser.authenticated == true)
        {
            LogText.text = "iOS Login Success";
        }
        else
        {
            Social.localUser.Authenticate((bool success) =>
            {
                if (success)
                {
                    LogText.text = "iOS Login Success";
                }
                else
                {
                    LogText.text = "iOS Login Failed";
                }
            });
        }
    }
 
    public void TryGoogleLogin()
    {
        if (!Social.localUser.authenticated) // 로그인 되어 있지 않다면
        {
            Social.localUser.Authenticate(success => // 로그인 시도
            {
                if (success) // 성공하면
                {
                    LogText.text = "Success";
                    StartCoroutine(TryFirebaseLogin()); // Firebase Login 시도
                }
                else // 실패하면
                {
                    LogText.text = "Fail";
                }
            });
        }
    }
 
 
    public void TryGoogleLogout()
    {
        if (Social.localUser.authenticated) // 로그인 되어 있다면
        {
            PlayGamesPlatform.Instance.SignOut(); // Google 로그아웃
            auth.SignOut(); // Firebase 로그아웃
        }
    }
 
 
    IEnumerator TryFirebaseLogin()
    {
        while (string.IsNullOrEmpty(((PlayGamesLocalUser)Social.localUser).GetIdToken()))
            yield return null;
        string idToken = ((PlayGamesLocalUser)Social.localUser).GetIdToken();
 
 
        Credential credential = GoogleAuthProvider.GetCredential(idToken, null);
        auth.SignInWithCredentialAsync(credential).ContinueWith(task => {
            if (task.IsCanceled)
            {
                FbText.text = "SignInWithCredentialAsync was canceled.";
                return;
            }
            if (task.IsFaulted)
            {
                FbText.text = "SignInWithCredentialAsync encountered an error: " + task.Exception;
                return;
            }
 
            LogText.text = "Success!";
        });
    }
    
    void LoginProcess_GameCenter()
    {
        Debug.Log("LoginProcess_GameCenter");
         SignInWithGameCenterAsync();
    }

    public Task SignInWithGameCenterAsync()
    {
        FirebaseAuth auth = FirebaseAuth.DefaultInstance;
        var credentialTask = Firebase.Auth.GameCenterAuthProvider.GetCredentialAsync();
        var continueTask = credentialTask.ContinueWithOnMainThread(task =>
        {
            if (!task.IsCompleted)
                return null;

            if (task.Exception != null)
                Debug.Log("GC Credential Task - Exception: " + task.Exception.Message);

            var credential = task.Result;

            var loginTask = auth.SignInWithCredentialAsync(credential);
            return loginTask.ContinueWithOnMainThread(HandleSignInWithUser);
        });

        return continueTask;
    }


    // Called when a sign-in without fetching profile data completes.
    void HandleSignInWithUser(Task<Firebase.Auth.FirebaseUser> task)
    {
        //EnableUI();
        if (LogTaskCompletion(task, "Sign-in"))
        {
            Debug.Log(String.Format("{0} signed in", task.Result.DisplayName));
        }
    }

    // Log the result of the specified task, returning true if the task
    // completed successfully, false otherwise.
    protected bool LogTaskCompletion(Task task, string operation)
    {
        bool complete = false;
        if (task.IsCanceled)
        {
            Debug.Log(operation + " canceled.");
        }
        else if (task.IsFaulted)
        {
            Debug.Log(operation + " encounted an error.");
            foreach (Exception exception in task.Exception.Flatten().InnerExceptions)
            {
                string authErrorCode = "";
                Firebase.FirebaseException firebaseEx = exception as Firebase.FirebaseException;
                if (firebaseEx != null)
                {
                    authErrorCode = String.Format("AuthError.{0}: ",
                      ((Firebase.Auth.AuthError)firebaseEx.ErrorCode).ToString());
                }
                Debug.Log(authErrorCode + exception.ToString());
            }
        }
        else if (task.IsCompleted)
        {
            Debug.Log(operation + " completed");
            complete = true;
        }
        return complete;
    }
}