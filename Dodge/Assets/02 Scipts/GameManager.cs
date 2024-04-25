using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private static GameManager instance;    // 싱글톤 패턴을 구현하기 위해 정의한 변수
    public static GameManager Instance()    // 이 메소드를 통해 다른 클래스에서 GameManager 인스턴스에 접근 가능하다.
    {
        return instance;
    }

    public GameObject gameoverText;         // 게임 오버 텍스트 오브젝트
    public Text timeText;                   // 시간 텍스트
    public Text recordText;                 // 기록 텍스트
    
    public float surviveTime = 0.0f;        // 생존 시간
    private bool isGameOver = false;        // 게임 오버 여부

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
    }

    void Start()
    {
        surviveTime = 0.0f;
        isGameOver = false;
    }

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
