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

public class RankData
{   
    public string username;
    public string time;
    public int death;

    public RankData(string username, string time, int death)
    {
        this.username = username;
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

    //public GameObject userNamePanel;
    //public GameObject userProfilePanel;
    public GameObject leaderboardPanel;
    public GameObject leaderboardContent;

    //public TMP_Text profileUserNameText;
    public TMP_Text profileUserTimeText;
    public TMP_Text profileUserDeathText;
    public TMP_Text errorUsernameText;

    public TMP_InputField usernameInput;

    public int totalUser = 0;
    public string userName = "";
    public string time = "";
    public int death = 0;

    private DatabaseReference db;
    public string dburl = "https://woo-game-db-default-rtdb.firebaseio.com/";


    public void SignInWithUsername() {
        StartCoroutine(CheckUserExistInDatabase());
    }

    private void Start() {
        
        firebaseInit();
    }

    public void GetData() {
        time = UIData.Instance.timeText.text;
        death = UIData.Instance.DiedCount;
    }

    public void SignOut() {
        PlayerPrefs.DeleteKey("PlayerID");
        PlayerPrefs.DeleteKey("Username");
    }

    void firebaseInit() {
        db = FirebaseDatabase.DefaultInstance.GetReference("/Leaderboard/");

        //db Leaderboard 데이터 자식값이 바뀌는 델리게이트 매핑(함수참조를 통해)
        db.ChildAdded += HandleChildAdded;
    }

    //Firebase 데이터베이스의 특정 노드(Leaderboard)에 새로운 자식 노드가 추가될 때 호출
    void HandleChildAdded(object sender, ChildChangedEventArgs args) {
        if (args.DatabaseError != null) {
            return;
        }

        GetTotalUser();

        StartCoroutine(FetchUserProfileData(PlayerPrefs.GetInt("PlayerID")));
    }

    void GetTotalUser() {

        db.ValueChanged += (object sender2, ValueChangedEventArgs e2) => {  
            if (e2.DatabaseError != null) {
                Debug.LogError(e2.DatabaseError.Message);
                return;
            }

            totalUser = int.Parse(e2.Snapshot.ChildrenCount.ToString());
            
            Debug.LogError("TotalUser : " + totalUser);
        };
    }

    //Username이 존재하는지 확인 하는 함수
    IEnumerator CheckUserExistInDatabase() {
        var task =  db.OrderByChild("Username").EqualTo(usernameInput.text).GetValueAsync();
        yield return new WaitUntil(() => task.IsCompleted);

        if (task.IsFaulted) {
            Debug.LogError("Invalid Error");
            errorUsernameText.text = "Invalid Error";
        }
        else if (task.IsCompleted) {
            DataSnapshot snapshot = task.Result;

            if (snapshot != null && snapshot.HasChildren) {
                Debug.LogError("Username Exist");

                errorUsernameText.text = "Username Already Exist";
            }
            else {
                Debug.LogError("Username Not Exist");

                //새로운 data 넣기
                PushUserData();
                PlayerPrefs.SetInt("PlayerID", totalUser + 1);
                PlayerPrefs.SetString("Username", usernameInput.text);

                StartCoroutine(delayFetchProfile());
            }
        }
    }

    IEnumerator FetchUserProfileData(int playerID)
    {
        if (playerID != 0) {
            var task = db.Child("User_" + playerID.ToString()).GetValueAsync();

            yield return new WaitUntil(() => task.IsCompleted);

            if (task.IsFaulted)
            {
                Debug.LogError("Invalid Error");
            }
            else if (task.IsCompleted) {
                DataSnapshot snapshot = task.Result;

                //정상 작동
                if (snapshot != null && snapshot.HasChildren) {
                    userName = snapshot.Child("Username").Value.ToString();
                    time = snapshot.Child("Time").Value.ToString();
                    death = int.Parse(snapshot.Child("Death").Value.ToString());

                    //profileUserNameText.text = userName;
                    profileUserTimeText.text = time;
                    profileUserDeathText.text = "" + death;
                }
                else {
                    Debug.LogError("User ID Not Exist");
                }
            }
        }

        yield return null;
    }

    IEnumerator delayFetchProfile() {
        yield return new WaitForSeconds(1f);
        StartCoroutine(FetchUserProfileData(totalUser));
    }

    void PushUserData() {
        db.Child("User_" + (totalUser + 1).ToString()).Child("Username").SetValueAsync(usernameInput.text);
        db.Child("User_" + (totalUser + 1).ToString()).Child("Time").SetValueAsync(time);
        db.Child("User_" + (totalUser + 1).ToString()).Child("Death").SetValueAsync(death);
    }

    IEnumerator FetchLeaderboardData()
    {
        var task = db.Child("Time").LimitToLast(10).GetValueAsync();

        yield return new WaitUntil(() => task.IsCompleted);

        if (task.IsFaulted)
        {
            Debug.LogError("Invalid Error");
        }
        else if (task.IsCompleted)
        {
            DataSnapshot snapshot = task.Result;

            List<RankData> listRankEntry = new List<RankData>();

            foreach (var childSnapShot in snapshot.Children) {
                string username = childSnapShot.Child("Username").Value.ToString();
                string time = childSnapShot.Child("Time").Value.ToString();
                int death = int.Parse(childSnapShot.Child("Death").Value.ToString());

                listRankEntry.Add(new RankData(username, time, death));
            }
        }
    }

    void DisplayLeaderboardData()
    {

    }

}
