using System.Collections;
using UnityEngine;
using GooglePlayGames;
using GooglePlayGames.BasicApi;
using Firebase.Auth;
using TMPro;
 
public class FirebaseGoogleAuth : MonoBehaviour
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
}