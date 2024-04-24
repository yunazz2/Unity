// �� ��ũ��Ʈ
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Unity.VisualScripting;

public class Gun : MonoBehaviourPun, IPunObservable
{
    // ���� ���¸� ���������� ����
    public enum State
    {
        Ready,      // �߻� �غ�� ����
        Empty,      // źâ�� �� ����
        Reloading   // ������ ��
    }
    
    // ������Ƽ
    public State state { get; private set; }    // ���� ���� ����

    public Transform fireTransform;             // �ѱ��� ��ġ ��
    
    public ParticleSystem muzzleFlashEffect;    // �ѱ� ȭ�� ȿ��
    public ParticleSystem shellEjectEffect;     // ź�� �������� ȿ��

    public LineRenderer bulletLineRenderer;     // �Ѿ��� �߻�� �� �Ѿ� ������ �׷��� �ϴϱ� �ʿ�

    public AudioSource gunAudioPlayer;
    public AudioClip shotClip;                  // �߻� �Ҹ� Ŭ��
    public AudioClip reloadClip;                // ������ �Ҹ� Ŭ��

    public float damage = 25.0f;                // ������ ��
    public float fireDistance = 50.0f;          // �Ѿ��� ���ư� �ִ� �Ÿ�
    
    public int ammoRemain = 100;                // ���� ��ü ź���� ��
    public int magCapacity = 25;                // źâ �뷮
    public int magAmmo;                         // ���� �� źâ ���� ź���� ��

    public float timeBetFire = 0.12f;           // ź�� �߻� ����
    public float reloadTime = 1.8f;             // ��� ������ �����ϵ��� �� �� �����ϱ� ������ �ҿ� �ð� �ο�
    public float lastFireTime;                  // ���������� �Ѿ��� �߻��� �ð�
    

    void Awake()
    {
        gunAudioPlayer = GetComponent<AudioSource>();
        bulletLineRenderer = GetComponent<LineRenderer>();
        fireTransform = transform.GetChild(3).GetComponent<Transform>();    // gun ������Ʈ�� �ڽ� ������Ʈ�� Fire Position ���� �Ҵ�

        bulletLineRenderer.positionCount = 2;
        bulletLineRenderer.enabled = false;
    }

    // Ȱ��ȭ�Ǵ� ������ �ʱ�ȭ��ų �͵�
    private void OnEnable()
    {
        magAmmo = magCapacity;  // źâ �뷮��ŭ
        state = State.Ready;    // �߻� �غ� ���·� ����
        lastFireTime = 0;       // ���������� �Ѿ��� �߻��� �ð�
    }

    // Gun ���� ������Ʈ�� photon view ������Ʈ�� ���� ��Ű���µ� Observed Component�� ��Ȱ��ȭ �ż� �ش� ������ Ȱ��ȭ ���Ѿ��ؼ�
    // IPunObservable �������̽��� ��� �ް� �Ǹ鼭 �Ʒ� �޼ҵ带 �����ϰ� ��
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    // �� ���� ��������! ��� �����͸� �ٸ� Ŭ���̾�Ʈ���� ���� �� �ش� ������ PhotonStream�� ��Ƽ� ������. (= ����� �� ���� ��� �����Ͱ� ���ִ�.)
    {
        if (stream.IsWriting)   // ���� ���� ��� ���¶��(���� Ŭ���̾�Ʈ�� �� �翬�� �� �����ʹ� ���� �� �� �����ϱ�)
        {
            stream.SendNext(ammoRemain);    // �ٸ� ����鿡�� ���� �����ڴ�.
            stream.SendNext(magAmmo);
            stream.SendNext(state);
        }
        else // ���� ���� ��� ���¶��
        {
            ammoRemain = (int)stream.ReceiveNext(); // �ٸ� ������� ���� �ްڴ�. * ReceiveNext�� ������ �� ��ȯ ������� �� => ���� ���� ������
            magAmmo = (int)stream.ReceiveNext();
            state = (State)stream.ReceiveNext();
        }
    }

    [PunRPC]
    public void AddAmmo(int ammo)
    {
        ammoRemain += ammo;
        
    }

    // �� �߻縦 �õ��� ���� �޼ҵ�
    public void Fire()
    {
        if(state == State.Ready && Time.time >= lastFireTime + timeBetFire)    // �߻� �غ� ���̸鼭 ���������� �߻��� �ð��� ��¼�� �������
        {
            lastFireTime = Time.time;
            Shot();
        }
    }

    // ���� �� �߻� �޼ҵ�(Fire�� �߻��ϰڴٴ� ��ȣ�� �ް� �� �ȿ��� ���� ó���� �ϰ� ���� �߻縦 ó��)
    private void Shot()
    {
        //RaycastHit hit;
        //Vector3 hitPosition = Vector3.zero;

        //// ���� ���̵� �������� �����ٸ�
        //if (Physics.Raycast(fireTransform.position, fireTransform.forward, out hit, fireDistance))
        //// Raycast(ó�� ������ ����, ����, ���� ������Ʈ(?) ������ hit�� ����, ���� �Ÿ�)
        //{
        //    IDamageable target = hit.collider.GetComponent<IDamageable>();  // �������� ���� �� �ִ� �͵�

        //    // �������� ���� �� �ִ� �Ϳ� �¾Ҵٸ�
        //    if (target != null)
        //    {
        //        target.OnDamage(damage, hit.point, hit.normal); // hit.point : ���� ��ġ ��, hit.normal : ���� ��(�� Ƣ��� ȿ�� �־��� �� ��� �������� Ƣ���� �˾ƾ��ؼ�)
        //    }

        //    hitPosition = hit.point;
        //}

        //// �� �����ٸ�
        //else
        //{
        //    hitPosition = fireTransform.position + fireTransform.forward * fireDistance;    // �ѱ��� ��ġ + ���� �������� * �ִ� ���� �Ÿ���ŭ
        //}

        photonView.RPC("ShotProcessOnServer", RpcTarget.MasterClient);  // ���� �ϸ鼭 �߰�

        //StartCoroutine(ShotEffect(hitPosition));

        magAmmo--;
        if(magAmmo <= 0 )
        {
            state = State.Empty;
        }
    }

    // ȣ��Ʈ���� ����Ǵ� ���� �߻� ó��
    [PunRPC]
    private void ShotProcessOnServer()
    {
        RaycastHit hit;
        Vector3 hitPosition = Vector3.zero;

        if (Physics.Raycast(fireTransform.position, fireTransform.forward, out hit, fireDistance))
        {
            IDamageable target = hit.collider.GetComponent<IDamageable>(); // �浹�� �ְ� IDamageable�� ������

            if(target != null)
            {
                target.OnDamage(damage, hit.point, hit.normal);
            }

            hitPosition += hit.point;
        }
        else
        {
            hitPosition = fireTransform.position + fireTransform.forward * fireDistance;    // �ѱ��� ��ġ + ���� �������� * �ִ� ���� �Ÿ���ŭ
        }
        photonView.RPC("ShotEffectProcessOnClients", RpcTarget.All, hitPosition);
    }

    [PunRPC]
    public void ShotEffectProcessOnClients(Vector3 hitPosition)
    {
        StartCoroutine(ShotEffect(hitPosition));
    }

    // �ڷ�ƾ �Լ� - �����Ű���� StartCoroutine�̶�� �Լ��� ����ؾ��Ѵ�.
    // ex) StartCoroutine(ShotEffect(Vector3.right));
    private IEnumerator ShotEffect(Vector3 hitPosition)
    {
        muzzleFlashEffect.Play();   // �ѱ� ȭ�� ȿ�� ����
        shellEjectEffect.Play();    // ź�� �������� ȿ�� ����

        gunAudioPlayer.PlayOneShot(shotClip);

        bulletLineRenderer.SetPosition(0, fireTransform.position);  // �� �߻� ��ġ�κ���
        bulletLineRenderer.SetPosition(1, hitPosition);             // ���� ��ġ���� ������ �׸��ڴ�.
        bulletLineRenderer.enabled = true;
        
        yield return new WaitForSeconds(0.03f); // �� �������� �� �����ϰ� 0.03�� �� �� �Ʒ��� �����ϰڴ�. => �� ������ ������ �Ѿ� �߻� ȿ���� �� ����

        bulletLineRenderer.enabled = false;
    }

    // ������ �޼ҵ�
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

    // �ڷ�ƾ �Լ� - �����Ű���� StartCoroutine�̶�� �Լ��� ����ؾ��Ѵ�.
    // ex) StartCoroutine(ShotEffect(Vector3.right));
    IEnumerator ReloadRoutin()
    {
        state = State.Reloading;
        gunAudioPlayer.PlayOneShot(reloadClip);

        yield return new WaitForSeconds(reloadTime);    // ������ �ð���ŭ ��� �� �ſ���~

        int ammoToFill = magCapacity - magAmmo;         // źâ�� �� �뷮 - ���� źâ ���� ź�� �� = �ᱹ ä������ ź�� ��

        if(ammoRemain < ammoToFill)
        {
            ammoToFill = ammoRemain;
        }

        magAmmo += ammoToFill;
        ammoRemain -= ammoToFill;

        state = State.Ready;
    }


}
