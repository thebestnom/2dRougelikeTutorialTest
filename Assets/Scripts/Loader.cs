﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Loader : MonoBehaviour
{
    public GameObject gameManager;
    public static GameManager instance;
    public static int level = 1;
    
    void Awake()
    {
        Instantiate(gameManager);
    }
}
