using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ���� �Ѿ��� �浹���� �� �Ѿ� ����
public class RemoveBullet : MonoBehaviour
{
    // �浹�� ������ �� �߻��ϴ� �̺�Ʈ
    private void OnCollisionEnter(Collision collision)
    {
        // �浹�� ���� ������Ʈ�� �±� �� ��
        if(collision.collider.CompareTag("BULLET"))
        {
            // �浹�� ���� ������Ʈ ����
            Destroy(collision.gameObject);
        }
    }
}
