using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using System;
using Firebase;
using Firebase.Database;
using Firebase.Extensions;

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
    //private Firebase.FirebaseApp app;

    public static UIData UIinstance;

    //int Death = UIinstance.DiedCount;  

    private void Awake()
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

        // URL을 명시적으로 설정
        FirebaseApp app = FirebaseApp.DefaultInstance;
        app.Options.DatabaseUrl = new System.Uri("https://woo-game-db-default-rtdb.firebaseio.com/");
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
}
