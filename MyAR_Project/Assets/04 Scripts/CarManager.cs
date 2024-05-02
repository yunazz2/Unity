using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

// 바닥이 인식된 곳에 카메라를 비추면 Indicator 오브젝트가 생성되도록 하는 스크립트
public class CarManager : MonoBehaviour
{
    public GameObject indicator;    // Indicator 오브젝트를 활성화하거나 위치 조절 할 수 있도록 오브젝트를 할당 할 변수
    public GameObject myCar;
    public float relocationDistance = 1.0f; // 사용자가 화면을 터치했을 때 모델링의 기존 위치와 새로 배치될 위치간의 거리를 측정하여 일정 거리 이상 떨어져야만 재배치 될 수 있도록 하기 위한 변수(1m 이상 떨어져야 재배치 가능하도록)

    ARRaycastManager arManager;
    GameObject placeObject; // 자동차 오브젝트 중복 생성 방지를 위한 변수

    void Start()
    {
        // AR 카메라가 바닥을 인식할 경우에만 표시되어야 하므로 처음엔 비활성화
        indicator.SetActive(false);

        // AR Raycast Manager 컴포넌트를 가져와 할당
        arManager = GetComponent<ARRaycastManager>();
    }

    void Update()
    {
        // 매 프레임마다 바닥 감지가 되는지 여부에 따라 바뀌어야 하므로 Update문에서 호출
        DetectGround();

        // 현재 클릭 또는 터치한 오브젝트가 UI 오브젝트라면 Update() 함수를 종료한다.
        if (EventSystem.current.currentSelectedGameObject)
            return;

        // Indicator가 활성화 중이면서 화면 터치가 있는 상태라면
        if (indicator.activeInHierarchy && Input.touchCount > 0)
        {
            // 첫 번째 터치 상태를 가져 옴
            Touch touch = Input.GetTouch(0);

            // 터치가 시작된 상태라면, 자동차를 Indicator와 동일한 위치에 생성한다.
            if(touch.phase == TouchPhase.Began)
            {
                // 생성되어있는 자동차 오브젝트가 없다면
                if(placeObject == null)
                {
                    // 자동차 프리팹을 생성하고 placeObject에 할당
                    placeObject = Instantiate(myCar, indicator.transform.position, indicator.transform.rotation);
                }
                // 기존에 생성되어있는 자동차 오브젝트가 있다면 그 오브젝트의 위치와 회전 값을 변경한다.
                else
                {
                    // 생성된 오브젝트와 Indicator 사이의 거리가 최소 이동 범위 이상이라면
                    if(Vector3.Distance(placeObject.transform.position, indicator.transform.position) > relocationDistance)
                    {
                        placeObject.transform.SetPositionAndRotation(indicator.transform.position, indicator.transform.rotation);
                    }


                }
            }
        }
    }

    // 바닥 감지 및 Indicator 표시를 위한 함수
    void DetectGround()
    {
        // * Screen 클래스 : 디스플레이의 장치의 가로, 세로 크기를 제공
        Vector2 screenSize = new Vector2(Screen.width * 0.5f, Screen.height * 0.5f);    // 제공된 디스플레이의 가로 및 세로 크기에 0.5씩 곱하면 정중앙 위치를 알 수 있다.

        // ray에 부딪힌 대상들의 정보를 저장할 리스트 변수
        List<ARRaycastHit> hitInfos = new List<ARRaycastHit>();

        // 스크린 정중앙에서 ray를 발사했을 때 Plane 타입 추적 대상이 있다면,
        if(arManager.Raycast(screenSize, hitInfos, TrackableType.Planes))   // ray가 감지해야할 대상은 바닥이므로, TrackableType.Planes로 지정
        {
            // Indicator 오브젝트를 활성화한다.
            indicator.SetActive(true);

            // Indicator의 위치 및 회전 값을 ray가 닿은 지점에 일치시킨다.
            indicator.transform.position = hitInfos[0].pose.position;
            indicator.transform.rotation= hitInfos[0].pose.rotation;

            // Indicator가 바닥과 겹치지 않도록 위쪽 방향으로 0.1m 올린다.
            indicator.transform.position += indicator.transform.up * 0.1f;  // Vector3가 아닌 transform 사용 이유는 기울어진 평면이 있을 수도 있기 때문에 현재의 rotation 상태에 맞춰 로컬의 위쪽 방향으로 높인 것.
        }
        // 없다면
        else
        {
            // Indicator 오브젝트를 비활성화한다.
            indicator.SetActive(false);
        }
    }
}
