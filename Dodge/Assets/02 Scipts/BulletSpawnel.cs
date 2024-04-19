using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletSpawnel : MonoBehaviour
{
    public GameObject bulletPrefab;
    public float spawnRateMin = 0.5f;   // �����ֱ� �ּڰ�
    public float spawnRateMax = 3.0f;   // �����ֱ� �ִ�

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
        timeAfterSpawn += Time.deltaTime;   // Ÿ�̸� ����

        if (timeAfterSpawn >= spawnRate)
        {
            timeAfterSpawn = 0.0f;
            GameObject bullet = Instantiate(bulletPrefab, transform.position, transform.rotation);  // Instantiate : ����
            bullet.transform.LookAt(target);

            spawnRate = Random.Range(spawnRateMin, spawnRateMax);
        }
    }
}
