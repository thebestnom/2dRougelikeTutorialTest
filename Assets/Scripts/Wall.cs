﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wall : MonoBehaviour
{
    public Sprite damageSprite;
    public int hp = 4;
    public AudioClip[] chopSounds;

    private SpriteRenderer spriteRenderer;
    
    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void DamageWall(int loss)
    {
        SoundManager.instance.RandomizeSfx(chopSounds);
        spriteRenderer.sprite = damageSprite;
        hp -= loss;
        if (hp <= 0) gameObject.SetActive(false);
    }
}
