using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIData : MonoBehaviour
{
    public static UIData instanceData;
    public int DiedCount;
    public int UIPoint;
    public int UIStage;

    private void Awake()
    {
        if(instanceData != null)
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
