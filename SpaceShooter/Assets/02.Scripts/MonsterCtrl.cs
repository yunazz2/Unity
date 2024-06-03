using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.AI;

// 몬스터가 플레이어를 직접 추적할 수 있도록 한 스크립트
public class MonsterCtrl : MonoBehaviour
{
    // 컴포넌트의 캐시를 처리할 변수
    private Transform monsterTr;
    private Transform playerTr;
    private NavMeshAgent agent;
    private Animator anim;

    // 몬스터의 상태 정보
    public enum State { IDLE, TRACE, ATTACK, DIE }

    // 몬스터의 현재 상태
    public State state = State.IDLE;
    // 추적 사정거리
    public float traceDistance = 10.0f;
    // 공격 사정거리
    public float attackDistance = 2.0f;
    // 몬스터의 사망 여부
    public bool isDie = false;


    void Start()
    {
        // 몬스터의 Transform 할당
        monsterTr = GetComponent<Transform>();

        // 추적 대상인 Player의 Transform 할당
        playerTr = GameObject.FindWithTag("PLAYER").GetComponent<Transform>();

        // NavMeshAgent 컴포넌트 할당
        agent = GetComponent<NavMeshAgent>();

        // Animator 컴포넌트 할당
        anim = GetComponent<Animator>();

        // 추적 대상의 위치를 설정하면 바로 추적 시작
        //agent.destination = playerTr.position;

        // 몬스터의 상태를 체크하는 코루틴 함수 호출
        StartCoroutine(CheckMonsterState());

        // 몬스터의 상태에 따라 행동을 수행하는 코루틴 함수 호출
        StartCoroutine(MonsterAction());
    }

    // 0.3초마다 플레이어와 몬스터 사이의 거리를 체크하여 몬스터의 상태를 업데이트하는 코루틴 함수
    // - Update() 함수 내에 작성해도 무방하지만, 매 프레임마다 실행하는 것은 성능상 문제가 생길 수 있으므로 코루틴 함수로 처리한다.
    IEnumerator CheckMonsterState()
    {
        // isDie가 false인 동안 0.3초마다 반복적으로 실행
        while(!isDie)
        {
            // 0.3초 동안 중지(대기)하는 동안 제어권을 메시지 루프에 양보 - 일종의 sleep 기능이라고 봐도 된다.
            yield return new WaitForSeconds(0.3f);

            // 몬스터와 플레이어 사이의 거리 측정
            float distance = Vector3.Distance(playerTr.position, monsterTr.position);

            // 공격 사정거리 범위로 들어왔는지 확인
            if (distance <= attackDistance)
            {
                state = State.ATTACK;
            }

            // 추적 사정거리 범위로 들어왔는지 확인
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

    // 몬스터의 상태에 따라 몬스터의 동작을 수행
    IEnumerator MonsterAction()
    {
        while(!isDie)
        {
            switch(state)
            {
                // 가만히 상태
                case State.IDLE:
                    // 추적 중지
                    agent.isStopped = true;
                    // Animator의 IsTrace 변수를 false로 설정
                    anim.SetBool("IsTrace", false);
                    break;

                // 추적 상태
                case State.TRACE:
                    // 추적 대상의 좌표로 이동 시작
                    agent.SetDestination(playerTr.position);
                    agent.isStopped = false;
                    // Animator의 IsTrace 변수를 true로 설정
                    anim.SetBool("IsTrace", true);
                    break;

                // 공격 상태
                case State.ATTACK:
                    break;

                // 사망
                case State.DIE:
                    break;
            }
            yield return new WaitForSeconds(0.3f);
        }
    }

    private void OnDrawGizmos()
    {
        // 추적 사정거리 표시
        if (state == State.TRACE)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(transform.position, traceDistance);
        }

        // 공격 사정거리 표시
        if (state == State.ATTACK)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, attackDistance);
        }
    }
}