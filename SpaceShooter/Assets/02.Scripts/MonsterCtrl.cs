using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.AI;

// ���Ͱ� �÷��̾ ���� ������ �� �ֵ��� �� ��ũ��Ʈ
public class MonsterCtrl : MonoBehaviour
{
    // ������Ʈ�� ĳ�ø� ó���� ����
    private Transform monsterTr;
    private Transform playerTr;
    private NavMeshAgent agent;
    private Animator anim;

    // ������ ���� ����
    public enum State { IDLE, TRACE, ATTACK, DIE }

    // ������ ���� ����
    public State state = State.IDLE;
    // ���� �����Ÿ�
    public float traceDistance = 10.0f;
    // ���� �����Ÿ�
    public float attackDistance = 2.0f;
    // ������ ��� ����
    public bool isDie = false;

    // Animator �Ķ������ �ؽ� �� ����
    private readonly int hashTrace = Animator.StringToHash("IsTrace");
    private readonly int hashAttack = Animator.StringToHash("IsAttack");
    private readonly int hashHit = Animator.StringToHash("Hit");
    private readonly int hashPlayerDie = Animator.StringToHash("PlayerDie");
    private readonly int hashSpeed = Animator.StringToHash("Speed");

    // ���� ȿ�� ������
    private GameObject bloodEffect;

    void Start()
    {
        // ������ Transform �Ҵ�
        monsterTr = GetComponent<Transform>();

        // ���� ����� Player�� Transform �Ҵ�
        playerTr = GameObject.FindWithTag("PLAYER").GetComponent<Transform>();

        // NavMeshAgent ������Ʈ �Ҵ�
        agent = GetComponent<NavMeshAgent>();

        // Animator ������Ʈ �Ҵ�
        anim = GetComponent<Animator>();

        // BloodSprayEffect ������ �ε�
        bloodEffect = Resources.Load<GameObject>("BloodSprayEffect");

        // ���� ����� ��ġ�� �����ϸ� �ٷ� ���� ����
        //agent.destination = playerTr.position;

        // ������ ���¸� üũ�ϴ� �ڷ�ƾ �Լ� ȣ��
        StartCoroutine(CheckMonsterState());

        // ������ ���¿� ���� �ൿ�� �����ϴ� �ڷ�ƾ �Լ� ȣ��
        StartCoroutine(MonsterAction());
    }

    // 0.3�ʸ��� �÷��̾�� ���� ������ �Ÿ��� üũ�Ͽ� ������ ���¸� ������Ʈ�ϴ� �ڷ�ƾ �Լ�
    // - Update() �Լ� ���� �ۼ��ص� ����������, �� �����Ӹ��� �����ϴ� ���� ���ɻ� ������ ���� �� �����Ƿ� �ڷ�ƾ �Լ��� ó���Ѵ�.
    IEnumerator CheckMonsterState()
    {
        // isDie�� false�� ���� 0.3�ʸ��� �ݺ������� ����
        while(!isDie)
        {
            // 0.3�� ���� ����(���)�ϴ� ���� ������� �޽��� ������ �纸 - ������ sleep ����̶�� ���� �ȴ�.
            yield return new WaitForSeconds(0.3f);

            // ���Ϳ� �÷��̾� ������ �Ÿ� ����
            float distance = Vector3.Distance(playerTr.position, monsterTr.position);

            // ���� �����Ÿ� ������ ���Դ��� Ȯ��
            if (distance <= attackDistance)
            {
                state = State.ATTACK;
            }

            // ���� �����Ÿ� ������ ���Դ��� Ȯ��
            else if (distance <= traceDistance)
            {
                state = State.TRACE;
            }

            else
            {
                state = State.IDLE;
            }
        }
    }

    // ������ ���¿� ���� ������ ������ ����
    IEnumerator MonsterAction()
    {
        while(!isDie)
        {
            switch(state)
            {
                // ������ ����
                case State.IDLE:
                    // ���� ����
                    agent.isStopped = true;
                    // Animator�� IsTrace ������ false�� ����
                    anim.SetBool(hashTrace, false);
                    break;

                // ���� ����
                case State.TRACE:
                    // ���� ����� ��ǥ�� �̵� ����
                    agent.SetDestination(playerTr.position);
                    agent.isStopped = false;

                    // Animator�� IsTrace ������ true�� ����
                    anim.SetBool(hashTrace, true);

                    // Animator�� IsAttack ������ false�� ����
                    anim.SetBool(hashAttack, false);
                    break;

                // ���� ����
                case State.ATTACK:
                    anim.SetBool(hashAttack, true);
                    break;

                // ���
                case State.DIE:
                    break;
            }
            yield return new WaitForSeconds(0.3f);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.collider.CompareTag("BULLET"))
        {
            // �浹�� �Ѿ��� ����
            Destroy(collision.gameObject);

            // �ǰ� ���׼� �ִϸ��̼� ����
            anim.SetTrigger(hashHit);

            // �Ѿ��� �浹 ����
            Vector3 pos = collision.GetContact(0).point;
            // �Ѿ��� �浹 ������ ���� ����
            Quaternion rot = Quaternion.LookRotation(-collision.GetContact(0).normal);
            // ���� ȿ���� �����ϴ� �Լ� ȣ��
            ShowBloodEffect(pos, rot);
        }
    }

    private void OnTriggerEnter(Collider collider)
    {
        Debug.Log(collider.gameObject.name);
    }
    void ShowBloodEffect(Vector3 pos, Quaternion rot)
    {
        // ���� ȿ�� ����
        GameObject blood = Instantiate<GameObject>(bloodEffect, pos, rot, monsterTr);
        
        // 1�� �� ����
        Destroy(blood, 1.0f);
    }

    // �÷��̾� ��� ��
    void OnPlayerDie()
    {
        // ������ ���¸� üũ�ϴ� �ڷ�ƾ �Լ��� ��� ������Ŵ
        StopAllCoroutines();

        // ������ �����ϰ� �ִϸ��̼��� ����
        agent.isStopped = true;
        anim.SetFloat(hashSpeed, UnityEngine.Random.Range(0.8f, 1.2f));
        anim.SetTrigger(hashPlayerDie);
    }

    // MonsterAction �Լ����� �÷��̾�� ������ �Ÿ��� ���� ������ ���¸� �����ϰ� �Ǵµ�, �� ���¿� ���� �Ǽ����� ���� �����Ÿ� �� ���� �����Ÿ��� ǥ���ϴ� �Լ�
    private void OnDrawGizmos()
    {
        // ���� �����Ÿ� ǥ��
        if (state == State.TRACE)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(transform.position, traceDistance);
        }

        // ���� �����Ÿ� ǥ��
        if (state == State.ATTACK)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, attackDistance);
        }
    }
}