using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIData : MonoBehaviour
{
    public int DiedCount;
    public int UIPoint;
    public int UIStage;
    public float second;
    public int minute;
    public int hour;
    public Text timeText;

    private static UIData instance = null;
    public static UIData Instance { 
        get {
            return instance;
        }
    }

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject); // 중복된 인스턴스 제거
            return;
        }
        
        instance = this;
        DontDestroyOnLoad(gameObject); // 씬 전환 시 유지
    }
}
