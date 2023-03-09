using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Snake : MonoBehaviour
{
    public int width, height;

    public GameObject blockPrefab;
    public GameObject snakeBodyPrefab;
    public GameObject foodPrefab;
    public float moveUnitsPerSecond = 0.1f;
    public TextMeshProUGUI score;
    public TextMeshProUGUI highestScore;

    GameObject head;
    GameObject food;
    GameObject bonusPoint;
    GameObject obstacle;
    List<GameObject> tailList;
    Vector2 direction;

    float timeBonusPointDuration = 5f;
    float timeBonusPointCount = 5f,scoreCount = 0;
    int highestScoreValue = 0;
    float passedTime;
    int totalFoods = 0;

    private void Start()
    {
        width++; height++;
        direction = Vector2.right;
        createGrid();
        createPlayer();
        spawnFood();
        createObstacle();
        readHighestScoreFromJson();
        highestScore.text = "Highest: " + highestScoreValue.ToString();
        // hide the bonus point
        score.gameObject.SetActive(true);
        highestScore.gameObject.SetActive(true);
    }
    private void createObstacle()
    {
        Vector2 ObstaclePos = getRandomPosition();
        while (constainedInSnake(ObstaclePos) || ObstaclePos == (Vector2)food.transform.position || ObstaclePos == (Vector2)head.transform.position)
        {
            ObstaclePos = getRandomPosition();
        }
        obstacle = Instantiate(blockPrefab, new Vector3(ObstaclePos.x, ObstaclePos.y, 0), Quaternion.identity);
        // set the color of the obstacle
        obstacle.GetComponent<SpriteRenderer>().color = Color.black;
        obstacle.transform.SetParent(transform);
    }
    private void spawnFood()
    {
        Vector2 spawnPos = getRandomPosition();
        while (constainedInSnake(spawnPos))
        {
            spawnPos = getRandomPosition();
        }
        food = Instantiate(foodPrefab) as GameObject;
        food.transform.position = new Vector3(spawnPos.x, spawnPos.y, 0);
    }
    private bool constainedInSnake(Vector2 spawnPos)
    {
        bool isInHead = spawnPos == (Vector2)head.transform.position;
        bool isInTail = false;
        foreach (var item in tailList)
        {
            if ((Vector2)item.transform.position == spawnPos)
            {
                isInTail = true;
                break;
            }
        }
        return isInHead || isInTail;

    }
    private Vector2 getRandomPosition()
    {
        return new Vector2(UnityEngine.Random.Range(-width / 2 + 1, width / 2), UnityEngine.Random.Range(-height / 2 + 1, height / 2));
    }
    private void createPlayer()
    {
        // create head and 2 tail
        head = Instantiate(snakeBodyPrefab) as GameObject;
        head.transform.position = new Vector3(0, 0, 0);
        tailList = new List<GameObject>();
        for (int i = 0; i < 2; i++)
        {
            GameObject tail = Instantiate(snakeBodyPrefab) as GameObject;
            tail.transform.position = new Vector3(0, -1, 0);
            tailList.Add(tail);
        }
    }
    private void createGrid()
    {
        for (int x = 0; x <= width; x++)
        {
            GameObject borderBottom = Instantiate(blockPrefab) as GameObject;
            borderBottom.GetComponent<Transform>().position = new Vector3(x - (width / 2), -height / 2, 0);

            GameObject borderTop = Instantiate(blockPrefab) as GameObject;
            borderTop.GetComponent<Transform>().position = new Vector3(x - (width / 2), height - (height / 2), 0);
        }
        for (int y = 0; y <= height; y++)
        {
            GameObject borderLeft = Instantiate(blockPrefab) as GameObject;
            borderLeft.GetComponent<Transform>().position = new Vector3((-width / 2), y - (height / 2), 0);

            GameObject borderRight = Instantiate(blockPrefab) as GameObject;
            borderRight.GetComponent<Transform>().position = new Vector3(width - (width / 2), y - (height / 2), 0);
        }
    }
    private void createBonusPoint()
    {
        Vector2 randombonusPoint = getRandomPosition();
        while (constainedInSnake(randombonusPoint) || randombonusPoint == (Vector2)food.transform.position || randombonusPoint == (Vector2)head.transform.position || randombonusPoint == (Vector2)obstacle.transform.position)
        {
            randombonusPoint = getRandomPosition();
        }
        bonusPoint = Instantiate(foodPrefab, new Vector3(randombonusPoint.x, randombonusPoint.y, 0), Quaternion.identity);
        // set the color of the bonus point
        bonusPoint.GetComponent<SpriteRenderer>().color = Color.green;
        // display time bonus point
        //timeBonusPoint.text = "Time: " + timeBonusPointDuration.ToString();
        //timeBonusPoint.gameObject.SetActive(true);
        // set time to destroy the bonus point
        Destroy(bonusPoint, timeBonusPointDuration);
    }
    private void writeHightestScoreToJson()
    {
        string path = Application.dataPath + "/Scripts/highestScore.json";
        string json = JsonUtility.ToJson(new HighestScore(highestScoreValue));
        System.IO.File.WriteAllText(path, json);
    }
    private void checkHighestScore()
    {
        if (scoreCount >= highestScoreValue)
        {
            highestScoreValue = (int)scoreCount;
            writeHightestScoreToJson();
        }
    }
    private void readHighestScoreFromJson()
    {
        string path = Application.dataPath + "/Scripts/highestScore.json";
        if (System.IO.File.Exists(path))
        {
            string json = System.IO.File.ReadAllText(path);
            HighestScore highestScore = JsonUtility.FromJson<HighestScore>(json);
            // check if the json is empty
            if (highestScore != null)
            {
                highestScoreValue = highestScore.highestScore;
            }
        }
    }
    [System.Serializable]
    private class HighestScore
    {
        public int highestScore;
        public HighestScore(int highestScore)
        {
            this.highestScore = highestScore;
        }
    }
    private void Update()
    {
        // move vertical
        if (Input.GetKeyDown(KeyCode.UpArrow) && direction != Vector2.down)
        {
            direction = Vector2.up;
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow) && direction != Vector2.up)
        {
            direction = Vector2.down;
        }
        // move horizontal
        else if (Input.GetKeyDown(KeyCode.LeftArrow) && direction != Vector2.right)
        {
            direction = Vector2.left;
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow) && direction != Vector2.left)
        {
            direction = Vector2.right;
        }
        
        if (bonusPoint != null) timeBonusPointCount -= Time.deltaTime;
        else { timeBonusPointCount = 5f; }
        passedTime += Time.deltaTime;
        if (moveUnitsPerSecond < passedTime)
        {
            passedTime = 0;
            // Move
            Vector3 newPos = head.GetComponent<Transform>().position + new Vector3(direction.x, direction.y, 0);

            // check if colliders with border
            if (newPos.x <= -width / 2 || newPos.x >= width / 2 || newPos.y <= -height / 2 || newPos.y >= height / 2 + 1)
            {
                checkHighestScore();
                // Game Over
                Debug.Log("Game Over");
                return;
            }

            // check if colliders with tail
            foreach (var item in tailList)
            {
                checkHighestScore();
                if (item.transform.position == newPos)
                {
                    // Game Over
                    Debug.Log("Game Over");
                    return;
                }
            }

            // check if colliders with obstacle
            if (obstacle.transform.position == newPos)
            {
                checkHighestScore();
                // Game Over
                Debug.Log("Game Over");
                return;
            }

            // check if time bonus point is active
            //if (bonusPoint != null)
            //{
            //    if (timeBonusPointCount >= 1)
            //    {
            //        timeBonusPoint.text = "Time: " + (timeBonusPointDuration - 1).ToString();
            //        timeBonusPointCount = 0;
            //        if (timeBonusPointDuration == 0)
            //        {
            //            timeBonusPoint.gameObject.SetActive(false);
            //            timeBonusPointDuration = 5;
            //        }
            //    }
            //}
            // check if snake ate bonus point
            if (bonusPoint != null && bonusPoint.transform.position == newPos)
            {
                // add score
                scoreCount += (scoreCount * timeBonusPointCount);
                Debug.Log("timeBonusPointCount: " + timeBonusPointCount);
                timeBonusPointCount = 0;
                // display score
                score.text = "Score: " + ((int)scoreCount).ToString();
                // destroy bonus point
                Destroy(bonusPoint);
            }

            // check if snake ate food
            if (newPos.x == food.transform.position.x && newPos.y == food.transform.position.y)
            {
                // add body
                GameObject newTile = Instantiate(snakeBodyPrefab);
                newTile.transform.position = food.transform.position;
                Destroy(food);
                tailList.Add(head);
                head = newTile;

                // increase score
                scoreCount++;
                totalFoods++;
                string scoreResult = "Score: " + ((int)scoreCount).ToString();
                score.text = scoreResult;
                spawnFood();

                // create bonus point when snake ate 5 foods
                if (totalFoods == 5)
                {
                    createBonusPoint();
                    totalFoods = 0;
                }
            }
            else
            {
                if (tailList.Count == 0)
                {
                    head.transform.position = newPos;
                }
                else
                {
                    tailList.Add(head);
                    head = tailList[0];
                    tailList.RemoveAt(0);
                    head.transform.position = newPos;
                }
            }

        }
    }

}
