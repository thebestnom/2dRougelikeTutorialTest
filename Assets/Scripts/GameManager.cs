using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public float turnDelay = .1f;
    public BoardManager boardScript;
    public int playerFoodPoint = 100;
    [HideInInspector] public bool playerTurn = true;

    private int level;
    private List<Enemy> enemies;
    private bool enemiesMoving;

    private void Awake()
    {
        level = Loader.level++;
        Loader.instance = this;
        enemies = new List<Enemy>();
        boardScript = GetComponent<BoardManager>();
        InitGame();
    }

    public void ReloadScene()
    {
    }

    private void InitGame()
    {
        enemies.Clear();
        boardScript.SetupScene(level);
    }

    public void GameOver()
    {
        enabled = false;
    }

    public void AddEnemyToList(Enemy script)
    {
        enemies.Add(script);
    }
    
    void Update()
    {
        if (playerTurn || enemiesMoving)
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
        
        if (enemies.Count < Player.MoveAnimationTime)
        {
            yield return new WaitForSeconds(Player.MoveAnimationTime - (enemies.Count * 0.1f));
        }

        playerTurn = true;
        enemiesMoving = false;
    }
}