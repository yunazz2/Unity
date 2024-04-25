using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Photon.Pun;

public class UIManager : MonoBehaviour
{
    // �̱��� ����
    private static UIManager instance;
    public static UIManager Instance()
    {
        return instance;
    }

    public Text ammoText;   // ź�� ��
    public Text scoreText;  // ����
    public Text waveText;   // ���̺�

    public GameObject gameOverUI;


    void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
    }

    // ź�� �� ���� �Լ�
    public void UpdateAmmoText(int magAmmo, int remainAmmo)
    {
        //ammoText.text = magAmmo + "/" + remainAmmo;   // �Ʒ��� ���� ������
        ammoText.text = string.Format("{0} / {1}", magAmmo, remainAmmo);
    }

    // ���� ���� �Լ�
    public void UpdateScoreText(int newScore)
    {
        scoreText.text = string.Format("Score : {0:000}", newScore);
    }

    // �� ���̺� �ؽ�Ʈ ����
    public void UpdateWaveText(int waves, int count)
    {
        //waveText.text = string.Format("wave : {0:0}, enemy : {0:1}", waves, count);   // ���� �ۼ��� ���
        waveText.text = $"Wave : {waves} \nEnemy : {count}";    // �Ʒ��� �������� �ۼ��� ���
    }

    // ���� ���� UI Ȱ��ȭ
    public void SetActiveGameOverUI(bool active)
    {
        gameOverUI.SetActive(active);
    }

    // ���� �����
    public void GameRestart()
    {
        Debug.Log("���� �����~");
        SceneManager.LoadScene(2);  // �� �� �ε�
        Debug.Log("���� ���� ����: " + PhotonNetwork.NetworkClientState);
        SceneManager.LoadScene(1);
    }
}
