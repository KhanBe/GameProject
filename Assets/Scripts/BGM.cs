using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGM : MonoBehaviour
{
    public static BGM instanceData;

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
    }
}
