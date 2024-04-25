using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private static GameManager instance;    // �̱��� ������ �����ϱ� ���� ������ ����
    public static GameManager Instance()    // �� �޼ҵ带 ���� �ٸ� Ŭ�������� GameManager �ν��Ͻ��� ���� �����ϴ�.
    {
        return instance;
    }

    public GameObject gameoverText;         // ���� ���� �ؽ�Ʈ ������Ʈ
    public Text timeText;                   // �ð� �ؽ�Ʈ
    public Text recordText;                 // ��� �ؽ�Ʈ
    
    public float surviveTime = 0.0f;        // ���� �ð�
    private bool isGameOver = false;        // ���� ���� ����

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
