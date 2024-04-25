using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

// 플레이어 생명체로서의 동작을 담당하는 스크립트
public class PlayerHealth : LivingEntity    // LivingEntity를 상속 받으면 자동으로 그 안의 MonoBehaviour도 상속 받게되기 때문에 기존 내용은 지움
{
    public Slider healthSlider;             // 체력 바
    
    public AudioClip deathClip;             // 플레이어 사망 시 오디오 파일
    public AudioClip hitClip;               // 플레이어 피격 시 오디오 파일
    public AudioClip itemPickupClip;        // 아이템 획득 시 오디오 파일

    public AudioSource playerAudioPlayer;   // 플레이어의 오디오 플레이어
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
        base.OnEnable();    // base는 부모한테 있는 메소드를 먼저 실행하겠다는 뜻

        healthSlider.gameObject.SetActive(true);    // 체력 바 활성화
        healthSlider.maxValue = startHealth;        // 체력 바의 최댓값
        healthSlider.value = health;

        playerMovement.enabled = true;
        playerShooter.enabled = true;
    }

    [PunRPC]
    // 체력 회복 메소드
    public override void RestoreHealth(float newHealth)
    {
        base.RestoreHealth(newHealth);

        healthSlider.value = health;
    }

    [PunRPC]
    // 데미지를 받았을 때
    public override void OnDamage(float damage, Vector3 hitPoint, Vector3 hitNormal)
    {
        // 플레이어가 사망한 상태가 아니라면 피격 소리 내고 데미지 처리
        if(!dead)
        {
            playerAudioPlayer.PlayOneShot(hitClip);
        }

        base.OnDamage(damage, hitPoint, hitNormal);

        healthSlider.value = health;
    }

    // 플레이어가 사망했을 때
    public override void Die()
    {
        base.Die();

        healthSlider.gameObject.SetActive(false);   // 체력 바 비활성화
        playerAudioPlayer.PlayOneShot(deathClip);   // 죽는 소리 재생
        playerAnimator.SetTrigger("Die");

        playerMovement.enabled = false;
        playerShooter.enabled = false;

        // Invoke : n초 뒤에 해당 메소드 함수를 실행시킬 거다.
        Invoke("Respawn", 5.0f);
    }

    // 아이템과 충돌했을 때
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

    // 플레이어 리스폰 함수
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
