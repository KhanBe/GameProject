using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasUI : MonoBehaviour
{
    private static CanvasUI instance;
    public static CanvasUI Instance { 
        get {
            return instance;
        }
    }

    private void Awake()
    {
        Debug.Log("CanvasUI Awake함수 호출");
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }
        
        instance = this;
        DontDestroyOnLoad(gameObject);
    }
    
    // 자식 오브젝트 중 n번째(index)자식 활성화/비활성화
    public void ChildrenSetActive(int n, bool active)
    {
        for (int i = 0; i < transform.childCount; i++) // 1번째 자식부터 반복
        {   
            if (i == n) {
                Transform child = transform.GetChild(i);
                child.gameObject.SetActive(active);
                return;
            }
        }
    }
}
