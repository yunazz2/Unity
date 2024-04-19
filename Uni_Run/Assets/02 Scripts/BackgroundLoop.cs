using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundLoop : MonoBehaviour
{
    public BoxCollider2D backgroundCollider;
    private float width;


    void Awake()
    {
        backgroundCollider = GetComponent<BoxCollider2D>();
        width = backgroundCollider.size.x;
    }

    void Update()
    {
        if(transform.position.x <= -width)
        {
            Reposition();
        }
    }

    public void Reposition()
    {
        Vector2 offset = new Vector2(width * 2, 0);
        transform.position = (Vector2)transform.position + offset;   // transform�� 2D������ ������ vector3�� -> �� ��ȯ �ʿ�
    }
}
