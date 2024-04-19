using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ������(enum)�� ���⿡ ���� ����

public class Bullet : MonoBehaviour // MonoBehaviour�� ����� �ްڴ�.
{
    // ������ Ŭ���� ���� ����

    public float speed = 10.0f;
    private Rigidbody bulletRb;  // Rigidbody�� ���� ������Ʈ�ε� Ŭ���� ������ ���������� ��� �����ϴ�.

    // Start is called before the first frame update
    void Start()    // �̺�Ʈ �Լ� : �� ��ũ��Ʈ�� ����� �� ���ʿ� �� �� �����ϰڴ�.
    {
        Debug.Log("����~!");
        bulletRb = GetComponent<Rigidbody>();

        // �ӵ�
        bulletRb.velocity = transform.forward * speed;
        // bulletRb.velocity = new Vector3(0, 0, 1) * speed;   // �� ���̶� ���� ����. ���� �� ���� ����

        Destroy(gameObject, 3.0f);

    }

 

    // Update is called once per frame
    void Update()    // �̺�Ʈ �Լ� : ������ ������, �������� �ٲ� ������ ����
    {
        Debug.Log("������Ʈ~!");
    }

    // �浹 ó��
    //private void OnCollisionEnter(Collision collision)  // �浹�޾��� �� ó��
    //{
    //    if (collision.collider.tag == "Player") // �浹�� ���� �±װ� Player���
    //    {

    //    }
    //}

    //private void OnCollisionStay(Collision collision)   // �浹�ϴ� ���� ���
    //{
        
    //}

    //private void OnCollisionExit(Collision collision)   // �浹�ߴٰ� �������� ����
    //{
        
    //}

    // Ʈ���� ó��
    private void OnTriggerEnter(Collider other) // 
    {
        // �Ʒ�ó�� ���ڿ� ��ü�� ���ϱ⺸��
        //if(other.tag == "Player") { }

        // �̷��� ���� �� ����ȭ�� ����
        if(other.CompareTag("Player")) {    // Player��� �±׸� ���� �ֶ� �浹�Ѵٸ�
            PlayerController playerController = other.GetComponent<PlayerController>();

            if(playerController != null)
            {
                playerController.Die(); // ���̰ڴ�.
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