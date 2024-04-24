using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEditor;

// �÷��̾�� �� ��ü�� ���� ����ü(?)��� ���� �����ϴ� Ŭ������ �ϳ� ���� ���� ������ �� ��ũ��Ʈ
// ����ü�� ������ ��� Ŭ������ ��� ���� Ŭ����
// ü��, ü�� ȸ��, ������, ��� ó��
public class LivingEntity : MonoBehaviourPun, IDamageable
{
    public float startHealth = 100.0f;
    public float health { get; protected set; }
    public bool dead { get; protected set; }    // ��� ����
    public event Action onDeath;                // ��� �� �߻� ��ų �̺�Ʈ ����

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
    // ������ ���� ��
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
    // ü�� ȸ��
    public virtual void RestoreHealth(float newHealth)
    {
        if(dead)
            return;

        if(PhotonNetwork.IsMasterClient)
        {
            health += newHealth;

            // ���� �� �޼ҵ��, ���� ���� �� ��� Ŭ���̾�Ʈ ex) �� ĳ���͸� ������ �ִ� ��������� �� ĳ������ ü�� ȸ�� ������ ���̵��� �� �� ����
            // �� ĳ���� ü�� ȸ�� �޼ҵ� ����������� �����ؼ� �ٸ� ����� ��ǻ�Ϳ����� �� ĳ���� ü�� ȸ���� �� ������ ���̴� ����?
            photonView.RPC("ApplyUpdateHealth", RpcTarget.Others, health, dead);    // Others : ���� ������ �ٸ� ����� ���(* ���� �����Ϸ��� All�� ���)
            photonView.RPC("RestoreHealth", RpcTarget.Others, newHealth);
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
