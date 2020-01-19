using System;
using System.Collections.Generic;
using UnityEngine;

public class BoardManager : MonoBehaviour
{
    [Serializable]
    public class Count
    {
        public int minimum;
        public int maximum;

        public Count(int minimum, int maximum)
        {
            this.minimum = minimum;
            this.maximum = maximum;
        }
    }

    public int columns = 8;
    public int rows = 8;
    public Count wallCount = new Count(5, 9);
    public Count foodCount = new Count(1, 5);
    public GameObject exit;
    public GameObject[] floorTiles;
    public GameObject[] foodTiles;
    public GameObject[] enemyTiles;
    public GameObject[] wallTiles;
    public GameObject[] outerWallTiles;

    private Transform boardHolder;
    private readonly List<Vector3> gridPositions = new List<Vector3>();

    private void InitialiseList()
    {
        gridPositions.Clear();

        for (var x = 1; x < columns - 1; x++)
        {
            for (var y = 1; y < rows - 1; y++)
            {
                gridPositions.Add(new Vector3(x, y, 0f));
            }
        }
    }

    private bool IsOuterWall(int x, int y)
    {
        return x == -1 || x == columns || y == -1 || y == rows;
    }

    private void BoardSetup()
    {
        boardHolder = new GameObject("board").transform;
        for (var x = -1; x < columns + 1; x++)
        {
            for (var y = -1; y < rows + 1; y++)
            {
                var toInstantiate = IsOuterWall(x, y)
                    ? outerWallTiles[UnityEngine.Random.Range(0, outerWallTiles.Length)]
                    : floorTiles[UnityEngine.Random.Range(0, floorTiles.Length)];

                var instantiate = Instantiate(toInstantiate, new Vector3(x, y, 0), Quaternion.identity);
                instantiate.transform.SetParent(boardHolder);
            }
        }
    }

    private void LayoutObjectAtRandom(IReadOnlyList<GameObject> tileArray, int minimum, int maximum)
    {
        var objectCount = UnityEngine.Random.Range(minimum, maximum + 1);
        for (var i = 0; i < objectCount; i++)
        {
            var randomPosition = RandomPosition();
            var tileChoice = tileArray[UnityEngine.Random.Range(0, tileArray.Count)];
            Instantiate(tileChoice, randomPosition, Quaternion.identity);
        }

    }

    private Vector3 RandomPosition()
    {
        var randomIndex = UnityEngine.Random.Range(9, gridPositions.Count);
        var randomPosition = gridPositions[randomIndex];
        gridPositions.RemoveAt(randomIndex);
        return randomPosition;
    }

    public void SetupScene(int level)
    {
        BoardSetup();
        InitialiseList();
        LayoutObjectAtRandom(wallTiles, wallCount.minimum, wallCount.maximum);
        LayoutObjectAtRandom(foodTiles, foodCount.minimum, foodCount.maximum);
        var enemyCount = (int)Mathf.Log(level, 2f);
        LayoutObjectAtRandom(enemyTiles, enemyCount, enemyCount);
        Instantiate(exit, new Vector3(columns - 1, rows - 1, 0f), Quaternion.identity);
    }
}