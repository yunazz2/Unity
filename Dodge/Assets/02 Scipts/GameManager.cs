using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private static GameManager instance;
    public static GameManager Instance()
    {
        return instance;
    }

    public GameObject gameoverText;
    public Text timeText;
    public Text recordText;

    public float surviveTime = 0.0f;
    private bool isGameOver = false;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        surviveTime = 0.0f;
        isGameOver = false;

    }

    // Update is called once per frame
    void Update()
    {
        if(!isGameOver)
        {
            surviveTime += Time.deltaTime;
            timeText.text = string.Format("Time : {0:0}", surviveTime);
        }
        else
        {
            if(Input.GetKeyDown(KeyCode.R))
            {
                SceneManager.LoadScene(0);
            }
        }

    }

    public void EndGame()
    {
        isGameOver = true;
        gameoverText.SetActive(true);

        float bestTime = PlayerPrefs.GetFloat("BestTime");

        if(surviveTime > bestTime)
        {
            bestTime = surviveTime;
            PlayerPrefs.SetFloat("BestTime", bestTime);
        }

        recordText.text = string.Format("Best Time : {0}", (int)bestTime);

    }

    public void Exit()
    {
        Application.Quit();
    }
}
