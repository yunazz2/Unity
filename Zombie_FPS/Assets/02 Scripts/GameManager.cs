using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // �̱��� �������� GameManager �ν��Ͻ� �ۼ� : �ٸ� ��ũ��Ʈ���� ������ ������ �̷��� �ۼ� ��
    private static GameManager instance;
    public static GameManager Instance()
    {
        return instance;
    } // �ٵ� �̷��� �� �� �� ���δ� ������ �� �ν��Ͻ��� ���� ���� �޾ư����ϴµ� �ȿ� ���� �ٲ����� ���ϵ��� ��ȣ�ϱ� ����

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
        // livingEntity�� �÷��̾�� ���� ��� ����ϴ°ǵ�, ���� �״´ٰ� ���� ������ �Ǹ� �ȵǴϱ� �÷��̾ �׾��� ���� ���� ������ �ǵ���
        // PlayerHealth�� onDeath�� Action��(�Լ��� ������ �� �ִ� ����)�� EndGame�� �����س��� �÷��̾ ������ Die�� ����Ǿ� onDeath��
        // �̾ ���� ��Ų��.
        FindObjectOfType<PlayerHealth>().onDeath += EndGame;
    }

    // ���� ���� �Լ�
    public void AddScore(int newScore)
    {
        // ���� �߰��ϰ� ui ����
        if(!isGameOver)
        {
            score += newScore;

            UIManager.Instance().UpdateScoreText(score);
        }
        
    }

    public void EndGame()
    {
        // ���� ���� ó��
        isGameOver = true;
        UIManager.Instance().SetActiveGameOverUI(true);
    }
}
