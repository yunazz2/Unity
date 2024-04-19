using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 플레이어의 입력을 감지할 스크립트
public class PlayerInput : MonoBehaviour
{
    public string moveAxisName = "Vertical";        // 플레이어를 앞 뒤로 이동
    public string rotateAxisName = "Horizontal";    // 플레이어를 좌 우로 회전
    public string fireButtonName = "Fire1";         // 총 발사
    public string reloadButtonName = "Reload";      // 재장전

    // 프로퍼티
    // 각 입력에 대한 값을 저장하며, 외부에서 수정은 불가능하다.
    public float move { get; private set; }
    public float rotate { get; private set; }
    public bool fire { get; private set; }
    public bool reload {  get; private set; }

    void Start()
    {
    }

    // 프레임마다 호출되는 함수로, 입력을 감지하고 각 입력에 해당하는 값을 설정
    void Update()
    {
        // 게임 오버 상태라면 입력 처리를 중지하고 각 변수를 초기화 시킴.
        if(GameManager.Instance().isGameOver)
        {
            move = 0;
            rotate = 0;
            fire = false;
            reload = false;

            return; // 끝낸다.
        }

        move = Input.GetAxis(moveAxisName);
        rotate = Input.GetAxis(rotateAxisName);
        fire = Input.GetButton(fireButtonName);
        reload = Input.GetButtonDown(reloadButtonName);
    }
}
