using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformSpawner : MonoBehaviour
{
    public GameObject platformPrefab;
    public int count = 3;   // ������ ������ ��

    public float timeBetSpawnMin = 1.25f;   // ������ �� �ּ� �ð�
    public float timeBetSpawnMax = 2.25f;   // ������ �� �ִ� �ð�
    public float timeBetSpawn;              // ��¥ ������ �ð�

    public float yMin = -3.5f;  // y�� ��ġ�� �ּڰ�
    public float yMax = 1.5f;  // y�� ��ġ�� �ִ�
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
            Quaternion : position�̶���� rotation�� x, y, z�� �� ���� ����������, ������ ȸ���� x, y, z, w �� �� 4���� ������ �����Ѵ�.. (�׷��ٰ� �Ѵ�...)
            �׷��� ȸ�� ���� Quaternion���� ����ϴ°Ű�, Quaternion.identity�� vector2.zero�� ���� ���̶�� ���� �ȴ�.
            �ٵ� ȸ�� ���� �ְ� ���� ���� �׳� Rotate() �Լ��� ����ؼ� ��ȣ �ȿ� Vector3 ���� �Է����ָ� ������ �� ���� �˾Ƽ� ����ؼ�
            ����.
             */
        }

        lastSpawnTime = 0.0f;
        timeBetSpawn = 0.0f;

        // transform.rotation = Quaternion.Euler(0, 0, 0); // Vector3 ���� ���Ϸ� ���� ���ʹϾ� ������ ��ȯ���Ѽ� �ְڴٴ� ��
        // transform.Rotate(Vector3.forward);              // �̷��� ����ص� ��
    }
    
    void Update()
    {
        if(GameManager.instance.isGameOver)
        { return; }

        // Time.time�� ���� �ð� �ҷ����� ��
        if(Time.time >= lastSpawnTime + timeBetSpawn)   // ������ ���� �ð��� �ο��� ���� �ð��� ���� �ð��� ���� �ð����� �۾����� => �ᱹ ������ �ٽ� �ؾ��ϸ�
        {
            lastSpawnTime = Time.time;

            timeBetSpawn = Random.Range(timeBetSpawnMin, timeBetSpawnMax);
            float yPos = Random.Range(yMin, yMax);  // �� �Ʒ� ���� ��ġ��

            platforms[currentIndex].SetActive(false);   // Platform ��ũ��Ʈ�� �ۼ��� OnEnable(Ȱ��ȭ) �Լ� ������ SetActive�� ���� ���ִ� ��
            platforms[currentIndex].SetActive(true);    // �׷��� OnEnable �Լ��� �ٽ� Ȱ��ȭ�Ǿ� �ȿ� �ڵ尡 ������״ϱ�

            platforms[currentIndex].transform.position = new Vector2(xPos, yPos);
            // �� �̷��� ��� ��ü�� ���� �����ؼ� ���°�? => ���� ���� ��� ���ϱ� ������ ���ο� ���� �޾Ƽ� ��ߵż�.
            currentIndex++;

            if(currentIndex >= count)
            {
                currentIndex = 0;
            }
        }
    }
}
