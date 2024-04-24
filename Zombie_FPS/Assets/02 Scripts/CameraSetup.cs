using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Cinemachine;

// �ó׸ӽ� ī�޶� ���� �÷��̾ �����ϵ��� ����
public class CameraSetup : MonoBehaviourPun
{
    void Start()
    {
        if(photonView.IsMine)   // ���� ������ �� �濡�� ���� ������ Ŭ���̾�Ʈ���(���� �����̶��)
        {
            // ���� �ִ� �ó׸ӽ� ī�޶� ��������
            CinemachineVirtualCamera followCam = FindObjectOfType<CinemachineVirtualCamera>();

            followCam.Follow = transform;
            followCam.LookAt = transform;
        }
    }
    


}
