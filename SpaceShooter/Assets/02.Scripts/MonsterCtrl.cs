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
    private readonly int hashDie = Animator.StringToHash("Die");

    // ���� ȿ�� ������
    private GameObject bloodEffect;

    // ���� ���� ����
    public int hp = 100;

    // ��ũ��Ʈ�� Ȱ��ȭ�� ������ ȣ��Ǵ� �Լ�
    private void OnEnable()
    {
        // �̺�Ʈ �߻� �� ������ �Լ� ����
        PlayerCtrl.OnPlayerDie += this.OnPlayerDie;

        // ������ ���¸� üũ�ϴ� �ڷ�ƾ �Լ� ȣ��
        StartCoroutine(CheckMonsterState());

        // ������ ���¿� ���� �ൿ�� �����ϴ� �ڷ�ƾ �Լ� ȣ��
        StartCoroutine(MonsterAction());
    }

    // ��ũ��Ʈ�� ��Ȱ��ȭ�� ������ ȣ��Ǵ� �Լ�
    private void OnDisable()
    {
        // ������ ����� �Լ� ����
        PlayerCtrl.OnPlayerDie -= this.OnPlayerDie;
    }


    void Awake()
    {
        // ������ Transform �Ҵ�
        monsterTr = GetComponent<Transform>();

        // ���� ����� Player�� Transform �Ҵ�
        playerTr = GameObject.FindWithTag("PLAYER").GetComponent<Transform>();

        // NavMeshAgent ������Ʈ �Ҵ�
        agent = GetComponent<NavMeshAgent>();
        // NavMeshAgent�� �ڵ� ȸ�� ��� ��Ȱ��ȭ
        agent.updateRotation = false;

        // Animator ������Ʈ �Ҵ�
        anim = GetComponent<Animator>();

        // BloodSprayEffect ������ �ε�
        bloodEffect = Resources.Load<GameObject>("BloodSprayEffect");

    }

    private void Update()
    {
        // ���������� ���� �Ÿ��� ȸ�� ���� �Ǵ�
        if (agent.remainingDistance >= 2.0f)
        {
            // ������Ʈ�� �̵� ����
            Vector3 direction = agent.desiredVelocity;
            // ȸ�� ����(���ʹϾ�) ����
            Quaternion rot = Quaternion.LookRotation(direction);
            // ���� �������� �Լ��� �ε巯�� ȸ�� ó��
            monsterTr.rotation = Quaternion.Slerp(monsterTr.rotation, rot, Time.deltaTime * 10.0f);
        }
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

            // ������ ���°� DIE�� �� �ڷ�ƾ ����
            if(state == State.DIE) yield break;

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
                    isDie = true;
                    // ���� ����
                    agent.isStopped = true;
                    // ��� �ִϸ��̼� ����
                    anim.SetTrigger(hashDie);
                    // ������ Collider ������Ʈ ��Ȱ��ȭ
                    GetComponent<CapsuleCollider>().enabled = false;

                    // ���� �ð� ��� �� ������Ʈ Ǯ������ ȯ��
                    yield return new WaitForSeconds(3.0f);

                    // ��� �� �ٽ� ����� ���� ���� hp �� �ʱ�ȭ
                    hp = 100;
                    isDie = false;

                    // ������ Collider ������Ʈ Ȱ��ȭ
                    GetComponent<CapsuleCollider>().enabled = true;
                    // ���͸� ��Ȱ��ȭ
                    this.gameObject.SetActive(false);

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
        }
    }

    // ����ĳ��Ʈ�� ���� �������� ������ ����
    public void OnDamage(Vector3 pos, Vector3 normal)
    {
        // �ǰ� ���׼� �ִϸ��̼� ����
        anim.SetTrigger(hashHit);
        
        Quaternion rot = Quaternion.LookRotation(normal);

        // ���� ȿ���� �����ϴ� �Լ� ȣ��
        ShowBloodEffect(pos, rot);

        // ������ HP ����
        hp -= 30;
        if (hp <= 0)
        {
            state = State.DIE;
            // ���Ͱ� ������� �� 50���� �߰�
            GameManager.instance.DisplayScore(50);
        }
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