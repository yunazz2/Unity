using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviourPunCallbacks, IPunObservable
{
    // 싱글톤 패턴으로 GameManager 인스턴스 작성 : 다른 스크립트에서 가져다 쓰려고 이렇게 작성 함
    private static GameManager instance;
    public static GameManager Instance()
    {
        return instance;
    } // 근데 이렇게 한 번 더 감싸는 이유는 이 인스턴스를 통해 값을 받아가야하는데 안에 값을 바꾸지는 못하도록 보호하기 위해


    public GameObject playerPrefabs;

    public int score = 0;
    public bool isGameOver = false;

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(score);
        }
        else
        {
            score = (int)stream.ReceiveNext();
            UIManager.Instance().UpdateScoreText(score);
        }
    }

    void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
    }
 
    private void Start()
    {
        Vector3 randomSpawnPos = Random.insideUnitSphere * 5.0f;
        randomSpawnPos.y = 0.0f;

        PhotonNetwork.Instantiate(playerPrefabs.name, randomSpawnPos, Quaternion.identity);

        // livingEntity는 플레이어와 좀비 모두 사용하는건데, 좀비가 죽는다고 게임 오버가 되면 안되니까 플레이어가 죽었을 때만 게임 오버가 되도록
        // PlayerHealth의 onDeath에 Action형(함수를 저장할 수 있는 변수)에 EndGame을 저장해놓고 플레이어가 죽으면 Die가 실행되어 onDeath도
        // 이어서 실행 시킨다.
        //FindObjectOfType<PlayerHealth>().onDeath += EndGame;
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))    // ESC 버튼 누르면 나갈 수 있도록
        {
            PhotonNetwork.LeaveRoom();
        }
    }

    // 점수 갱신
    public void AddScore(int newScore)
    {
        // 점수 추가하고 ui 갱신
        if(!isGameOver)
        {
            score += newScore;

            UIManager.Instance().UpdateScoreText(score);
        }
    }

    // 게임 오버 처리
    public void EndGame()
    {
        isGameOver = true;
        UIManager.Instance().SetActiveGameOverUI(true);
    }

    // 로비로 나가기
    public void BackToLobby()
    {
        Debug.Log("로비로 나가기!");
        PhotonNetwork.LeaveRoom();
    }

    // 게임 재시작
    public void GameRestart()
    {
        Debug.Log("게임 재시작~");

        StartCoroutine(DelayedInvoke()); // 코루틴을 통해 지연 실행
    }

    IEnumerator DelayedInvoke()
    {
        yield return new WaitForSeconds(0.5f);
     
        SceneManager.LoadScene(2);  // 빈 씬 로드
        Debug.Log("들어오니?");
        Debug.Log("현재 연결 상태: " + PhotonNetwork.NetworkClientState);
        SceneManager.LoadScene(1);
    }

    // 방을 나갈 때 자동으로 실행되는 함수
    public override void OnLeftRoom()
    {
        SceneManager.LoadScene("00 Lobby");
    }
}
