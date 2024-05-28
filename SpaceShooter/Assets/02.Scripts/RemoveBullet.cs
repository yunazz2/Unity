using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 벽에 총알이 충돌했을 때 총알 삭제
public class RemoveBullet : MonoBehaviour
{
    // 충돌이 시작할 떄 발생하는 이벤트
    private void OnCollisionEnter(Collision collision)
    {
        // 충돌한 게임 오브젝트의 태그 값 비교
        if(collision.collider.CompareTag("BULLET"))
        {
            // 충돌한 게임 오브젝트 삭제
            Destroy(collision.gameObject);
        }
    }
}
