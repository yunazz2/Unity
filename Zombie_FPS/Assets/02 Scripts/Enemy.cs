using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Photon.Pun;

public class Enemy : LivingEntity
{
    // LayerMask : 레이어를 저장하는 형
    public LayerMask targetLayer;           // 추적 대상 레이어

    private LivingEntity targetEntity;      // 추적할 대상, LivingEntity로 받는 이유는 생명체라면 LivingEntity로 되어있으니까
    private NavMeshAgent pathFinder;        // 네비 메쉬 에이전트 컴포넌트

    public ParticleSystem hitEffect;        // 피격 효과 파티클
    public AudioClip deathSound;            // 죽는 소리
    public AudioClip hitSound;              // 피격 소리

    private Animator enemyAnimator;         // 좀비 애니메이션
    private AudioSource enemyAudioPlayer;   // 좀비 소리 재생기
    public Renderer enemyRenderer;         // 

    public float damage = 20.0f;            // 데미지
    public float timeBetAttack = 0.5f;      // 공격 시간 간격
    public float lastAttackTime;            // 마지막 공격 시간

    private bool hasTarget  // 추적할 대상이 존재하는지 여부 확인
    {
        get
        {
            if (targetEntity != null && !targetEntity.dead)
            {
                return true;
            }

            return false;
        }
    }

    void Awake()
    {
        pathFinder = GetComponent<NavMeshAgent>();
        enemyAnimator = GetComponent<Animator>();
        enemyAudioPlayer = GetComponent<AudioSource>();
    }

    void Start()
    {
        if(!PhotonNetwork.IsMasterClient)
        {
            return;
        }

        // 시작하자마자 좀비가 경로를 탐색할 수 있도록 코루틴 함수 실행
        StartCoroutine(UpdatePath());
    }

    void Update()
    {
        if(!PhotonNetwork.IsMasterClient)
        {
            return;
        }

        // 프레임 단위로 좀비가 타겟을 찾는 애니메이션이 실행되도록
        enemyAnimator.SetBool("HasTarget", hasTarget);
    }

    [PunRPC]
    // 좀비 AI 초기 능력치를 결정하는 함수
    public void SetUp(float newHealth, float newDamage, float newSpeed, Color skinColor)
    {
        startHealth = newHealth;
        health = newHealth;
        
        damage = newDamage;
        pathFinder.speed = newSpeed;
        
        enemyRenderer.material.color = skinColor;
    }


    // 코루틴 함수
    // 경로를 추적할 함수
    private IEnumerator UpdatePath()
    {
        while(!dead) // 좀비 본인이 죽지 않은 상태이면 타겟을 계속 추적한다.
        {
            if(hasTarget)
            {
                pathFinder.isStopped = false;
                pathFinder.SetDestination(targetEntity.transform.position);
            }
            else
            {
                pathFinder.isStopped = true;

                // 좀비 본인 반경 20 안에 있는 모든 레이어에 해당하는 콜라이더를 찾아서 콜라이더 배열 안에 넣어줄거임
                Collider[] colliders = Physics.OverlapSphere(transform.position, 20.0f, targetLayer);

                for(int i = 0; i < colliders.Length; i++)
                {
                    // 콜라이더 배열 안에서도 livingEntity를 가지고 있는 애라면 생명체(플레이어)니까 걔를 타겟으로 설정하겠다.
                    LivingEntity livingEntity = colliders[i].GetComponent<LivingEntity>();

                    if(livingEntity != null && !livingEntity.dead)
                    {
                        targetEntity = livingEntity;
                        break;  // 타겟을 찾았으면 for문을 빠져나가
                    }
                }
            }

            yield return new WaitForSeconds(0.25f);
        }
    }

    [PunRPC]
    // 데미지를 받았을 때
    public override void OnDamage(float damage, Vector3 hitPoint, Vector3 hitNormal)
    {
        if(!dead)
        {
            hitEffect.transform.position = hitPoint;
            hitEffect.transform.rotation = Quaternion.LookRotation(hitNormal);  // 알아서 쳐다보는 방향으로 회전을 해!
            hitEffect.Play();

            enemyAudioPlayer.PlayOneShot(hitSound);
        }

        base.OnDamage(damage, hitPoint, hitNormal);
    }

    // 죽었을 때
    public override void Die()
    {
        base.Die();

        Collider[] enemyColliders = GetComponents<Collider>();  // 좀비한테 있는 콜라이더가 캡슐, 박스 두 개니까 components!!로

        for(int i = 0; i < enemyColliders.Length; i++)
        {
            enemyColliders[i].enabled = false;
        }

        pathFinder.isStopped = true;
        pathFinder.enabled = false;

        enemyAnimator.SetTrigger("Die");
        enemyAudioPlayer.PlayOneShot(deathSound);
    }

    // 좀비가 플레이어에게 피해를 줄 때(Stay로 하는 이유는 좀비 손이 플에이어에게 닿는 동안 계속 데미지를 받아야하니까)
    private void OnTriggerStay(Collider other)
    {
        if(!PhotonNetwork.IsMasterClient)
        {
            return;
        }

        if(!dead && Time.time >= lastAttackTime + timeBetAttack)    // 좀비 본인이 죽지 않은 상태이고, 마지막 공격 시간 + 공격 시간 간격이 지금 시간을 넘었다면(?)
        {
            LivingEntity attackTarget = other.GetComponent<LivingEntity>();

            if(attackTarget != null && attackTarget == targetEntity)
            {
                lastAttackTime = Time.time; // 마지막 공격 시간을 현재 시간으로 갱신

                Vector3 hitPoint = other.ClosestPoint(transform.position);  // other가 collider 값인데, 콜라이더 안에서 충돌한 위치 안에서 가장 가까운 지점 값을 가져온다.
                Vector3 hitNormal = transform.position - other.transform.position;  // 내 위치에서 너 위치를 빼면 너 쪽 방향이 된다.(바라보는 건 LookRotation)

                attackTarget.OnDamage(damage, hitPoint, hitNormal);
            }
        }
    }
}
