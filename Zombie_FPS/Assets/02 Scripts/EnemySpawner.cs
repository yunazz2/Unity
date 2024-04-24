using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 
public class EnemySpawner : MonoBehaviour
{
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
    private int wave;

    void Update()
    {
        if (GameManager.Instance().isGameOver)
            return;

        if(enemies.Count <= 0)
        {
            SpawnWave();
        }

        UpdateUI();
    }

    // ���̺�, ���� �� �ؽ�Ʈ ����
    public void UpdateUI()
    {
        UIManager.Instance().UpdateWaveText(wave, enemies.Count);
    }

    // �ܰ� ���׷��̵�
    public void SpawnWave()
    {
        wave++;

        int spawnCount = Mathf.RoundToInt(wave * 1.5f); // ���̺긶�� ������ ���� 1.5��� �þ����

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

        Enemy enemy = Instantiate(enemyPrefab, spawnPoint.position, spawnPoint.rotation);
        enemy.SetUp(health, damage, speed, skinColor);
        enemies.Add(enemy);

        // ���� ������ �Ʒ� Action�� ��� �� �Լ��� ������� ����
        enemy.onDeath += () => enemies.Remove(enemy);
        enemy.onDeath += () => Destroy(enemy.gameObject, 10.0f);
        enemy.onDeath += () => GameManager.Instance().AddScore(100);
    }
}
