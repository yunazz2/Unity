using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Photon.Pun;

public class Enemy : LivingEntity
{
    // LayerMask : ���̾ �����ϴ� ��
    public LayerMask targetLayer;           // ���� ��� ���̾�

    private LivingEntity targetEntity;      // ������ ���, LivingEntity�� �޴� ������ ����ü��� LivingEntity�� �Ǿ������ϱ�
    private NavMeshAgent pathFinder;        // �׺� �޽� ������Ʈ ������Ʈ

    public ParticleSystem hitEffect;        // �ǰ� ȿ�� ��ƼŬ
    public AudioClip deathSound;            // �״� �Ҹ�
    public AudioClip hitSound;              // �ǰ� �Ҹ�

    private Animator enemyAnimator;         // ���� �ִϸ��̼�
    private AudioSource enemyAudioPlayer;   // ���� �Ҹ� �����
    public Renderer enemyRenderer;         // 

    public float damage = 20.0f;            // ������
    public float timeBetAttack = 0.5f;      // ���� �ð� ����
    public float lastAttackTime;            // ������ ���� �ð�

    private bool hasTarget  // ������ ����� �����ϴ��� ���� Ȯ��
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

        // �������ڸ��� ���� ��θ� Ž���� �� �ֵ��� �ڷ�ƾ �Լ� ����
        StartCoroutine(UpdatePath());
    }

    void Update()
    {
        if(!PhotonNetwork.IsMasterClient)
        {
            return;
        }

        // ������ ������ ���� Ÿ���� ã�� �ִϸ��̼��� ����ǵ���
        enemyAnimator.SetBool("HasTarget", hasTarget);
    }

    [PunRPC]
    // ���� AI �ʱ� �ɷ�ġ�� �����ϴ� �Լ�
    public void SetUp(float newHealth, float newDamage, float newSpeed, Color skinColor)
    {
        startHealth = newHealth;
        health = newHealth;
        
        damage = newDamage;
        pathFinder.speed = newSpeed;
        
        enemyRenderer.material.color = skinColor;
    }


    // �ڷ�ƾ �Լ�
    // ��θ� ������ �Լ�
    private IEnumerator UpdatePath()
    {
        while(!dead) // ���� ������ ���� ���� �����̸� Ÿ���� ��� �����Ѵ�.
        {
            if(hasTarget)
            {
                pathFinder.isStopped = false;
                pathFinder.SetDestination(targetEntity.transform.position);
            }
            else
            {
                pathFinder.isStopped = true;

                // ���� ���� �ݰ� 20 �ȿ� �ִ� ��� ���̾ �ش��ϴ� �ݶ��̴��� ã�Ƽ� �ݶ��̴� �迭 �ȿ� �־��ٰ���
                Collider[] colliders = Physics.OverlapSphere(transform.position, 20.0f, targetLayer);

                for(int i = 0; i < colliders.Length; i++)
                {
                    // �ݶ��̴� �迭 �ȿ����� livingEntity�� ������ �ִ� �ֶ�� ����ü(�÷��̾�)�ϱ� �¸� Ÿ������ �����ϰڴ�.
                    LivingEntity livingEntity = colliders[i].GetComponent<LivingEntity>();

                    if(livingEntity != null && !livingEntity.dead)
                    {
                        targetEntity = livingEntity;
                        break;  // Ÿ���� ã������ for���� ��������
                    }
                }
            }

            yield return new WaitForSeconds(0.25f);
        }
    }

    [PunRPC]
    // �������� �޾��� ��
    public override void OnDamage(float damage, Vector3 hitPoint, Vector3 hitNormal)
    {
        if(!dead)
        {
            hitEffect.transform.position = hitPoint;
            hitEffect.transform.rotation = Quaternion.LookRotation(hitNormal);  // �˾Ƽ� �Ĵٺ��� �������� ȸ���� ��!
            hitEffect.Play();

            enemyAudioPlayer.PlayOneShot(hitSound);
        }

        base.OnDamage(damage, hitPoint, hitNormal);
    }

    // �׾��� ��
    public override void Die()
    {
        base.Die();

        Collider[] enemyColliders = GetComponents<Collider>();  // �������� �ִ� �ݶ��̴��� ĸ��, �ڽ� �� ���ϱ� components!!��

        for(int i = 0; i < enemyColliders.Length; i++)
        {
            enemyColliders[i].enabled = false;
        }

        pathFinder.isStopped = true;
        pathFinder.enabled = false;

        enemyAnimator.SetTrigger("Die");
        enemyAudioPlayer.PlayOneShot(deathSound);
    }

    // ���� �÷��̾�� ���ظ� �� ��(Stay�� �ϴ� ������ ���� ���� �ÿ��̾�� ��� ���� ��� �������� �޾ƾ��ϴϱ�)
    private void OnTriggerStay(Collider other)
    {
        if(!PhotonNetwork.IsMasterClient)
        {
            return;
        }

        if(!dead && Time.time >= lastAttackTime + timeBetAttack)    // ���� ������ ���� ���� �����̰�, ������ ���� �ð� + ���� �ð� ������ ���� �ð��� �Ѿ��ٸ�(?)
        {
            LivingEntity attackTarget = other.GetComponent<LivingEntity>();

            if(attackTarget != null && attackTarget == targetEntity)
            {
                lastAttackTime = Time.time; // ������ ���� �ð��� ���� �ð����� ����

                Vector3 hitPoint = other.ClosestPoint(transform.position);  // other�� collider ���ε�, �ݶ��̴� �ȿ��� �浹�� ��ġ �ȿ��� ���� ����� ���� ���� �����´�.
                Vector3 hitNormal = transform.position - other.transform.position;  // �� ��ġ���� �� ��ġ�� ���� �� �� ������ �ȴ�.(�ٶ󺸴� �� LookRotation)

                attackTarget.OnDamage(damage, hitPoint, hitNormal);
            }
        }
    }
}
