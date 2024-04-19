// �� ��ũ��Ʈ
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    // ���� ���¸� ���������� ����
    public enum State
    {
        Ready,  // �߻� �غ�� ����
        Empty,  // źâ�� �� ����
        Reload  // ������ ��
    }

    public State state { get; private set; }    // ���� ���� ����

    public Transform fireTransform;             // �ѱ��� ��ġ ��
    
    public ParticleSystem muzzleFlashEffect;
    public ParticleSystem shellEjectEffect;

    // �Ѿ��� �߻�� �� �Ѿ� ������ �׷��� �ϴϱ� �ʿ�
    public LineRenderer bulletLineRenderer;
    
    public AudioSource gunAudioPlayer;
    public AudioClip shotClip;      // �߻� �Ҹ� Ŭ��
    public AudioClip reloadClip;    // ������ �Ҹ� Ŭ��

    public float damage = 25.0f;
    public float fireDistance = 50.0f;  // �Ѿ��� ���ư� �ִ� �Ÿ�
    
    public int ammoRemain = 100;    // ���� ��ü ź���� ��
    public int magCapacity = 25;    // źâ �뷮
    public int magAmmo;             // ���� �� źâ ���� ź���� ��

    public float timeBetFire = 0.12f;   // ź�� �߻� ����
    public float reloadTime = 1.8f;     // ��� �������ϰ� �� �� �����ϱ� ������ �ҿ� �ð� �ο�
    public float lastFireTime;          // ���������� �Ѿ��� �߻��� �ð�

    void Start()
    {
        
    }

    void Update()
    {
        
    }
}
