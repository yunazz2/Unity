using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 벽에 총알이 충돌했을 때 총알 삭제
public class RemoveBullet : MonoBehaviour
{
    // 스파크 파티클 프리팹을 연결할 변수
    public GameObject sparkEffect;

    // 충돌이 시작할 떄 발생하는 이벤트
    private void OnCollisionEnter(Collision collision)
    {
        // 충돌한 게임 오브젝트의 태그 값 비교
        if(collision.collider.CompareTag("BULLET"))
        {
            // 첫 번째 충돌 지점의 정보 추출
            ContactPoint cp = collision.GetContact(0);

            // 충돌한 총알의 법선 벡터를 쿼터니언 타입으로 변환
            Quaternion rot = Quaternion.LookRotation(-cp.normal);

            // 스파크 파티클을 동적으로 생성
            GameObject spark = Instantiate(sparkEffect, collision.transform.position, Quaternion.identity);

            // 일정 시간이 지난 후 스파크 파티클을 삭제
            Destroy(spark, 0.5f);

            // 충돌한 게임 오브젝트 삭제
            Destroy(collision.gameObject);
        }
    }
}
