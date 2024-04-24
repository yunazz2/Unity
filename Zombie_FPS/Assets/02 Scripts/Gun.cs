// 총 스크립트
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Unity.VisualScripting;

public class Gun : MonoBehaviourPun, IPunObservable
{
    // 총의 상태를 열거형으로 관리
    public enum State
    {
        Ready,      // 발사 준비된 상태
        Empty,      // 탄창이 빈 상태
        Reloading   // 재장전 중
    }
    
    // 프로퍼티
    public State state { get; private set; }    // 현재 총의 상태

    public Transform fireTransform;             // 총구의 위치 값
    
    public ParticleSystem muzzleFlashEffect;    // 총구 화염 효과
    public ParticleSystem shellEjectEffect;     // 탄피 떨어지는 효과

    public LineRenderer bulletLineRenderer;     // 총알이 발사될 때 총알 궤적을 그려야 하니까 필요

    public AudioSource gunAudioPlayer;
    public AudioClip shotClip;                  // 발사 소리 클립
    public AudioClip reloadClip;                // 재장전 소리 클립

    public float damage = 25.0f;                // 데미지 값
    public float fireDistance = 50.0f;          // 총알이 날아갈 최대 거리
    
    public int ammoRemain = 100;                // 남은 전체 탄알의 수
    public int magCapacity = 25;                // 탄창 용량
    public int magAmmo;                         // 현재 내 탄창 안의 탄알의 수

    public float timeBetFire = 0.12f;           // 탄알 발사 간격
    public float reloadTime = 1.8f;             // 계속 재장전 가능하도록 둘 수 없으니까 재장전 소요 시간 부여
    public float lastFireTime;                  // 마지막으로 총알을 발사한 시간
    

    void Awake()
    {
        gunAudioPlayer = GetComponent<AudioSource>();
        bulletLineRenderer = GetComponent<LineRenderer>();
        fireTransform = transform.GetChild(3).GetComponent<Transform>();    // gun 오브젝트의 자식 컴포넌트인 Fire Position 값을 할당

        bulletLineRenderer.positionCount = 2;
        bulletLineRenderer.enabled = false;
    }

    // 활성화되는 순간에 초기화시킬 것들
    private void OnEnable()
    {
        magAmmo = magCapacity;  // 탄창 용량만큼
        state = State.Ready;    // 발사 준비 상태로 설정
        lastFireTime = 0;       // 마지막으로 총알을 발사한 시간
    }

    // Gun 게임 오브젝트에 photon view 컴포넌트를 적용 시키려는데 Observed Component가 비활성화 돼서 해당 내용을 활성화 시켜야해서
    // IPunObservable 인터페이스를 상속 받게 되면서 아래 메소드를 생성하게 됨
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    // 나 지금 움직였어! 라는 데이터를 다른 클라이언트에게 보낼 때 해당 내용을 PhotonStream에 담아서 보낸다. (= 통신할 때 쓰는 모든 데이터가 들어가있다.)
    {
        if (stream.IsWriting)   // 현재 쓰기 모드 상태라면(로컬 클라이언트일 때 당연히 내 데이터는 내가 쓸 수 있으니까)
        {
            stream.SendNext(ammoRemain);    // 다른 사람들에게 값을 보내겠다.
            stream.SendNext(magAmmo);
            stream.SendNext(state);
        }
        else // 현재 쓰기 모드 상태라면
        {
            ammoRemain = (int)stream.ReceiveNext(); // 다른 사람들의 값을 받겠다. * ReceiveNext는 무조건 형 변환 시켜줘야 함 => 내가 받을 형으로
            magAmmo = (int)stream.ReceiveNext();
            state = (State)stream.ReceiveNext();
        }
    }

    [PunRPC]
    public void AddAmmo(int ammo)
    {
        ammoRemain += ammo;
        
    }

    // 총 발사를 시도할 때의 메소드
    public void Fire()
    {
        if(state == State.Ready && Time.time >= lastFireTime + timeBetFire)    // 발사 준비 중이면서 마지막으로 발사한 시간이 어쩌구 지났디면
        {
            lastFireTime = Time.time;
            Shot();
        }
    }

    // 실제 총 발사 메소드(Fire로 발사하겠다는 신호를 받고 이 안에서 예외 처리를 하고 실제 발사를 처리)
    private void Shot()
    {
        //RaycastHit hit;
        //Vector3 hitPosition = Vector3.zero;

        //// 만약 벽이든 누군가든 맞혔다면
        //if (Physics.Raycast(fireTransform.position, fireTransform.forward, out hit, fireDistance))
        //// Raycast(처음 시작할 지점, 방향, 맞은 오브젝트(?) 정보를 hit에 저장, 사정 거리)
        //{
        //    IDamageable target = hit.collider.GetComponent<IDamageable>();  // 데미지를 받을 수 있는 것들

        //    // 데미지를 받을 수 있는 것에 맞았다면
        //    if (target != null)
        //    {
        //        target.OnDamage(damage, hit.point, hit.normal); // hit.point : 맞은 위치 값, hit.normal : 방향 값(피 튀기는 효과 넣었을 때 어느 방향으로 튀길지 알아야해서)
        //    }

        //    hitPosition = hit.point;
        //}

        //// 못 맞혔다면
        //else
        //{
        //    hitPosition = fireTransform.position + fireTransform.forward * fireDistance;    // 총구의 위치 + 앞쪽 방향으로 * 최대 사정 거리만큼
        //}

        photonView.RPC("ShotProcessOnServer", RpcTarget.MasterClient);  // 서버 하면서 추가

        //StartCoroutine(ShotEffect(hitPosition));

        magAmmo--;
        if(magAmmo <= 0 )
        {
            state = State.Empty;
        }
    }

    // 호스트에서 실행되는 실제 발사 처리
    [PunRPC]
    private void ShotProcessOnServer()
    {
        RaycastHit hit;
        Vector3 hitPosition = Vector3.zero;

        if (Physics.Raycast(fireTransform.position, fireTransform.forward, out hit, fireDistance))
        {
            IDamageable target = hit.collider.GetComponent<IDamageable>(); // 충돌한 애가 IDamageable이 있으면

            if(target != null)
            {
                target.OnDamage(damage, hit.point, hit.normal);
            }

            hitPosition += hit.point;
        }
        else
        {
            hitPosition = fireTransform.position + fireTransform.forward * fireDistance;    // 총구의 위치 + 앞쪽 방향으로 * 최대 사정 거리만큼
        }
        photonView.RPC("ShotEffectProcessOnClients", RpcTarget.All, hitPosition);
    }

    [PunRPC]
    public void ShotEffectProcessOnClients(Vector3 hitPosition)
    {
        StartCoroutine(ShotEffect(hitPosition));
    }

    // 코루틴 함수 - 실행시키려면 StartCoroutine이라는 함수를 사용해야한다.
    // ex) StartCoroutine(ShotEffect(Vector3.right));
    private IEnumerator ShotEffect(Vector3 hitPosition)
    {
        muzzleFlashEffect.Play();   // 총구 화염 효과 실행
        shellEjectEffect.Play();    // 탄피 떨어지는 효과 실행

        gunAudioPlayer.PlayOneShot(shotClip);

        bulletLineRenderer.SetPosition(0, fireTransform.position);  // 총 발사 위치로부터
        bulletLineRenderer.SetPosition(1, hitPosition);             // 맞은 위치까지 궤적을 그리겠다.
        bulletLineRenderer.enabled = true;
        
        yield return new WaitForSeconds(0.03f); // 위 과정까지 다 실행하고 0.03초 쉰 후 아래를 실행하겠다. => 이 내용이 없으면 총알 발사 효과가 안 보임

        bulletLineRenderer.enabled = false;
    }

    // 재장전 메소드
    public bool Reload()
    {
        if(state == State.Reloading || ammoRemain <= 0 || magAmmo >= magCapacity)
        {
            return false;
        }
        else
        {
            StartCoroutine(ReloadRoutin());
            return true;
        }
    }

    // 코루틴 함수 - 실행시키려면 StartCoroutine이라는 함수를 사용해야한다.
    // ex) StartCoroutine(ShotEffect(Vector3.right));
    IEnumerator ReloadRoutin()
    {
        state = State.Reloading;
        gunAudioPlayer.PlayOneShot(reloadClip);

        yield return new WaitForSeconds(reloadTime);    // 재장전 시간만큼 잠깐 쉴 거예요~

        int ammoToFill = magCapacity - magAmmo;         // 탄창의 총 용량 - 현재 탄창 안의 탄약 수 = 결국 채워야할 탄약 수

        if(ammoRemain < ammoToFill)
        {
            ammoToFill = ammoRemain;
        }

        magAmmo += ammoToFill;
        ammoRemain -= ammoToFill;

        state = State.Ready;
    }


}
