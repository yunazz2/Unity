using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// �ڵ��� ������Ʈ�� Material ���� ��Ʈ�� �� ��ũ��Ʈ
public class NewBehaviourScript : MonoBehaviour
{
    public GameObject[] bodyObject; // LOD(Level Of Detail) ������Ʈ�� ���� �迭 ����
    public Color32[] colors;        // ���� ������ ���� �迭 ����
    public float rotSpeed = 0.1f;   // ���� ȸ�� �ӵ����� ������ ȸ���� �� �ֵ��� ���� 0~1 ������ ���� ����

    Material[] carMats;             // ������ ������Ʈ�� Material �迭 ����


    void Start()
    {
        // carMats �迭�� �ڵ��� �ٵ� ������Ʈ �� ��ŭ �ʱ�ȭ
        carMats = new Material[bodyObject.Length];

        // �ڵ��� �ٵ� ������Ʈ�� Material�� ���� carMats�� ����
        for(int i = 0; i < carMats.Length; i++)
        {
            carMats[i] = bodyObject[i].GetComponent<MeshRenderer>().material;
        }

        // ���� �迭 0���� Material�� �ʱ� ������ ����
        colors[0] = carMats[0].color;
    }

    void Update()
    {
        // ��ġ�� �κ��� 1�� �̻��̶��
        if(Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            // ��ġ ���°� �����̰� �ִٸ�
            if(touch.phase == TouchPhase.Moved)
            {
                // ī�޶� ��ġ���� �������� ray�� �߻��� �ε��� ����� 8�� ���̾��� ��ġ �̵����� ���Ѵ�.
                Ray ray = new Ray(transform.position, transform.forward);

                RaycastHit hitInfo;

                if(Physics.Raycast(ray, out hitInfo, Mathf.Infinity, 1 << 8))
                {
                    // * deltaPosition : ���� �����Ӻ��� ���� �����ӱ��� �� �����ӵ��� �հ����� ���� ��ǥ�� �̵����� ��ȯ
                    Vector3 deltaPos = touch.deltaPosition;

                    // x�� ��ġ �̵����� ����Ͽ� ���� y�� �������� ȸ����Ų��.
                    transform.Rotate(transform.up, deltaPos.x * -0.1f * rotSpeed); // -0.1�� ���ϴ� ������ deltaPos.x�� ������ ����(-) ������ ���(+)�� �ݸ�, deltaPos.y ��ȸ���� ���(+) ��ȸ���� ����(-)�̱� ������ ȸ�� ���⿡ �����ֱ� ���ؼ�
                }
            }
        }
    }

    public void ChanceColor(int num)
    {
        // �� LOD Material ������ ��ư�� ������ �������� �����Ѵ�.
        for(int i = 0;i < carMats.Length;i++)
        {
            carMats[i].color = colors[num];
        }
    }
}
