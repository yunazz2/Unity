using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 열거형(enum)은 여기에 선언 가능

public class Bullet : MonoBehaviour // MonoBehaviour의 상속을 받겠다.
{
    // 변수는 클래스 내에 선언

    public float speed = 10.0f;
    private Rigidbody bulletRb;  // Rigidbody는 원래 컴포넌트인데 클래스 내에서 변수형으로 사용 가능하다.

    // Start is called before the first frame update
    void Start()    // 이벤트 함수 : 이 스크립트가 실행될 때 최초에 한 번 실행하겠다.
    {
        Debug.Log("시작~!");
        bulletRb = GetComponent<Rigidbody>();

        // 속도
        bulletRb.velocity = transform.forward * speed;
        // bulletRb.velocity = new Vector3(0, 0, 1) * speed;   // 윗 줄이랑 같은 말임. 위가 더 간략 버전

        Destroy(gameObject, 3.0f);

    }

 

    // Update is called once per frame
    void Update()    // 이벤트 함수 : 프레임 단위로, 프레임이 바뀔 때마다 실행
    {
        Debug.Log("업데이트~!");
    }

    // 충돌 처리
    //private void OnCollisionEnter(Collision collision)  // 충돌받았을 때 처음
    //{
    //    if (collision.collider.tag == "Player") // 충돌한 애의 태그가 Player라면
    //    {

    //    }
    //}

    //private void OnCollisionStay(Collision collision)   // 충돌하는 동안 계속
    //{
        
    //}

    //private void OnCollisionExit(Collision collision)   // 충돌했다가 떨어지는 순간
    //{
        
    //}

    // 트리거 처리
    private void OnTriggerEnter(Collider other) // 
    {
        // 아래처럼 문자열 자체를 비교하기보다
        //if(other.tag == "Player") { }

        // 이렇게 쓰는 게 최적화에 좋다
        if(other.CompareTag("Player")) {    // Player라는 태그를 가진 애랑 충돌한다면
            PlayerController playerController = other.GetComponent<PlayerController>();

            if(playerController != null)
            {
                playerController.Die(); // 죽이겠다.
            }
        }
    }

    private void OnTriggerStay(Collider other)  // 
    {

    }

    private void OnTriggerExit(Collider other)  // 
    {
        
    }



}