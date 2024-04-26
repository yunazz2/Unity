using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

// 로비 매니저와 동일하지만 이건 버튼 클릭 없이 바로 룸에 들어갈 수 있도록 작성
public class LoadingManager : MonoBehaviourPunCallbacks
{
    private string gameVersion = "1";

    void Start()
    {
        PhotonNetwork.GameVersion = gameVersion;

        PhotonNetwork.ConnectUsingSettings();

        Connect();
    }

    // 마스터 서버에 접속 실패 시 자동 실행될 함수
    public override void OnDisconnected(DisconnectCause cause)
    {
        // 마스터 서버로 재접속 시도
        PhotonNetwork.ConnectUsingSettings();
    }

    // 마스터 서버에 접속 성공하여 룸 접속 시도
    public void Connect()
    {
        if (PhotonNetwork.IsConnected)   // 마스터 서버에 접속이 되어있다면
        {
            PhotonNetwork.JoinRandomRoom();
        }
        else
        {
            PhotonNetwork.ConnectUsingSettings();
        }
    }

    // 랜덤 룸 참가에 실패한 경우 자동 실행될 함수
    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        // 최대 4명까지 수용 가능한 빈 방 생성
        PhotonNetwork.CreateRoom(null, new RoomOptions { MaxPlayers = 4 });
    }

    // 룸에 참가 완료된 경우 자동 실행될 함수
    public override void OnJoinedRoom()
    {
        PhotonNetwork.LoadLevel("01 Main");
        Debug.Log("게임 시작!");
        Debug.Log("-----------------------");
    }
}
