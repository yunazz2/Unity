using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// �÷��̾�� �� ��ü�� ���� ����ü(?)��� ���� �����ϴ� Ŭ������ �ϳ� ���� ���� ������ �� ��ũ��Ʈ
// ����ü�� ������ ��� Ŭ������ ��� ���� Ŭ����
// ü��, ü�� ȸ��, ������, ��� ó��
public class LivingEntity : MonoBehaviour, IDamageable
{
    public float startHealth = 100.0f;
    public float health { get; private set; }
    public bool dead { get; private set; }  // ��� ����
    public event Action onDeath;            // ��� �� �߻� ��ų �̺�Ʈ ����

    protected virtual void OnEnable()
    {
        dead = false;
        health = startHealth;
    }

    // ������ ���� ��
    public virtual void OnDamage(float damage, Vector3 hitPoint, Vector3 hitNormal)
    {
        health -= damage;

        if(health <= 0 && !dead)
        {
            Die();
        }
    }

    // ü�� ȸ��
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

    // ��� ó��
    public virtual void Die()
    {
        if(onDeath != null)
        {
            onDeath();
        }

        dead = true;
    }
}
