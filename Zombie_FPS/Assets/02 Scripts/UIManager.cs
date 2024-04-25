using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Photon.Pun;

public class UIManager : MonoBehaviour
{
    // 싱글톤 패턴
    private static UIManager instance;
    public static UIManager Instance()
    {
        return instance;
    }

    public Text ammoText;   // 탄약 수
    public Text scoreText;  // 점수
    public Text waveText;   // 웨이브

    public GameObject gameOverUI;


    void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
    }

    // 탄알 수 갱신 함수
    public void UpdateAmmoText(int magAmmo, int remainAmmo)
    {
        //ammoText.text = magAmmo + "/" + remainAmmo;   // 아래랑 같은 내용임
        ammoText.text = string.Format("{0} / {1}", magAmmo, remainAmmo);
    }

    // 점수 갱신 함수
    public void UpdateScoreText(int newScore)
    {
        scoreText.text = string.Format("Score : {0:000}", newScore);
    }

    // 적 웨이브 텍스트 갱신
    public void UpdateWaveText(int waves, int count)
    {
        //waveText.text = string.Format("wave : {0:0}, enemy : {0:1}", waves, count);   // 내가 작성한 방법
        waveText.text = $"Wave : {waves} \nEnemy : {count}";    // 아래는 유정씨가 작성한 방법
    }

    // 게임 오버 UI 활성화
    public void SetActiveGameOverUI(bool active)
    {
        gameOverUI.SetActive(active);
    }

    // 게임 재시작
    public void GameRestart()
    {
        Debug.Log("게임 재시작~");
        SceneManager.LoadScene(2);  // 빈 씬 로드
        Debug.Log("현재 연결 상태: " + PhotonNetwork.NetworkClientState);
        SceneManager.LoadScene(1);
    }
}
