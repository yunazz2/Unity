using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 플레이어와 적 자체가 같은 생명체(?)라고 봐도 무방하니 클래스를 하나 만들어서 같이 쓰도록 한 스크립트
// 생명체로 동작할 모든 클래스가 상속 받을 클래스
// 체력, 체력 회복, 데미지, 사망 처리
public class LivingEntity : MonoBehaviour, IDamageable
{
    public float startHealth = 100.0f;
    public float health { get; private set; }
    public bool dead { get; private set; }  // 사망 여부
    public event Action onDeath;            // 사망 시 발생 시킬 이벤트 변수

    protected virtual void OnEnable()
    {
        dead = false;
        health = startHealth;
    }

    // 데미지 받을 때
    public virtual void OnDamage(float damage, Vector3 hitPoint, Vector3 hitNormal)
    {
        health -= damage;

        if(health <= 0 && !dead)
        {
            Die();
        }
    }

    // 체력 회복
    public virtual void RestoreHealth(float newHealth)
    {
        if(dead)
        {
            return;
        }
        else
        {
            health += newHealth;
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
