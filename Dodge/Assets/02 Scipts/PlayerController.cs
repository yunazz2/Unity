using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Rigidbody playerRd;
    public float speed = 8.0f;

    // Start is called before the first frame update
    void Start()
    {
        playerRd = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        float xInput = Input.GetAxis("Horizontal"); // 수평 값을 받겠다.
        float yInput = Input.GetAxis("Vertical");   // 수직 값을 받겠다.

        float xSpeed = xInput * speed;
        float zSpeed = yInput * speed;

        Vector3 _velocity = new Vector3(xSpeed, 0.0f, zSpeed);
        playerRd.velocity = _velocity;

        

        // GetKey는 True False에 대한 값만 받음 : 누름 여부
        //if(Input.GetKey(KeyCode.UpArrow))   // 위 화살표를 누르는걸 계속 감지하겠다.
        //{
        //    Debug.Log("위 화살표~");
        //    playerRd.AddForce(0, 0, speed); // AddForce : 움직이고 싶은 방향으로 힘을 주겠다.
        //}
        //if(Input.GetKey(KeyCode.DownArrow))   // 아래 화살표를 누르는걸 계속 감지하겠다.
        //{
        //    Debug.Log("아래 화살표~");
        //    playerRd.AddForce(0, 0, -speed);
        //}
        //if(Input.GetKey(KeyCode.LeftArrow))   // 왼쪽 화살표를 누르는걸 계속 감지하겠다.
        //{
        //    Debug.Log("왼쪽 화살표~");
        //    playerRd.AddForce(-speed, 0, 0);
        //}
        //if(Input.GetKey(KeyCode.RightArrow))   // 오른쪽 화살표를 누르는걸 계속 감지하겠다.
        //{
        //    Debug.Log("오른쪽 화살표~");
        //    playerRd.AddForce(speed, 0, 0);
        //}
        
    }

    public void Die()
    {
        // setActive : 활성화 / 비활성화
        gameObject.SetActive(false);    // gameObject는 GameObject랑 다름.
        GameManager.Instance().EndGame();
    }
}
