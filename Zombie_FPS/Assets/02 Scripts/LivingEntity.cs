using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEditor;

// 플레이어와 적 자체가 같은 생명체(?)라고 봐도 무방하니 클래스를 하나 만들어서 같이 쓰도록 한 스크립트
// 생명체로 동작할 모든 클래스가 상속 받을 클래스
// 체력, 체력 회복, 데미지, 사망 처리
public class LivingEntity : MonoBehaviourPun, IDamageable
{
    public float startHealth = 100.0f;
    public float health { get; protected set; }
    public bool dead { get; protected set; }    // 사망 여부
    public event Action onDeath;                // 사망 시 발생 시킬 이벤트 변수

    protected virtual void OnEnable()
    {
        dead = false;
        health = startHealth;
    }

    [PunRPC]
    public void ApplyUpdateHealth(float newHealth, bool newDead)
    {
        health = newHealth;
        dead = newDead;
    }

    [PunRPC]
    // 데미지 받을 때
    public virtual void OnDamage(float damage, Vector3 hitPoint, Vector3 hitNormal)
    {
        if(PhotonNetwork.IsMasterClient)
        {
            health -= damage;

            photonView.RPC("ApplyUpdateHealth", RpcTarget.Others, health, dead);
            photonView.RPC("OnDamage", RpcTarget.Others, damage, hitPoint, hitNormal);
        }
        
        if(health <= 0 && !dead)
        {
            Die();
        }

    }

    [PunRPC]
    // 체력 회복
    public virtual void RestoreHealth(float newHealth)
    {
        if(dead)
            return;

        if(PhotonNetwork.IsMasterClient)
        {
            health += newHealth;

            // 실행 할 메소드명, 원격 실행 할 대상 클라이언트 ex) 내 캐릭터를 가지고 있는 사람들한테 내 캐릭터의 체력 회복 내용이 보이도록 할 수 있음
            // 내 캐릭터 체력 회복 메소드 실행시켰음을 통지해서 다른 사람들 컴퓨터에서도 내 캐릭터 체력 회복이 된 것으로 보이는 느낌?
            photonView.RPC("ApplyUpdateHealth", RpcTarget.Others, health, dead);    // Others : 나를 제외한 다른 사람들 모두(* 나를 포함하려면 All을 사용)
            photonView.RPC("RestoreHealth", RpcTarget.Others, newHealth);
        }
    }

    // 사망 처리
    public virtual void Die()
    {
        if(onDeath != null)
        {
            onDeath();
        }

        dead = true;
    }


}
