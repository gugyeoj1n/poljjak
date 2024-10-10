using UnityEngine;
using System.Collections.Generic;
using Firebase;
using Firebase.Messaging;
using Firebase.Extensions;
using Firebase.Firestore;
using Unity.VisualScripting;

public class FirebaseManager : MonoBehaviour
{
    public FirebaseApp _app;

    public FirebaseFirestore db;
    void Start()
    {
        db = FirebaseFirestore.DefaultInstance;
        ReadUserData(FirebaseAuthManager.instance.uid);
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

    public void ReadUserData(string userId)
    {
        DocumentReference userRef = db.Collection("users").Document(userId);
        userRef.GetSnapshotAsync().ContinueWithOnMainThread(task =>
        {
            if (task.IsCompleted && task.Result.Exists)
            {
                DocumentSnapshot document = task.Result;
                Debug.LogFormat("User: {0}", document.Id);
                Dictionary<string, object> dic = document.ToDictionary();
                UIManager.instance.gem.text = dic["Gem"].ToString();
                Debug.LogFormat("Gem: {0}", dic["Gem"]);
            }
        });
    }    
    
}