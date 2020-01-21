using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Remoting.Messaging;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MovingObject
{
    private const string HORIZONTAL = "Horizontal";
    private const string VERTICAL = "Vertical";
    
    public int wallDamage = 1;
    public int pointsPerFood = 10;
    public int pointsPerSoda = 20;
    public float restartLevelDelay = 1f;
    public static readonly float MoveAnimationTime = 1f;

    private Animator animator;
    private int food;
    private static readonly int PlayerChop = Animator.StringToHash("playerChop");
    private static readonly int PlayerHit = Animator.StringToHash("playerHit");

    protected override void Start()
    {
        animator = GetComponent<Animator>();

        food = Loader.instance.playerFoodPoint;
        
        base.Start();
    }

    private void OnDisable()
    {
        Loader.instance.playerFoodPoint = food;
    }
    
    // Update is called once per frame
    void Update()
    {
        if (!Loader.instance.playerTurn) return;

        var horizontal = (int) Input.GetAxisRaw(HORIZONTAL);
        var vertical =  (int) Input.GetAxisRaw(VERTICAL);

        if (horizontal != 0) vertical = 0;

        if (horizontal != 0 || vertical != 0)
        {
            AttemptMove<Wall>(horizontal, vertical);
        }
    }

    protected override void AttemptMove<T>(int xDir, int yDir)
    {
        food--;
        base.AttemptMove<T>(xDir, yDir);
        RaycastHit2D hit;
        CheckIfGameOver();
        Loader.instance.playerTurn = false;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        switch (other.tag)
        {
            case "Exit":
                Invoke(nameof(Restart), restartLevelDelay);
                break;
            case "Food":
                food += pointsPerFood;
                other.gameObject.SetActive(false);
                break;
            case "Soda":
                food += pointsPerSoda;
                other.gameObject.SetActive(false);
                break;
            default:
                break;
        }
    }

    protected override void OnCantMove<T>(T component)
    {
        var hitWall = component as Wall;
        // ReSharper disable once PossibleNullReferenceException
        hitWall.DamageWall(wallDamage);
        animator.SetTrigger(PlayerChop);
    }

    private void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void LoseFood(int loss)
    {
        animator.SetTrigger(PlayerHit);
        food -= loss;
        CheckIfGameOver();
    }

    private void CheckIfGameOver()
    {
        if(food <= 0) Loader.instance.GameOver();
    }
}
