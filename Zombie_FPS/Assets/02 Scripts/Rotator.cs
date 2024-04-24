using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 아이템이 가만히 있으면 재미없으니까 빙글빙글 돌아가는 효과를 주기 위한 스크립트
public class Rotator : MonoBehaviour
{
    public float rotationSpeed = 60.0f;

    void Update()
    {
        transform.Rotate(0.0f, rotationSpeed * Time.deltaTime, 0.0f);    
    }
}
