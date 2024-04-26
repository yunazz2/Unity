using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviourPunCallbacks, IPunObservable
{
    // �̱��� �������� GameManager �ν��Ͻ� �ۼ� : �ٸ� ��ũ��Ʈ���� ������ ������ �̷��� �ۼ� ��
    private static GameManager instance;
    public static GameManager Instance()
    {
        return instance;
    } // �ٵ� �̷��� �� �� �� ���δ� ������ �� �ν��Ͻ��� ���� ���� �޾ư����ϴµ� �ȿ� ���� �ٲ����� ���ϵ��� ��ȣ�ϱ� ����


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

        // livingEntity�� �÷��̾�� ���� ��� ����ϴ°ǵ�, ���� �״´ٰ� ���� ������ �Ǹ� �ȵǴϱ� �÷��̾ �׾��� ���� ���� ������ �ǵ���
        // PlayerHealth�� onDeath�� Action��(�Լ��� ������ �� �ִ� ����)�� EndGame�� �����س��� �÷��̾ ������ Die�� ����Ǿ� onDeath��
        // �̾ ���� ��Ų��.
        //FindObjectOfType<PlayerHealth>().onDeath += EndGame;
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))    // ESC ��ư ������ ���� �� �ֵ���
        {
            PhotonNetwork.LeaveRoom();
        }
    }

    // ���� ����
    public void AddScore(int newScore)
    {
        // ���� �߰��ϰ� ui ����
        if(!isGameOver)
        {
            score += newScore;

            UIManager.Instance().UpdateScoreText(score);
        }
    }

    // ���� ���� ó��
    public void EndGame()
    {
        isGameOver = true;
        UIManager.Instance().SetActiveGameOverUI(true);
    }

    // �κ�� ������
    public void BackToLobby()
    {
        Debug.Log("�κ�� ������!");
        PhotonNetwork.LeaveRoom();
    }

    // ���� �����
    public void GameRestart()
    {
        Debug.Log("���� �����~");

        StartCoroutine(DelayedInvoke()); // �ڷ�ƾ�� ���� ���� ����
    }

    IEnumerator DelayedInvoke()
    {
        yield return new WaitForSeconds(0.5f);
     
        SceneManager.LoadScene(2);  // �� �� �ε�
        Debug.Log("������?");
        Debug.Log("���� ���� ����: " + PhotonNetwork.NetworkClientState);
        SceneManager.LoadScene(1);
    }

    // ���� ���� �� �ڵ����� ����Ǵ� �Լ�
    public override void OnLeftRoom()
    {
        SceneManager.LoadScene("00 Lobby");
    }
}
