using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    // 플레이어가 이동할 속력
    public float speed = 5;
    void Update()
    {
        float h = Input.GetAxis("Horizontal");  // 수평 입력 값 - 좌 : -1, 우 : 1
        float v = Input.GetAxis("Vertical");    // 수직 입력 값 - 상 : 1, 하 : -1

        Vector3 dir = Vector3.right * h + Vector3.up * v;

        transform.Translate(dir * speed * Time.deltaTime);
    }
}
