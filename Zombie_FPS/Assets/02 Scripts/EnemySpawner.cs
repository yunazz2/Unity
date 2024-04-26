using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using ExitGames.Client.Photon;

public class EnemySpawner : MonoBehaviourPun, IPunObservable
{
    private static EnemySpawner instance;
    public static EnemySpawner Instance()
    {
        return instance;
    }

    public Enemy enemyPrefab;

    public Transform[] spawnPoints; // 만들어둔 스폰 포인트들

    public float damageMax = 40.0f;
    public float damageMin = 20.0f;

    public float healthMax = 200.0f;
    public float healthMin = 100.0f;

    public float speedMax = 3.0f;
    public float speedMin = 1.0f;

    public Color strongEnemyColor = Color.red;

    private List<Enemy> enemies = new List<Enemy>();
    public int enemyCount = 0;
    public int wave;

    // IPunObservable 상속 받으면 무조건 아래 메소드가 있어야 함
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(enemyCount);
            stream.SendNext(wave);
        }
        else
        {
            enemyCount = (int)stream.ReceiveNext();
            wave = (int)stream.ReceiveNext();
        }
    }

    void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }

        PhotonPeer.RegisterType(typeof(Color), 128, ColorSerialization.SerializeColor, ColorSerialization.DeserializeColor);
    }

    void Update()
    {
        if(PhotonNetwork.IsMasterClient)
        {
            if (GameManager.Instance().isGameOver)  // 게임 오버 상태라면
                return;

            if(!GameManager.Instance().isGameOver && enemies.Count <= 0)
            {
                SpawnWave();
            }
        }

        UpdateUI();
    }

    // 웨이브, 좀비 수 텍스트 갱신
    public void UpdateUI()
    {
        if(PhotonNetwork.IsMasterClient)
        {
            UIManager.Instance().UpdateWaveText(wave, enemies.Count);
        }
        else
        {
            UIManager.Instance().UpdateWaveText(wave, enemyCount);
        }
    }

    // 단계 업그레이드
    public void SpawnWave()
    {
        wave++;

        int spawnCount = Mathf.RoundToInt(wave * 1.5f);         // 웨이브마다 좀비의 수가 1.5배로 늘어나도록

        for (int i = 0; i < spawnCount; i++)
        {
            float enemyIntensity = Random.Range(0.0f, 1.0f);    // 좀비를 스폰할 때마다 좀비의 강함 정도를 랜덤으로 지정
            CreateEnemy(enemyIntensity);
        }
    }

    // 좀비 생성 메소드
    public void CreateEnemy(float intensity)
    {
        // Lerp : 선형 보간 - a와 b 좌표 값을 알고 있을 때, 그 사이의 c 좌표 값을 알아낼 수 있다.
        float health = Mathf.Lerp(healthMin, healthMax, intensity);
        float damage = Mathf.Lerp(damageMin, damageMax, intensity);
        float speed = Mathf.Lerp(speedMin, speedMax, intensity);

        Color skinColor = Color.Lerp(Color.white, strongEnemyColor, intensity);

        Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];


        //Enemy enemy = Instantiate(enemyPrefab, spawnPoint.position, spawnPoint.rotation);
        GameObject createdEnemy = PhotonNetwork.Instantiate(enemyPrefab.name, spawnPoint.position, spawnPoint.rotation);
        Enemy enemy = createdEnemy.GetComponent<Enemy>();
        
        //enemy.SetUp(health, damage, speed, skinColor);
        enemy.photonView.RPC("SetUp", RpcTarget.All, health, damage, speed, skinColor);

        enemies.Add(enemy);

        // 좀비가 죽으면 아래 Action에 담아 둔 함수들 순서대로 실행
        enemy.onDeath += () => enemies.Remove(enemy);
        //enemy.onDeath += () => Destroy(enemy.gameObject, 10.0f);
        enemy.onDeath += () => StartCoroutine(DestroyAfter(enemy.gameObject, 10.0f));
        enemy.onDeath += () => GameManager.Instance().AddScore(100);
    }

    IEnumerator DestroyAfter(GameObject target, float delay)
    {
        yield return new WaitForSeconds(delay);

        if(target != null)
        {
            PhotonNetwork.Destroy(target);
        }
    }

}
