using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blood : MonoBehaviour
{
    SpriteRenderer spriteRenderer;
    public Sprite[] bloodSprites;
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = bloodSprites[Random.Range(0,bloodSprites.Length)];
    }
}
