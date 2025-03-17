using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase.Auth;
using TMPro;

public class FirebaseLogin : MonoBehaviour
{
    public TMP_InputField emailField;
    public TMP_InputField passwordField;
    public TMP_Text text;
    
    public string email;
    public string password;
    
    public FirebaseAuth firebaseAuth;
    
    private void Awake()
    {
        firebaseAuth = FirebaseAuth.DefaultInstance;
    }

    public void OnClickSignInButton()
    {
        email = emailField.text;
        password = passwordField.text;
        
        firebaseAuth.CreateUserWithEmailAndPasswordAsync(email, password).ContinueWith(task => 
        {
            if (task.IsCanceled)
            {
                text.text = "가입 취소";
                return;
            }
            if (task.IsFaulted)
            {
                Debug.LogError("가입 실패: " + task.Exception?.InnerExceptions[0].Message);
                text.text = "가입 실패";
                return;
            }
            
            text.text = "가입 성공";
        });
    }

    public void OnClickSignUpButton()
    {
        email = emailField.text;
        password = passwordField.text;
        
        firebaseAuth.SignInWithEmailAndPasswordAsync(email, password).ContinueWith(task => 
        {
            if (task.IsCanceled)
            {
                text.text = "로그인 취소";
                return;
            }
            if (task.IsFaulted)
            {
                text.text = "로그인 실패";
                return;
            }
            
            text.text = "로그인 성공";
        });
    }
}
