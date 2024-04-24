using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Cinemachine;

// 시네머신 카메라가 로컬 플레이어를 추적하도록 설정
public class CameraSetup : MonoBehaviourPun
{
    void Start()
    {
        if(photonView.IsMine)   // 지금 생성된 이 방에서 내가 마스터 클라이언트라면(내가 방장이라면)
        {
            // 씬에 있는 시네머신 카메라 가져오기
            CinemachineVirtualCamera followCam = FindObjectOfType<CinemachineVirtualCamera>();

            followCam.Follow = transform;
            followCam.LookAt = transform;
        }
    }
    


}
