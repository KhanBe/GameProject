using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIData : MonoBehaviour
{
    public static UIData instanceData;
    public int DiedCount;
    public int UIPoint;
    public int UIStage;

    public float second;
    public int minute;
    public int hour;
    public Text timeText;

    private void Awake()
    {
        if(instanceData != null && instanceData != this)
        {
            Destroy(gameObject);
        }
        else
        {
            instanceData = this;
            DontDestroyOnLoad(gameObject);
        }
    }
}
