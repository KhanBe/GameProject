using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinSound : MonoBehaviour
{
    public AudioClip audioCoin;

    AudioSource audioSource;
    SpriteRenderer spriteRenderer;
    CircleCollider2D capsuleCollider;

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        capsuleCollider = GetComponent<CircleCollider2D>();
    }

    void DeActive()
    {
        //Destroy(gameObject);
        gameObject.SetActive(false);
    }
    public void getSound()
    {
        audioSource.clip = audioCoin;
        audioSource.Play();
        capsuleCollider.enabled = false;
        spriteRenderer.color = new Color(1, 1, 1, 0);
        Invoke("DeActive", 0.4f);
        

    }
}
