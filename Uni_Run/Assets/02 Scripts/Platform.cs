using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platform : MonoBehaviour
{
    public GameObject[] obstacles;
    public bool stepped = false;    // 플랫폼 밟음 여부 

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
            // Random.Range : int or float형 모두 입력 가능하나, (0, 3)일 때 int는 0, 1, 2 값을 갖고, (0.0f, 3.0f)일 때 float는 0.0f 부터 3.0f까지 모두 가지기 가능
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

    // OnTriggerEnter2D는 Collider를 받기 때문에 collistion에서 바로 태그 비교 가능
    //private void OnTriggerEnter2D(Collider2D collision)
    //{
    //    if(collision.CompareTag("Player") && (stepped) )
    //    {

    //    }
    //}
}
