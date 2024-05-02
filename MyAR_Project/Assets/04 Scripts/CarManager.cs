using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

// �ٴ��� �νĵ� ���� ī�޶� ���߸� Indicator ������Ʈ�� �����ǵ��� �ϴ� ��ũ��Ʈ
public class CarManager : MonoBehaviour
{
    public GameObject indicator;    // Indicator ������Ʈ�� Ȱ��ȭ�ϰų� ��ġ ���� �� �� �ֵ��� ������Ʈ�� �Ҵ� �� ����
    public GameObject myCar;
    public float relocationDistance = 1.0f; // ����ڰ� ȭ���� ��ġ���� �� �𵨸��� ���� ��ġ�� ���� ��ġ�� ��ġ���� �Ÿ��� �����Ͽ� ���� �Ÿ� �̻� �������߸� ���ġ �� �� �ֵ��� �ϱ� ���� ����(1m �̻� �������� ���ġ �����ϵ���)

    ARRaycastManager arManager;
    GameObject placeObject; // �ڵ��� ������Ʈ �ߺ� ���� ������ ���� ����

    void Start()
    {
        // AR ī�޶� �ٴ��� �ν��� ��쿡�� ǥ�õǾ�� �ϹǷ� ó���� ��Ȱ��ȭ
        indicator.SetActive(false);

        // AR Raycast Manager ������Ʈ�� ������ �Ҵ�
        arManager = GetComponent<ARRaycastManager>();
    }

    void Update()
    {
        // �� �����Ӹ��� �ٴ� ������ �Ǵ��� ���ο� ���� �ٲ��� �ϹǷ� Update������ ȣ��
        DetectGround();

        // ���� Ŭ�� �Ǵ� ��ġ�� ������Ʈ�� UI ������Ʈ��� Update() �Լ��� �����Ѵ�.
        if (EventSystem.current.currentSelectedGameObject)
            return;

        // Indicator�� Ȱ��ȭ ���̸鼭 ȭ�� ��ġ�� �ִ� ���¶��
        if (indicator.activeInHierarchy && Input.touchCount > 0)
        {
            // ù ��° ��ġ ���¸� ���� ��
            Touch touch = Input.GetTouch(0);

            // ��ġ�� ���۵� ���¶��, �ڵ����� Indicator�� ������ ��ġ�� �����Ѵ�.
            if(touch.phase == TouchPhase.Began)
            {
                // �����Ǿ��ִ� �ڵ��� ������Ʈ�� ���ٸ�
                if(placeObject == null)
                {
                    // �ڵ��� �������� �����ϰ� placeObject�� �Ҵ�
                    placeObject = Instantiate(myCar, indicator.transform.position, indicator.transform.rotation);
                }
                // ������ �����Ǿ��ִ� �ڵ��� ������Ʈ�� �ִٸ� �� ������Ʈ�� ��ġ�� ȸ�� ���� �����Ѵ�.
                else
                {
                    // ������ ������Ʈ�� Indicator ������ �Ÿ��� �ּ� �̵� ���� �̻��̶��
                    if(Vector3.Distance(placeObject.transform.position, indicator.transform.position) > relocationDistance)
                    {
                        placeObject.transform.SetPositionAndRotation(indicator.transform.position, indicator.transform.rotation);
                    }


                }
            }
        }
    }

    // �ٴ� ���� �� Indicator ǥ�ø� ���� �Լ�
    void DetectGround()
    {
        // * Screen Ŭ���� : ���÷����� ��ġ�� ����, ���� ũ�⸦ ����
        Vector2 screenSize = new Vector2(Screen.width * 0.5f, Screen.height * 0.5f);    // ������ ���÷����� ���� �� ���� ũ�⿡ 0.5�� ���ϸ� ���߾� ��ġ�� �� �� �ִ�.

        // ray�� �ε��� ������ ������ ������ ����Ʈ ����
        List<ARRaycastHit> hitInfos = new List<ARRaycastHit>();

        // ��ũ�� ���߾ӿ��� ray�� �߻����� �� Plane Ÿ�� ���� ����� �ִٸ�,
        if(arManager.Raycast(screenSize, hitInfos, TrackableType.Planes))   // ray�� �����ؾ��� ����� �ٴ��̹Ƿ�, TrackableType.Planes�� ����
        {
            // Indicator ������Ʈ�� Ȱ��ȭ�Ѵ�.
            indicator.SetActive(true);

            // Indicator�� ��ġ �� ȸ�� ���� ray�� ���� ������ ��ġ��Ų��.
            indicator.transform.position = hitInfos[0].pose.position;
            indicator.transform.rotation= hitInfos[0].pose.rotation;

            // Indicator�� �ٴڰ� ��ġ�� �ʵ��� ���� �������� 0.1m �ø���.
            indicator.transform.position += indicator.transform.up * 0.1f;  // Vector3�� �ƴ� transform ��� ������ ������ ����� ���� ���� �ֱ� ������ ������ rotation ���¿� ���� ������ ���� �������� ���� ��.
        }
        // ���ٸ�
        else
        {
            // Indicator ������Ʈ�� ��Ȱ��ȭ�Ѵ�.
            indicator.SetActive(false);
        }
    }
}
