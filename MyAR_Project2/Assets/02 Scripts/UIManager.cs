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

    // ��ư�� ������ �� ������ �Լ�
    public void ToggleMaskImage()
    {
        // faceManager ������Ʈ���� ���� ������ Face ������Ʈ�� ��� ��ȸ
        foreach(ARFace face in faceManager.trackables)
        {
            // ���� Face ������Ʈ�� ���� �ν��ϰ� �ִ� ���¶��
            if(face.trackingState == TrackingState.Tracking)
            {
                // Face ������Ʈ�� Ȱ��ȭ ���¸� �ݴ�� �����Ѵ�.
                face.gameObject.SetActive(!face.gameObject.activeSelf);
            }
        }
    }

    public void SwitchFaceMaterial(int num)
    {
        // faceManager ������Ʈ���� ���� ������ Face ������Ʈ�� ��� ��ȸ
        foreach(ARFace face in faceManager.trackables)
        {
            // ���� Face ������Ʈ�� ���� �ν��ϰ� �ִ� ���¶��
            if(face.trackingState == TrackingState.Tracking)
            {
                // Face ������Ʈ�� MeshRenderer ������Ʈ�� �����Ѵ�.
                MeshRenderer mr = face.GetComponent<MeshRenderer>();

                // ��ư�� ������ ��ȣ(image : 0 / movie : 1)�� �ش��ϴ� Material�� �����Ѵ�.
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
