using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Photon.Pun;

public class ItemSpawner : MonoBehaviourPun
{
    public GameObject[] items;              // 아이템 게임 오브젝트들을 저장할 배열
    public Transform playerTransform;       // 플레이어의 위치
    
    public float maxDistance = 5.0f;        // 플레이어로부터 아이템이 배치될 최대 반경

    public float timeBetSpawnMax = 7.0f;    // 아이템 생성 간격 최대 시간
    public float timeBetSpawnMin = 2.0f;    // 아이템 생성 간격 최소 시간
    public float timeBetSpawn;              // 아이템 생성 간격

    public float lastSpawnTime;             // 아이템 마지막 스폰 시간


    void Start()
    {
        // 아이템 생성 간격 랜덤 설정과 마지막 아이템 생성 시점 초기화
        timeBetSpawn = Random.Range(timeBetSpawnMin, timeBetSpawnMax);
        lastSpawnTime = 0;
    }

    void Update()
    {
        if(!PhotonNetwork.IsMasterClient)
        {
            return;
        }

        // 주기적으로 아이템 생성 처리
        // 현재 시점이 아이템 마지막 생성 시점에서 생성 주기 이상 지났다면 && 플레이어가 존재한다면
        if(Time.time >= lastSpawnTime + timeBetSpawn && !GameManager.Instance().isGameOver)
        {
            lastSpawnTime = Time.time;
            timeBetSpawn = Random.Range(timeBetSpawnMin, timeBetSpawnMax);

            Spawn();
        }
    }

    // 아이템 스폰 처리
    public void Spawn()
    {
        // 플레이어 근처에서 내비메쉬 위의 랜덤 위치 가져오기
        //Vector3 spawnPosition = GetRandomPointOnNavMesh(playerTransform.position, maxDistance); // 이렇게 그냥 두면 마스터 주변에만 아이템이 생성되니까
        Vector3 spawnPosition = GetRandomPointOnNavMesh(Vector3.zero, maxDistance);   // 불합리한 상황을 막으려고 vector3로 변경

        // 바닥에서 아이템을 0.5만큼 띄우기
        spawnPosition += Vector3.up * 0.5f;

        // 아이템 중 랜덤으로 하나 생성
        GameObject selectItem = items[Random.Range(0, items.Length)];
        //GameObject item = Instantiate(selectItem, spawnPosition, Quaternion.identity);
        GameObject item = PhotonNetwork.Instantiate(selectItem.name, spawnPosition, Quaternion.identity);

        // 생성된 아이템 5초 뒤에 파괴하기
        //Destroy(item, 5.0f);
        StartCoroutine(DestroyAfter(item, 5.0f));
    }

    // 코루틴 함수
    // PhotonNetwork.Destroy 함수는 딜레이 값을 넣을 수 없어서 직접 Destroy 함수를 만들어서 사용
    IEnumerator DestroyAfter(GameObject target, float delay)
    {
        yield return new WaitForSeconds(delay);

        if(target != null)
        {
            PhotonNetwork.Destroy(target);
        }
    }

    // 내비메쉬 위의 랜덤한 위치를 반환하는 함수
    // 아이템이 지형 내의 장애물 위에 생기고 그러면 안되고,
    // 내비메쉬를 이미 장애물 다 피해서 구워놨으니까 그 안에서 랜덤한 위치에 아이템을 생성하겠다.
    public Vector3 GetRandomPointOnNavMesh(Vector3 center, float distance)
    {
        Vector3 randomPos = Random.insideUnitSphere * distance + center;    // 일반적으로 insideUnitSphere에서 반지름은 1인데, 너무 작으니까 거리를 곱하고 센터 값을 더해주는 것임
        NavMeshHit hit;

        NavMesh.SamplePosition(randomPos, out hit, distance, NavMesh.AllAreas);

        return hit.position;
    }
}
