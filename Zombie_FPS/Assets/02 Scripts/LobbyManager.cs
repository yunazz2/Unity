using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;

// �������� ������ �� ���� ����ϴ� ��ũ��Ʈ
public class LobbyManager : MonoBehaviourPunCallbacks   // ���� ���� �̿��� ���� �׳� MonoBehaviour�� �ƴ� MonoBehaviourPunCallbacks ���
{
    private string gameVersion = "1";

    public Text connectionInfoText;
    public Button joinButton;

    void Start()
    {
        // ������ �Ʒ� �� ���� ��� ���� ����� �����ϴ�.
        // ���ӿ� �ʿ��� ���� ����
        PhotonNetwork.GameVersion = gameVersion;
        // ������ ������ ������ ������ ���� �õ�
        PhotonNetwork.ConnectUsingSettings();

        // �� ���� ��ư�� ��� ��Ȱ��ȭ
        joinButton.interactable = false;

        // ���� �õ� ������ �ؽ�Ʈ�� ǥ��
        connectionInfoText.text = "������ ������ ���� ��!";
    }

    // ������ ������ ���� ���� �� �ڵ� ����� �Լ�
    public override void OnConnectedToMaster()
    {
        // ���� ���ӿ� ���������� �� ���� ��ư�� �ٽ� Ȱ��ȭ
        joinButton.interactable = true;
        connectionInfoText.text = "�¶��� : ������ ������ ���� ��";
    }

    // ������ ������ ���� ���� �� �ڵ� ����� �Լ�
    public override void OnDisconnected(DisconnectCause cause)
    {
        joinButton.interactable = false;
        connectionInfoText.text = "�������� : ������ ������ ���� ���� ���� \n���� ��õ� ��!";

        // ������ ������ ������ �õ�
        PhotonNetwork.ConnectUsingSettings();
    }

    // ������ ������ ���� �����Ͽ� �� ���� �õ�
    public void Connect()
    {
        // �ߺ� ���� �õ��� ���� ���� ���� ��ư ��Ȱ��ȭ
        joinButton.interactable = false;

        if(PhotonNetwork.IsConnected)   // ������ ������ ������ �Ǿ��ִٸ�
        {
            connectionInfoText.text = "�뿡 ���� ��!";

            PhotonNetwork.JoinRandomRoom();
        }
        else
        {
            connectionInfoText.text = "�������� : ������ ������ ������� ���� \n���� ��õ� ��!";

            PhotonNetwork.ConnectUsingSettings();
        }
    }

    // ���� �� ������ ������ ��� �ڵ� ����� �Լ�
    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        connectionInfoText.text = "�� ���� �����̤Ѥ�, ���ο� ���� �����մϴ�!";

        // �ִ� 4����� ���� ������ �� �� ����
        PhotonNetwork.CreateRoom(null, new RoomOptions {MaxPlayers = 4});
    }

    // �뿡 ���� �Ϸ�� ��� �ڵ� ����� �Լ�
    public override void OnJoinedRoom()
    {
        connectionInfoText.text = "�� ���� ����~!";
        PhotonNetwork.LoadLevel("01 Main");
        Debug.Log("���� ���۾�~");
    }
}
