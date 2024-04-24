using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ItemSpawner : MonoBehaviour
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
        Vector3 spawnPosition = GetRandomPointOnNavMesh(playerTransform.position, maxDistance);
        
        // �ٴڿ��� �������� 0.5��ŭ ����
        spawnPosition += Vector3.up * 0.5f;

        // ������ �� �������� �ϳ� ����
        GameObject selectItem = items[Random.Range(0, items.Length)];
        GameObject item = Instantiate(selectItem, spawnPosition, Quaternion.identity);

        // ������ ������ 5�� �ڿ� �ı��ϱ�
        Destroy(item, 5.0f);
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
