using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGScroller : MonoBehaviour
{
    private MeshRenderer render;
    private float offset;
    private float playerSpeedX;

    
    // Start is called before the first frame update
    void Start()
    {
        render = GetComponent<MeshRenderer>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        playerSpeedX = PlayerMove.Instance.curXSpeed;
        offset += Time.deltaTime * playerSpeedX *0.03f;
        render.material.mainTextureOffset = new Vector2(offset, 0);
    }
}
