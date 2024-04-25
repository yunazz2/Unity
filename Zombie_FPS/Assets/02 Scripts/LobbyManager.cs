using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;

// 마스터의 서버와 룸 접속 담당하는 스크립트
public class LobbyManager : MonoBehaviourPunCallbacks   // 포톤 서버 이용할 때는 그냥 MonoBehaviour가 아닌 MonoBehaviourPunCallbacks 사용
{
    private string gameVersion = "1";

    public Text connectionInfoText;
    public Button joinButton;

    void Start()
    {
        // 무조건 아래 두 개를 써야 포톤 사용이 가능하다.
        // 접속에 필요한 정보 설정
        PhotonNetwork.GameVersion = gameVersion;
        // 설정한 정보로 마스터 서버에 접속 시도
        PhotonNetwork.ConnectUsingSettings();

        // 룸 접속 버튼을 잠시 비활성화
        joinButton.interactable = false;

        // 접속 시도 중임을 텍스트로 표시
        connectionInfoText.text = "마스터 서버에 접속 중!";
    }

    // 마스터 서버에 접속 성공 시 자동 실행될 함수
    public override void OnConnectedToMaster()
    {
        // 서버 접속에 성공했으니 룸 접속 버튼을 다시 활성화
        joinButton.interactable = true;
        connectionInfoText.text = "온라인 : 마스터 서버와 연결 됨";
    }

    // 마스터 서버에 접속 실패 시 자동 실행될 함수
    public override void OnDisconnected(DisconnectCause cause)
    {
        joinButton.interactable = false;
        connectionInfoText.text = "오프라인 : 마스터 서버와 연결 되지 않음 \n접속 재시도 중!";

        // 마스터 서버로 재접속 시도
        PhotonNetwork.ConnectUsingSettings();
    }

    // 마스터 서버에 접속 성공하여 룸 접속 시도
    public void Connect()
    {
        // 중복 접속 시도를 막기 위해 접속 버튼 비활성화
        joinButton.interactable = false;

        if(PhotonNetwork.IsConnected)   // 마스터 서버에 접속이 되어있다면
        {
            connectionInfoText.text = "룸에 접속 중!";

            PhotonNetwork.JoinRandomRoom();
        }
        else
        {
            connectionInfoText.text = "오프라인 : 마스터 서버와 연결되지 않음 \n접속 재시도 중!";

            PhotonNetwork.ConnectUsingSettings();
        }
    }

    // 랜덤 룸 참가에 실패한 경우 자동 실행될 함수
    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        connectionInfoText.text = "빈 방이 없어요ㅜㅡㅜ, 새로운 방을 생성합니다!";

        // 최대 4명까지 수용 가능한 빈 방 생성
        PhotonNetwork.CreateRoom(null, new RoomOptions {MaxPlayers = 4});
    }

    // 룸에 참가 완료된 경우 자동 실행될 함수
    public override void OnJoinedRoom()
    {
        connectionInfoText.text = "방 참가 성공~!";
        PhotonNetwork.LoadLevel("01 Main");
        Debug.Log("게임 시작쓰~");
    }
}
