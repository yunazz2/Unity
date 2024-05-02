using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 스마트폰 절전 방지를 위한 스크립트
public class NonSleepMode : MonoBehaviour
{
    void Start()
    {
        // 앱 실행 중에는 절전 모드로 전환되지 않도록
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
    }

    void Update()
    {
        
    }
}
