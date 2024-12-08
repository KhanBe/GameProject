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
    public int time;
    public int death;

    public Rank(string id, int time, int death)
    {
        this.id = id;
        this.time = time;
        this.death = death;
    }
}

public class RankManager : MonoBehaviour
{

    /*
    1. Init firebase database
    2. Get Total User
    3. Check User Exist in Database
    4. Fetch User Profile Data
    5. Fetch Leaderboard Data
    6. Display Leaderboard Data
    7. Add SignIn, SignOut / Leaderboard Click Event
    */

    public GameObject userNamePanel;
    public GameObject userProfilePanel;
    public GameObject leaderboardPanel;
    public GameObject leaderboardContent;

    public TMP_Text profileUserNameText;
    public TMP_Text profileUserTimeText;
    public TMP_Text profileUserDeathText;

    public TMP_InputField usernameInput;

    public int totalUser = 0;
    public string userName = "";
    
    private DatabaseReference db;


    private void Start() {
        
        firebaseInit();
    }

    void firebaseInit() {
        db = FirebaseDatabase.DefaultInstance.GetReference("/Leaderbaord/");

        db.ChildAdded += HandleChildAdded;
    }

    //Firebase 데이터베이스의 특정 노드에 새로운 자식 노드가 추가될 때 호출
    void HandleChildAdded(object sender, ChildChangedEventArgs args) {
        if (args.DatabaseError != null) {
            return;
        }

        GetTotalUser();

        StartCoroutine(FetchUserProfileData(PlayerPrefs.GetString("PlayerID")));
    }

    void GetTotalUser() {

        db.ValueChanged += (object sender2, ValueChangedEventArgs e2) => {
            if (e2.DatabaseError != null) {
                Debug.LogError(e2.DatabaseError.Message);
                return;
            }

            totalUser = int.Parse(e2.Snapshot.ChildrenCount.ToString());
        };
    }

    IEnumerator CheckUserExistInDatabase() {

    }

    IEnumerator FetchUserProfileData(string playerID)
    {

    }
    IEnumerator FetchLeaderboardData()
    {

    }
    void DisplayLeaderboardData()
    {

    }

    public string dburl = "https://woo-game-db-default-rtdb.firebaseio.com/";


}
