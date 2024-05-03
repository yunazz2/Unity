using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class NewBehaviourScript : MonoBehaviour
{
    public ARFaceManager faceManager;
    public Material[] faceMats;

    // 버튼을 눌렀을 때 실행할 함수
    public void ToggleMaskImage()
    {
        // faceManager 컴포넌트에서 현재 생성된 Face 오브젝트를 모두 순회
        foreach(ARFace face in faceManager.trackables)
        {
            // 만일 Face 오브젝트가 얼굴을 인식하고 있는 상태라면
            if(face.trackingState == TrackingState.Tracking)
            {
                // Face 오브젝트의 활성화 상태를 반대로 변경한다.
                face.gameObject.SetActive(!face.gameObject.activeSelf);
            }
        }
    }

    public void SwitchFaceMaterial(int num)
    {
        // faceManager 컴포넌트에서 현재 생성된 Face 오브젝트를 모두 순회
        foreach(ARFace face in faceManager.trackables)
        {
            // 만일 Face 오브젝트가 얼굴을 인식하고 있는 상태라면
            if(face.trackingState == TrackingState.Tracking)
            {
                // Face 오브젝트의 MeshRenderer 컴포넌트에 접근한다.
                MeshRenderer mr = face.GetComponent<MeshRenderer>();

                // 버튼에 설정된 번호(image : 0 / movie : 1)에 해당하는 Material로 변경한다.
                mr.material = faceMats[num];
            }
        }
    }

    void Start()
    {
        
    }

    void Update()
    {
        
    }
}
