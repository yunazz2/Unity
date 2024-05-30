using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// �ݵ�� �ʿ��� ������Ʈ�� ����� �ش� ������Ʈ�� �����Ǵ� ���� �����ϴ� ��Ʈ����Ʈ
[RequireComponent(typeof(AudioSource))]
public class FireCtrl : MonoBehaviour
{
    // �Ѿ� ������
    public GameObject bullet;

    // �Ѿ� �߻� ��ǥ
    public Transform firePos;

    // �� �Ҹ��� ����� ����� ����
    public AudioClip fireSfx;

    // AudioSource ������Ʈ�� ������ ����
    private new AudioSource audio;

    // Muzzle Flash�� MeshRenderer ������Ʈ
    private MeshRenderer muzzleFlash;

    private void Start()
    {
        audio = GetComponent<AudioSource>();

        // FirePos ������ �ִ� MuzzleFlash�� Material ������Ʈ�� ����
        muzzleFlash = firePos.GetComponentInChildren<MeshRenderer>();
        // ó�� ������ �� ��Ȱ��ȭ
        muzzleFlash.enabled = false;
    }

    void Update()
    {
        // ���콺 ���� ��ư�� Ŭ������ �� Fire �Լ� ȣ��
        if(Input.GetMouseButtonDown(0))
        {
            Fire();
        }
    }

    void Fire()
    {
        // Bullet �������� �������� ����(������ ��ü, ��ġ, ȸ��)
        Instantiate(bullet, firePos.position, firePos.rotation);

        // �� �Ҹ� �߻�
        audio.PlayOneShot(fireSfx, 1.0f);

        // �ѱ� ȭ�� ȿ�� �ڷ�ƾ �Լ� ȣ��
        StartCoroutine(ShowMuzzleFlash());
    }

    IEnumerator ShowMuzzleFlash()
    {
        // Muzzle Flash Ȱ��ȭ
        muzzleFlash.enabled = true;

        // 0.2�ʵ��� ���(����)�ϴ� ���� �޽��� ������ ������� �纸
        yield return new WaitForSeconds(0.2f);

        // MuzzleFlash ��Ȱ��ȭ
        muzzleFlash.enabled = false;

    }
}

