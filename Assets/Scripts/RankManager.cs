using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

using System;
using Firebase;
using Firebase.Database;
using Firebase.Extensions;
using Firebase.Auth;

public class Rank
{   
    public string id;
    public int score;
    public int time;
    public int death;

    public Rank(string id, int score, int time, int death)
    {
        this.id = id;
        this.score = score;
        this.time = time;
        this.death = death;
    }
}

public class RankManager : MonoBehaviour
{
    public static RankManager instanceData;
    
    public static UIData UIinstance;

    private FirebaseAuth auth;
    private FirebaseUser user;

    public TMP_InputField email;
    public TMP_InputField password;

    //int Death = UIinstance.DiedCount;  

    /*private void Awake()
    {
        if (instanceData != null)
        {
            Destroy(gameObject);
        }
        else
        {
            instanceData = this;
            DontDestroyOnLoad(gameObject);
        }
    }*/

    private void Start() {
        auth = FirebaseAuth.DefaultInstance;

    }

    public void SaveRank(int rank, string userId, int score, int time, int death)
    {
        
        DatabaseReference reference = FirebaseDatabase.DefaultInstance.RootReference;
        Rank ranking = new Rank(userId, score, time, death);


        //Extensions import
        reference.Child("rank").Child(rank.ToString()).SetRawJsonValueAsync(JsonUtility.ToJson(ranking)).ContinueWithOnMainThread(task =>
        {
            if (task.IsCompleted)
            {
                Debug.Log("Rank saved successfully!");
            }
        });
    }

    public void OnBtnSubmit() {
        SaveRank(6, "testtest", 113355, 123, 4444);
    }

    
    public void Create() {
        auth.CreateUserWithEmailAndPasswordAsync(email.text, password.text).ContinueWith(task => {
            if (task.IsCanceled)
            {
                Debug.LogError("회원가입 취소");
                return;
            }
            if (task.IsFaulted)
            {
                //이메일 비정상,간단한 비밀번호, 이메일 중복
                Debug.LogError("회원가입 실패");
                return;
            }

            FirebaseUser newUser = task.Result.User;
            Debug.LogError("회원가입 완료");
         });
    }

    public void LogIn()
    {
        auth.SignInWithEmailAndPasswordAsync(email.text, password.text).ContinueWith(task =>
        {
            if (task.IsCanceled)
            {
                Debug.LogError("로그인 취소");
                return;
            }
            if (task.IsFaulted)
            {
                //이메일 비정상,간단한 비밀번호, 이메일 중복
                Debug.LogError("로그인 실패");
                return;
            }

            FirebaseUser newUser = task.Result.User;
            Debug.LogError("로그인 완료");
        });    
    }
    public void LogOut()
    {
        auth.SignOut();
        Debug.LogError("로그아웃 완료");
    }
}
