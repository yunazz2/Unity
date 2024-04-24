using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour, IItem
{
    public int score = 200; // ���� �������� ���� �������� �츮�� ���� �� �ø��� ���� ��

    public void Use(GameObject target)
    {
        GameManager.Instance().AddScore(score); // ���� �ø���
        Destroy(gameObject);    // ��������ϱ� ���� ������Ʈ ����
    }
}
