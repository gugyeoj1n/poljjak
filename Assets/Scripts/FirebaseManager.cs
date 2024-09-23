using UnityEngine;
using Firebase;
using Firebase.Messaging;
using Unity.VisualScripting;

public class FirebaseManager : MonoBehaviour
{
    public FirebaseApp _app;
    void Start()
    {
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task =>
        {
            if (task.Result == DependencyStatus.Available)
            {
                _app = FirebaseApp.DefaultInstance;

                FirebaseMessaging.TokenReceived += OnTokenReceived;

                FirebaseMessaging.MessageReceived += OnMessageReceived;
            }
            else
            {
                Debug.LogError("[FIREBASE] Could not resolve all dependencies " + task.Result);
            }
        });
    }

    void OnTokenReceived(object sender, TokenReceivedEventArgs e)
    {
        if (e != null)
        {
            Debug.LogFormat("[FIREBASE] Token: {0}", e.Token);
        }
    }

    void OnMessageReceived(object sender, MessageReceivedEventArgs e)
    {
        if (e != null && e.Message != null && e.Message.Notification != null)
        {
            Debug.LogFormat("[FIREBASE] From: {0}, Title: {1}, Text: {2}",
                e.Message.From,
                e.Message.Notification.Title,
                e.Message.Notification.Body);
        }
    }
}
