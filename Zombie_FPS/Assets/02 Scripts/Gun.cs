// 총 스크립트
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    // 총의 상태를 열거형으로 관리
    public enum State
    {
        Ready,  // 발사 준비된 상태
        Empty,  // 탄창이 빈 상태
        Reload  // 재장전 중
    }

    public State state { get; private set; }    // 현재 총의 상태

    public Transform fireTransform;             // 총구의 위치 값
    
    public ParticleSystem muzzleFlashEffect;
    public ParticleSystem shellEjectEffect;

    // 총알이 발사될 때 총알 궤적을 그려야 하니까 필요
    public LineRenderer bulletLineRenderer;
    
    public AudioSource gunAudioPlayer;
    public AudioClip shotClip;      // 발사 소리 클립
    public AudioClip reloadClip;    // 재장전 소리 클립

    public float damage = 25.0f;
    public float fireDistance = 50.0f;  // 총알이 날아갈 최대 거리
    
    public int ammoRemain = 100;    // 남은 전체 탄알의 수
    public int magCapacity = 25;    // 탄창 용량
    public int magAmmo;             // 현재 내 탄창 안의 탄알의 수

    public float timeBetFire = 0.12f;   // 탄알 발사 간격
    public float reloadTime = 1.8f;     // 계속 재장전하게 둘 수 없으니까 재장전 소요 시간 부여
    public float lastFireTime;          // 마지막으로 총알을 발사한 시간

    void Start()
    {
        
    }

    void Update()
    {
        
    }
}
