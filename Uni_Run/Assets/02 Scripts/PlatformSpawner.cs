using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformSpawner : MonoBehaviour
{
    public GameObject platformPrefab;
    public int count = 3;   // 생성할 발판의 수

    public float timeBetSpawnMin = 1.25f;   // 스폰을 할 최소 시간
    public float timeBetSpawnMax = 2.25f;   // 스폰을 할 최대 시간
    public float timeBetSpawn;              // 진짜 스폰할 시간

    public float yMin = -3.5f;  // y축 배치의 최솟값
    public float yMax = 1.5f;  // y축 배치의 최댓값
    public float xPos = 20.0f;

    private GameObject[] platforms;
    private int currentIndex = 0;

    private Vector2 poolPosition = new Vector2(0, -25);
    private float lastSpawnTime;

    void Start()
    {
        platforms = new GameObject[count];

        for(int i = 0; i < count; i++)
        {
            platforms[i] = Instantiate(platformPrefab, poolPosition, Quaternion.identity);
            /*
            Quaternion : position이라던가 rotation은 x, y, z축 세 개만 존재하지만, 실제로 회전은 x, y, z, w 로 총 4가지 방향이 존재한다.. (그렇다고 한다...)
            그래서 회전 값은 Quaternion으로 줘야하는거고, Quaternion.identity는 vector2.zero랑 같은 결이라고 보면 된다.
            근데 회전 값을 주고 싶을 때는 그냥 Rotate() 함수를 사용해서 괄호 안에 Vector3 값을 입력해주면 마지막 축 값은 알아서 계산해서
            들어간다.
             */
        }

        lastSpawnTime = 0.0f;
        timeBetSpawn = 0.0f;

        // transform.rotation = Quaternion.Euler(0, 0, 0); // Vector3 값인 오일러 값을 쿼터니언 형으로 변환시켜서 넣겠다는 뜻
        // transform.Rotate(Vector3.forward);              // 이렇게 사용해도 됨
    }
    
    void Update()
    {
        if(GameManager.instance.isGameOver)
        { return; }

        // Time.time은 현재 시간 불러오는 것
        if(Time.time >= lastSpawnTime + timeBetSpawn)   // 마지막 스폰 시간에 부여한 스폰 시간을 더한 시간이 현재 시간보다 작아지면 => 결국 스폰을 다시 해야하면
        {
            lastSpawnTime = Time.time;

            timeBetSpawn = Random.Range(timeBetSpawnMin, timeBetSpawnMax);
            float yPos = Random.Range(yMin, yMax);  // 위 아래 랜덤 위치에

            platforms[currentIndex].SetActive(false);   // Platform 스크립트에 작성한 OnEnable(활성화) 함수 때문에 SetActive로 껐다 켜주는 것
            platforms[currentIndex].SetActive(true);    // 그래야 OnEnable 함수가 다시 활성화되어 안에 코드가 실행될테니까

            platforms[currentIndex].transform.position = new Vector2(xPos, yPos);
            // 왜 이렇게 계속 객체를 새로 생성해서 쓰는가? => 벡터 값은 계속 변하기 때문에 새로운 값을 받아서 써야돼서.
            currentIndex++;

            if(currentIndex >= count)
            {
                currentIndex = 0;
            }
        }
    }
}
