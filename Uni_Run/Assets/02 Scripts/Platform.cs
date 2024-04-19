using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platform : MonoBehaviour
{
    public GameObject[] obstacles;
    public bool stepped = false;    // �÷��� ���� ���� 

    void Start()
    {
        
    }

    void Update()
    {

    }

    private void OnEnable()
    {
        stepped = false;

        for(int i = 0; i < obstacles.Length; i++)
        {
            // Random.Range : int or float�� ��� �Է� �����ϳ�, (0, 3)�� �� int�� 0, 1, 2 ���� ����, (0.0f, 3.0f)�� �� float�� 0.0f ���� 3.0f���� ��� ������ ����
            if(Random.Range(0, 3) == 0)
            {
                obstacles[i].SetActive(true);
            }
            else
            {
                obstacles[i].SetActive(false);
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.collider.CompareTag("Player") && !stepped)
        {
            stepped = true;
            GameManager.instance.AddScore(1);
        }
    }

    // OnTriggerEnter2D�� Collider�� �ޱ� ������ collistion���� �ٷ� �±� �� ����
    //private void OnTriggerEnter2D(Collider2D collision)
    //{
    //    if(collision.CompareTag("Player") && (stepped) )
    //    {

    //    }
    //}
}
