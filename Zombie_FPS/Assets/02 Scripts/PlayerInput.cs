using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

// �÷��̾��� �Է��� ������ ��ũ��Ʈ
public class PlayerInput : MonoBehaviourPun
{
    public string moveAxisName = "Vertical";        // �÷��̾ �� �ڷ� �̵�
    public string rotateAxisName = "Horizontal";    // �÷��̾ �� ��� ȸ��
    public string fireButtonName = "Fire1";         // �� �߻�
    public string reloadButtonName = "Reload";      // ������

    // ������Ƽ
    // �� �Է¿� ���� ���� �����ϸ�, �ܺο��� ������ �Ұ����ϴ�.
    public float move { get; private set; }
    public float rotate { get; private set; }
    public bool fire { get; private set; }
    public bool reload {  get; private set; }

    void Start()
    {
    }

    // �����Ӹ��� ȣ��Ǵ� �Լ���, �Է��� �����ϰ� �� �Է¿� �ش��ϴ� ���� ����
    void Update()
    {
        if(!photonView.IsMine)
        {
            return;
        }


        // ���� ���� ���¶�� �Է� ó���� �����ϰ� �� ������ �ʱ�ȭ ��Ŵ.
        if(GameManager.Instance().isGameOver)
        {
            move = 0;
            rotate = 0;
            fire = false;
            reload = false;

            return; // ������.
        }

        move = Input.GetAxis(moveAxisName);
        rotate = Input.GetAxis(rotateAxisName);
        fire = Input.GetButton(fireButtonName);
        reload = Input.GetButtonDown(reloadButtonName);
    }
}
