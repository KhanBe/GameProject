using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class STAGE : MonoBehaviour
{
    private static STAGE instance;

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
