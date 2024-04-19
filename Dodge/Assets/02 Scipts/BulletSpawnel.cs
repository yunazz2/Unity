using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletSpawnel : MonoBehaviour
{
    public GameObject bulletPrefab;
    public float spawnRateMin = 0.5f;   // 생성주기 최솟값
    public float spawnRateMax = 3.0f;   // 생성주기 최댓값

    private Transform target;
    private float spawnRate;
    private float timeAfterSpawn;


    // Start is called before the first frame update
    void Start()
    {
        timeAfterSpawn = 0.0f;
        spawnRate = Random.Range(spawnRateMin, spawnRateMax);
        target = FindObjectOfType<PlayerController>().transform;

    }

    // Update is called once per frame
    void Update()
    {
        timeAfterSpawn += Time.deltaTime;   // 타이머 역할

        if (timeAfterSpawn >= spawnRate)
        {
            timeAfterSpawn = 0.0f;
            GameObject bullet = Instantiate(bulletPrefab, transform.position, transform.rotation);  // Instantiate : 복제
            bullet.transform.LookAt(target);

            spawnRate = Random.Range(spawnRateMin, spawnRateMax);
        }
    }
}
