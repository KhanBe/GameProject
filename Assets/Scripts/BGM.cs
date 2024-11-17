using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGM : MonoBehaviour
{
    private static BGM instance = null;

    /*public static BGM Instance {
        get {
            if (instance == null) {
                instance = new BGM();
            }
            return instance;
        }
    }*/
    //생성자를 private으로 만들어 외부에서 인스턴스를 생성하지 못하도록 막습니다.
    private BGM() { }

    private void Awake() {
        if (instance == null) {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else {
            Destroy(gameObject);
        }
    }
}
