using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotator : MonoBehaviour
{
    public float rotationSpeed = 60.0f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(0.0f, rotationSpeed * Time.deltaTime, 0.0f);   // 이동이나 회전할 때는 Time.deltaTime을 무조건 곱해준다~! : 플레이어마다 컴퓨터 사양이 다르기 때문에 누구는 빠르고 누구는 느리게 둘 수 없으니 서로 맞춰주려고 쓴다.
    }
}
