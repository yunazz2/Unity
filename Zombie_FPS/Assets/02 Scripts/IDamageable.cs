using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamageable
{
    // ��� �÷��̾��� ������ �� �������̽�
    
    // ���� �Ӹ��� �¾Ҵµ� �߿��� �ǰ� ���� �ȵǴϱ� ���� ��ġ�� �����Ϸ���
    // �׸��� ���� �տ��� �¾����� �ǰ� �ڷ� Ƣ����ϴµ� ������ Ƣ�� �ȵǴϱ� �׷� �͵� �����ַ���
    void onDamage(float damage, Vector3 hitPoint, Vector3 hitNormal);


}
