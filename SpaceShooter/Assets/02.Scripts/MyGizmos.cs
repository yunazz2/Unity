using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyGizmos : MonoBehaviour
{
    public Color _color = Color.yellow;
    public float _radius = 0.1f;

    private void OnDrawGizmos()
    {
        // ����� ���� ����
        Gizmos.color = _color;

        // ��ü ����� ����� ����
        // * DrawSphere(���� ��ġ, ������);
        Gizmos.DrawSphere(transform.position, _radius);
    }
}
