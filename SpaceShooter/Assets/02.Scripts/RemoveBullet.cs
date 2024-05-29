using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ���� �Ѿ��� �浹���� �� �Ѿ� ����
public class RemoveBullet : MonoBehaviour
{
    // ����ũ ��ƼŬ �������� ������ ����
    public GameObject sparkEffect;

    // �浹�� ������ �� �߻��ϴ� �̺�Ʈ
    private void OnCollisionEnter(Collision collision)
    {
        // �浹�� ���� ������Ʈ�� �±� �� ��
        if(collision.collider.CompareTag("BULLET"))
        {
            // ù ��° �浹 ������ ���� ����
            ContactPoint cp = collision.GetContact(0);

            // �浹�� �Ѿ��� ���� ���͸� ���ʹϾ� Ÿ������ ��ȯ
            Quaternion rot = Quaternion.LookRotation(-cp.normal);

            // ����ũ ��ƼŬ�� �������� ����
            GameObject spark = Instantiate(sparkEffect, collision.transform.position, Quaternion.identity);

            // ���� �ð��� ���� �� ����ũ ��ƼŬ�� ����
            Destroy(spark, 0.5f);

            // �浹�� ���� ������Ʈ ����
            Destroy(collision.gameObject);
        }
    }
}
