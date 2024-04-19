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
        float xInput = Input.GetAxis("Horizontal"); // ���� ���� �ްڴ�.
        float yInput = Input.GetAxis("Vertical");   // ���� ���� �ްڴ�.

        float xSpeed = xInput * speed;
        float zSpeed = yInput * speed;

        Vector3 _velocity = new Vector3(xSpeed, 0.0f, zSpeed);
        playerRd.velocity = _velocity;

        

        // GetKey�� True False�� ���� ���� ���� : ���� ����
        //if(Input.GetKey(KeyCode.UpArrow))   // �� ȭ��ǥ�� �����°� ��� �����ϰڴ�.
        //{
        //    Debug.Log("�� ȭ��ǥ~");
        //    playerRd.AddForce(0, 0, speed); // AddForce : �����̰� ���� �������� ���� �ְڴ�.
        //}
        //if(Input.GetKey(KeyCode.DownArrow))   // �Ʒ� ȭ��ǥ�� �����°� ��� �����ϰڴ�.
        //{
        //    Debug.Log("�Ʒ� ȭ��ǥ~");
        //    playerRd.AddForce(0, 0, -speed);
        //}
        //if(Input.GetKey(KeyCode.LeftArrow))   // ���� ȭ��ǥ�� �����°� ��� �����ϰڴ�.
        //{
        //    Debug.Log("���� ȭ��ǥ~");
        //    playerRd.AddForce(-speed, 0, 0);
        //}
        //if(Input.GetKey(KeyCode.RightArrow))   // ������ ȭ��ǥ�� �����°� ��� �����ϰڴ�.
        //{
        //    Debug.Log("������ ȭ��ǥ~");
        //    playerRd.AddForce(speed, 0, 0);
        //}
        
    }

    public void Die()
    {
        // setActive : Ȱ��ȭ / ��Ȱ��ȭ
        gameObject.SetActive(false);    // gameObject�� GameObject�� �ٸ�.
        GameManager.Instance().EndGame();
    }
}
