using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Xml.Serialization;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public GameObject gameOverUI;
    public bool isGameOver = false;
    public Text scoreText;
    private int score = 0;

    void Awake()    // Awake�� ���� ������ Start�������� �� ���� �ҷ����� �Ϸ���
    {
        if(instance == null) {
            instance = this;
        }
        else {
            Destroy(gameObject);
        }
    }

    void Update()
    {
        if (isGameOver && Input.GetMouseButtonDown(0))
        {
            SceneManager.LoadScene(1);
        }
    }

    public void AddScore(int newScore)
    {
        if(!isGameOver)
        {
            score += newScore;
            scoreText.text = string.Format("Score : {0}", score);
        }
    }

    public void onPlayerDead()
    {
        isGameOver = true;
        gameOverUI.SetActive(true);
    }

    public void GameStart()
    {
        SceneManager.LoadScene(1);
    }

    public void Exit()
    {
        Application.Quit();
    }
}
