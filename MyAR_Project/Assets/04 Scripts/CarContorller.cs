using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// 자동차 오브젝트의 Material 등을 컨트롤 할 스크립트
public class NewBehaviourScript : MonoBehaviour
{
    public GameObject[] bodyObject; // LOD(Level Of Detail) 오브젝트에 대한 배열 변수
    public Color32[] colors;        // 색상 정보를 담을 배열 변수
    public float rotSpeed = 0.1f;   // 기존 회전 속도보다 느리게 회전할 수 있도록 위해 0~1 사이의 값을 지정

    Material[] carMats;             // 각각의 오브젝트별 Material 배열 변수


    void Start()
    {
        // carMats 배열을 자동차 바디 오브젝트 수 만큼 초기화
        carMats = new Material[bodyObject.Length];

        // 자동차 바디 오브젝트의 Material을 각각 carMats에 저장
        for(int i = 0; i < carMats.Length; i++)
        {
            carMats[i] = bodyObject[i].GetComponent<MeshRenderer>().material;
        }

        // 색상 배열 0번에 Material의 초기 색상을 지정
        colors[0] = carMats[0].color;
    }

    void Update()
    {
        // 터치된 부분이 1개 이상이라면
        if(Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            // 터치 상태가 움직이고 있다면
            if(touch.phase == TouchPhase.Moved)
            {
                // 카메라 위치에서 정면으로 ray를 발사해 부딪힌 대상이 8번 레이어라면 터치 이동량을 구한다.
                Ray ray = new Ray(transform.position, transform.forward);

                RaycastHit hitInfo;

                if(Physics.Raycast(ray, out hitInfo, Mathf.Infinity, 1 << 8))
                {
                    // * deltaPosition : 직전 프레임부터 현재 프레임까지 한 프레임동안 손가락이 닿은 좌표의 이동량을 반환
                    Vector3 deltaPos = touch.deltaPosition;

                    // x축 터치 이동량에 비례하여 로컬 y축 방향으로 회전시킨다.
                    transform.Rotate(transform.up, deltaPos.x * -0.1f * rotSpeed); // -0.1을 곱하는 이유는 deltaPos.x가 좌측은 음수(-) 우즉은 양수(+)인 반면, deltaPos.y 좌회전이 양수(+) 우회전이 음수(-)이기 때문에 회전 방향에 맞춰주기 위해서
                }
            }
        }
    }

    public void ChanceColor(int num)
    {
        // 각 LOD Material 색상을 버튼에 지정된 색상으로 변경한다.
        for(int i = 0;i < carMats.Length;i++)
        {
            carMats[i].color = colors[num];
        }
    }
}
