using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerCtrl : MonoBehaviour
{
    // ������Ʈ ĳ�ø� ó���� ����
    private Transform tr;
    // Animation ������Ʈ�� ������ ����
    private Animation anim;

    // �̵� �ӵ� ����
    public float moveSpeed = 10.0f;
    // ȸ�� �ӵ� ����
    public float turnSpeed = 80.0f;

    // �ʱ� ���� ��
    private readonly float initHp = 100.0f;
    // ���� ���� ��
    public float currHp;
    // Hpbar�� ������ ����
    private Image hpBar;

    // ��������Ʈ ����
    public delegate void PlayerDieHandler();
    // �̺�Ʈ ����
    public static event PlayerDieHandler OnPlayerDie;

    IEnumerator Start()
    {
        // Hpbar ����
        hpBar = GameObject.FindGameObjectWithTag("HP_BAR")?.GetComponent<Image>();
        // HP �ʱ�ȭ
        currHp = initHp;

        // ������Ʈ�� ������ ������ ����
        tr = GetComponent<Transform>();
        anim = GetComponent<Animation>();

        // �ִϸ��̼� ����
        anim.Play("Idle");

        // ������ ������ ������ ���ΰ� ĳ���Ͱ� �ٶ󺸴� ������ �����ϰ� ����
        turnSpeed = 0.0f;   // ȸ�� �ӵ�
        yield return new WaitForSeconds(0.3f);
        turnSpeed = 80.0f;
    }

    void Update()
    {
        float h = Input.GetAxis("Horizontal");  // �¿�
        float v = Input.GetAxis("Vertical");    // ����
        float r = Input.GetAxis("Mouse X");     // ���콺 ���� ���⿡ ���� -1 �Ǵ� 1

        // �����¿� �̵� ���� ���� ���
        Vector3 moveDir = (Vector3.forward * v) + (Vector3.right* h);

        // Translate �Լ��� ����� �̵� ����
        tr.Translate(moveDir.normalized * Time.deltaTime * moveSpeed);

        // Rotate �Լ��� �̿��� ȸ�� ����
        tr.Rotate(Vector3.up * turnSpeed * Time.deltaTime * r);

        // ���ΰ� ĳ������ �ִϸ��̼� ����
        PlayerAnim(h, v);
    }

    void PlayerAnim(float h, float v)
    {
        // Ű���� �Է� ���� �������� ������ �ִϸ��̼� ����
        // * CrossFade("������ �ִϸ��̼� Ŭ����", �ٸ� �ִϸ��̼����� ���̵� �ƿ��Ǵ� �ð�);
        if(v >= 1.0f)
        {
            anim.CrossFade("RunF", 0.25f);  // ���� �ִϸ��̼� ����
        }
        else if (v <= -1.0f)
        {
            anim.CrossFade("RunB", 0.25f);  // ���� �ִϸ��̼� ����
        }
        else if(h >= 0.1f)
        {
            anim.CrossFade("RunR", 0.25f);  // ������ �̵� �ִϸ��̼� ����
        }
        else if(h <= -0.1f)
        {
            anim.CrossFade("RunL", 0.25f);  // ���� �̵� �ִϸ��̼� ����
        }
        else
        {
            anim.CrossFade("Idle", 0.25f);  // ���� �� Idle �ִϸ��̼� ����
        }
    }

    // �浹�� Collider�� IsTrigger �ɼ��� üũ���� �� �߻�
    private void OnTriggerEnter(Collider collider)
    {
        // �浹�� Collider�� ������ PUNCH�̸� �÷��̾��� HP ����
        if(currHp >= 0.0f && collider.CompareTag("PUNCH"))
        {
            currHp -= 10.0f;
            DisplayHealth();

            Debug.Log($"Player HP = {currHp / initHp}");

            // �÷��̾��� ������ 0 �����̸� ��� ó��
            if(currHp <= 0.0f)
            {
                PlayerDie();
            }
        }
    }

    // �÷��̾��� ��� ó��
    void PlayerDie()
    {
        Debug.Log("�÷��̾� ���!");

        //// Monster �±׸� ���� ��� ���� ������Ʈ�� ã�� ��
        //GameObject[] monsters = GameObject.FindGameObjectsWithTag("MONSTER");

        //// ��� ������ OnPlayerDie �Լ��� ���������� ȣ��
        //foreach (GameObject monster in monsters)
        //{
        //    monster.SendMessage("OnPlayerDie", SendMessageOptions.DontRequireReceiver);
        //}

        // �÷��̾� ��� �̺�Ʈ ȣ��(�߻�)
        OnPlayerDie();

        // GameManager ��ũ��Ʈ�� IsGameOver ������Ƽ ���� ����
        //GameObject.Find("GameMgr").GetComponent<GameManager>().IsGameOver = true;
        GameManager.instance.IsGameOver = true;
    }

    void DisplayHealth()
    {
        hpBar.fillAmount = currHp / initHp;
    }
}