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

    public Transform[] spawnPoints; // ������ ���� ����Ʈ��

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

    // IPunObservable ��� ������ ������ �Ʒ� �޼ҵ尡 �־�� ��
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
            if (GameManager.Instance().isGameOver)  // ���� ���� ���¶��
                return;

            if(!GameManager.Instance().isGameOver && enemies.Count <= 0)
            {
                SpawnWave();
            }
        }

        UpdateUI();
    }

    // ���̺�, ���� �� �ؽ�Ʈ ����
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

    // �ܰ� ���׷��̵�
    public void SpawnWave()
    {
        wave++;

        int spawnCount = Mathf.RoundToInt(wave * 1.5f);         // ���̺긶�� ������ ���� 1.5��� �þ����

        for (int i = 0; i < spawnCount; i++)
        {
            float enemyIntensity = Random.Range(0.0f, 1.0f);    // ���� ������ ������ ������ ���� ������ �������� ����
            CreateEnemy(enemyIntensity);
        }
    }

    // ���� ���� �޼ҵ�
    public void CreateEnemy(float intensity)
    {
        // Lerp : ���� ���� - a�� b ��ǥ ���� �˰� ���� ��, �� ������ c ��ǥ ���� �˾Ƴ� �� �ִ�.
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

        // ���� ������ �Ʒ� Action�� ��� �� �Լ��� ������� ����
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
