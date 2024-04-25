using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

// �÷��̾� ����ü�μ��� ������ ����ϴ� ��ũ��Ʈ
public class PlayerHealth : LivingEntity    // LivingEntity�� ��� ������ �ڵ����� �� ���� MonoBehaviour�� ��� �ްԵǱ� ������ ���� ������ ����
{
    public Slider healthSlider;             // ü�� ��
    
    public AudioClip deathClip;             // �÷��̾� ��� �� ����� ����
    public AudioClip hitClip;               // �÷��̾� �ǰ� �� ����� ����
    public AudioClip itemPickupClip;        // ������ ȹ�� �� ����� ����

    public AudioSource playerAudioPlayer;   // �÷��̾��� ����� �÷��̾�
    public Animator playerAnimator;

    public PlayerMovement playerMovement;
    public PlayerShooter playerShooter;

    void Awake()
    {
        playerAudioPlayer = GetComponent<AudioSource>();
        playerAnimator = GetComponent<Animator>();

        playerMovement = GetComponent<PlayerMovement>();
        playerShooter = GetComponent<PlayerShooter>();
    }

    protected override void OnEnable()
    {
        base.OnEnable();    // base�� �θ����� �ִ� �޼ҵ带 ���� �����ϰڴٴ� ��

        healthSlider.gameObject.SetActive(true);    // ü�� �� Ȱ��ȭ
        healthSlider.maxValue = startHealth;        // ü�� ���� �ִ�
        healthSlider.value = health;

        playerMovement.enabled = true;
        playerShooter.enabled = true;
    }

    [PunRPC]
    // ü�� ȸ�� �޼ҵ�
    public override void RestoreHealth(float newHealth)
    {
        base.RestoreHealth(newHealth);

        healthSlider.value = health;
    }

    [PunRPC]
    // �������� �޾��� ��
    public override void OnDamage(float damage, Vector3 hitPoint, Vector3 hitNormal)
    {
        // �÷��̾ ����� ���°� �ƴ϶�� �ǰ� �Ҹ� ���� ������ ó��
        if(!dead)
        {
            playerAudioPlayer.PlayOneShot(hitClip);
        }

        base.OnDamage(damage, hitPoint, hitNormal);

        healthSlider.value = health;
    }

    // �÷��̾ ������� ��
    public override void Die()
    {
        base.Die();

        healthSlider.gameObject.SetActive(false);   // ü�� �� ��Ȱ��ȭ
        playerAudioPlayer.PlayOneShot(deathClip);   // �״� �Ҹ� ���
        playerAnimator.SetTrigger("Die");

        playerMovement.enabled = false;
        playerShooter.enabled = false;

        // Invoke : n�� �ڿ� �ش� �޼ҵ� �Լ��� �����ų �Ŵ�.
        Invoke("Respawn", 5.0f);
    }

    // �����۰� �浹���� ��
    private void OnTriggerEnter(Collider other)
    {
        if(!dead)
        {
            IItem item = other.GetComponent<IItem>();

            if(item != null )
            {
                if(PhotonNetwork.IsMasterClient)
                {
                    item.Use(gameObject);
                    playerAudioPlayer.PlayOneShot(itemPickupClip);
                }
            }
        }
    }

    // �÷��̾� ������ �Լ�
    public void Respawn()
    {
        if(photonView.IsMine)
        {
            Vector3 randomSpawnPos = Random.insideUnitSphere * 5.0f;
            randomSpawnPos.y = 0.0f;

            transform.position = randomSpawnPos;
        }

        gameObject.SetActive(false);
        gameObject.SetActive(true);
    }
}
