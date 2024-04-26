using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

// �κ� �Ŵ����� ���������� �̰� ��ư Ŭ�� ���� �ٷ� �뿡 �� �� �ֵ��� �ۼ�
public class LoadingManager : MonoBehaviourPunCallbacks
{
    private string gameVersion = "1";

    void Start()
    {
        PhotonNetwork.GameVersion = gameVersion;

        PhotonNetwork.ConnectUsingSettings();

        Connect();
    }

    // ������ ������ ���� ���� �� �ڵ� ����� �Լ�
    public override void OnDisconnected(DisconnectCause cause)
    {
        // ������ ������ ������ �õ�
        PhotonNetwork.ConnectUsingSettings();
    }

    // ������ ������ ���� �����Ͽ� �� ���� �õ�
    public void Connect()
    {
        if (PhotonNetwork.IsConnected)   // ������ ������ ������ �Ǿ��ִٸ�
        {
            PhotonNetwork.JoinRandomRoom();
        }
        else
        {
            PhotonNetwork.ConnectUsingSettings();
        }
    }

    // ���� �� ������ ������ ��� �ڵ� ����� �Լ�
    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        // �ִ� 4����� ���� ������ �� �� ����
        PhotonNetwork.CreateRoom(null, new RoomOptions { MaxPlayers = 4 });
    }

    // �뿡 ���� �Ϸ�� ��� �ڵ� ����� �Լ�
    public override void OnJoinedRoom()
    {
        PhotonNetwork.LoadLevel("01 Main");
        Debug.Log("���� ����!");
        Debug.Log("-----------------------");
    }
}
