using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // 싱글톤 패턴으로 GameManager 인스턴스 작성 : 다른 스크립트에서 가져다 쓰려고 이렇게 작성 함
    private static GameManager instance;
    public static GameManager Instance()
    {
        return instance;
    } // 근데 이렇게 한 번 더 감싸는 이유는 이 인스턴스를 통해 값을 받아가야하는데 안에 값을 바꾸지는 못하도록 보호하기 위해

    public int score = 0;
    public bool isGameOver = false;

    void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
    }

    private void Start()
    {
        // livingEntity는 플레이어와 좀비 모두 사용하는건데, 좀비가 죽는다고 게임 오버가 되면 안되니까 플레이어가 죽었을 때만 게임 오버가 되도록
        // PlayerHealth의 onDeath에 Action형(함수를 저장할 수 있는 변수)에 EndGame을 저장해놓고 플레이어가 죽으면 Die가 실행되어 onDeath도
        // 이어서 실행 시킨다.
        FindObjectOfType<PlayerHealth>().onDeath += EndGame;
    }

    // 점수 갱신 함수
    public void AddScore(int newScore)
    {
        // 점수 추가하고 ui 갱신
        if(!isGameOver)
        {
            score += newScore;

            UIManager.Instance().UpdateScoreText(score);
        }
        
    }

    public void EndGame()
    {
        // 게임 오버 처리
        isGameOver = true;
        UIManager.Instance().SetActiveGameOverUI(true);
    }
}
