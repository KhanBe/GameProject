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

    private static UIData instance;
    public static UIData Instance { 
        get {
            return instance;
        }
        private set {       
            instance = Instance;
        }
    }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject); // 중복된 인스턴스 제거
            return;
        }
        
        Instance = this;
        DontDestroyOnLoad(gameObject); // 씬 전환 시 유지
    }
}
