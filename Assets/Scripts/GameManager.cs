using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public float levelStartDelay = 2f;
    public float turnDelay = .1f;
    public BoardManager boardScript;
    public int playerFoodPoint = 100;
    [HideInInspector] public bool playerTurn = true;

    private UnityEngine.UI.Text levelText;
    private GameObject levelImage;
    private int level;
    private List<Enemy> enemies;
    private bool enemiesMoving;
    private bool doingSetup;

    private void Awake()
    {
        level = Loader.level++;
        Loader.instance = this;
        enemies = new List<Enemy>();
        boardScript = GetComponent<BoardManager>();
        InitGame();
    }

    private void InitGame()
    {
        doingSetup = true;

        levelImage = GameObject.Find("LevelImage");
        levelText = GameObject.Find("LevelText").GetComponent<UnityEngine.UI.Text>();
        levelText.text = $"Day {level}";
        levelImage.SetActive(true);
        Invoke(nameof(HideLevelImage), levelStartDelay);
        
        enemies.Clear();
        boardScript.SetupScene(level);
    }

    private void HideLevelImage()
    {
        levelImage.SetActive(false);
        doingSetup = false;
    }

    public void GameOver()
    {
        levelText.text = $"After {level} of days, you starved.";
        levelImage.SetActive(true);
        enabled = false;
    }

    public void AddEnemyToList(Enemy script)
    {
        enemies.Add(script);
    }
    
    void Update()
    {
        if (playerTurn || enemiesMoving || doingSetup)
        {
            return;
        }

        StartCoroutine(MoveEnemies());
    }
    
    IEnumerator MoveEnemies()
    {
        enemiesMoving = true;
        yield return new WaitForSeconds(turnDelay);

        foreach (var enemy in enemies)
        {
            enemy.MoveEnemy();
            yield return new WaitForSeconds(enemy.moveTime);
        }

        playerTurn = true;
        enemiesMoving = false;
    }
}