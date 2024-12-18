using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SummonEnemy : MonoBehaviour
{
    [Header("Enemy Summon Settings")]
    [SerializeField] private GameObject[] enemyPrefabs;
    [SerializeField] private float spawnInterval = 3f;
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private Transform targetPoint;

    [Header("Random Spawn Area Settings")]
    [SerializeField] private float spawnRangeX = 5f; // Rentang acak untuk posisi X
    [SerializeField] private float spawnRangeZ = 5f; // Rentang acak untuk posisi Z


    private void Start()
    {
        StartCoroutine(SpawnEnemyRoutine());
    }

    private IEnumerator SpawnEnemyRoutine()
    {
        while (true)
        {
            // Random Pick Enemy
            GameObject enemyToSpawn = enemyPrefabs[Random.Range(0, enemyPrefabs.Length)];

            // Tentukan posisi spawn acak dalam rentang yang ditentukan
            Vector3 randomSpawnPosition = GetRandomSpawnPosition();

            // Summon musuh pada posisi acak
            GameObject newEnemy = Instantiate(enemyToSpawn, randomSpawnPosition, Quaternion.identity);

            // Gerakan musuh maju
            StartCoroutine(MoveEnemyToTarget(newEnemy));
            yield return new WaitForSeconds(spawnInterval);
        }
    }

    private Vector3 GetRandomSpawnPosition()
    {
        // Menghitung posisi acak dalam rentang yang ditentukan
        float randomX = Random.Range(spawnPoint.position.x - spawnRangeX, spawnPoint.position.x + spawnRangeX);
        float randomZ = Random.Range(spawnPoint.position.z - spawnRangeZ, spawnPoint.position.z + spawnRangeZ);

        // Kembalikan posisi 3D dengan Y yang tetap sama dengan spawnPoint
        return new Vector3(randomX, spawnPoint.position.y, randomZ);
    }

    private IEnumerator MoveEnemyToTarget(GameObject enemy)
    {
        while (Vector3.Distance(enemy.transform.position, targetPoint.position) > 0.1f)
        {
            enemy.transform.position = Vector3.MoveTowards(enemy.transform.position, targetPoint.position, moveSpeed * Time.deltaTime);
            yield return null;
        }

        Destroy(enemy);
    }
}
