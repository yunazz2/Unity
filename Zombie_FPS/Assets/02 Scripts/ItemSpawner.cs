using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Photon.Pun;

public class ItemSpawner : MonoBehaviourPun
{
    public GameObject[] items;              // ������ ���� ������Ʈ���� ������ �迭
    public Transform playerTransform;       // �÷��̾��� ��ġ
    
    public float maxDistance = 5.0f;        // �÷��̾�κ��� �������� ��ġ�� �ִ� �ݰ�

    public float timeBetSpawnMax = 7.0f;    // ������ ���� ���� �ִ� �ð�
    public float timeBetSpawnMin = 2.0f;    // ������ ���� ���� �ּ� �ð�
    public float timeBetSpawn;              // ������ ���� ����

    public float lastSpawnTime;             // ������ ������ ���� �ð�


    void Start()
    {
        // ������ ���� ���� ���� ������ ������ ������ ���� ���� �ʱ�ȭ
        timeBetSpawn = Random.Range(timeBetSpawnMin, timeBetSpawnMax);
        lastSpawnTime = 0;
    }

    void Update()
    {
        if(!PhotonNetwork.IsMasterClient)
        {
            return;
        }

        // �ֱ������� ������ ���� ó��
        // ���� ������ ������ ������ ���� �������� ���� �ֱ� �̻� �����ٸ� && �÷��̾ �����Ѵٸ�
        if(Time.time >= lastSpawnTime + timeBetSpawn && !GameManager.Instance().isGameOver)
        {
            lastSpawnTime = Time.time;
            timeBetSpawn = Random.Range(timeBetSpawnMin, timeBetSpawnMax);

            Spawn();
        }
    }

    // ������ ���� ó��
    public void Spawn()
    {
        // �÷��̾� ��ó���� ����޽� ���� ���� ��ġ ��������
        //Vector3 spawnPosition = GetRandomPointOnNavMesh(playerTransform.position, maxDistance); // �̷��� �׳� �θ� ������ �ֺ����� �������� �����Ǵϱ�
        Vector3 spawnPosition = GetRandomPointOnNavMesh(Vector3.zero, maxDistance);   // ���ո��� ��Ȳ�� �������� vector3�� ����

        // �ٴڿ��� �������� 0.5��ŭ ����
        spawnPosition += Vector3.up * 0.5f;

        // ������ �� �������� �ϳ� ����
        GameObject selectItem = items[Random.Range(0, items.Length)];
        //GameObject item = Instantiate(selectItem, spawnPosition, Quaternion.identity);
        GameObject item = PhotonNetwork.Instantiate(selectItem.name, spawnPosition, Quaternion.identity);

        // ������ ������ 5�� �ڿ� �ı��ϱ�
        //Destroy(item, 5.0f);
        StartCoroutine(DestroyAfter(item, 5.0f));
    }

    // �ڷ�ƾ �Լ�
    // PhotonNetwork.Destroy �Լ��� ������ ���� ���� �� ��� ���� Destroy �Լ��� ���� ���
    IEnumerator DestroyAfter(GameObject target, float delay)
    {
        yield return new WaitForSeconds(delay);

        if(target != null)
        {
            PhotonNetwork.Destroy(target);
        }
    }

    // ����޽� ���� ������ ��ġ�� ��ȯ�ϴ� �Լ�
    // �������� ���� ���� ��ֹ� ���� ����� �׷��� �ȵǰ�,
    // ����޽��� �̹� ��ֹ� �� ���ؼ� ���������ϱ� �� �ȿ��� ������ ��ġ�� �������� �����ϰڴ�.
    public Vector3 GetRandomPointOnNavMesh(Vector3 center, float distance)
    {
        Vector3 randomPos = Random.insideUnitSphere * distance + center;    // �Ϲ������� insideUnitSphere���� �������� 1�ε�, �ʹ� �����ϱ� �Ÿ��� ���ϰ� ���� ���� �����ִ� ����
        NavMeshHit hit;

        NavMesh.SamplePosition(randomPos, out hit, distance, NavMesh.AllAreas);

        return hit.position;
    }
}
