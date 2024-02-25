using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject[] spawnPrefabs; // Array of different entities to spawn
    [SerializeField] Tower tower; 
    public int[] manaCosts; // Mana costs for each prefab

    private float aiSpawnCooldown = 5f; // Cooldown time in seconds for AI to spawn next troop
    private float nextAiSpawnTime;

    void Update()
    {
        if (Time.time >= nextAiSpawnTime)
        {
            SpawnNoMana(); 
            //TrySpawnAiTroop();
        }
    }


    private void SpawnNoMana()
    {
        int randomInt = Random.Range(0, spawnPrefabs.Length);
        Instantiate(spawnPrefabs[randomInt], new Vector2(transform.position.x + 10, transform.position.y), Quaternion.identity);
        nextAiSpawnTime = Time.time + aiSpawnCooldown;
    }



    private void TrySpawnAiTroop()
    {
        // For simplicity, AI always tries to spawn the first troop
        if (tower.getMana() >= manaCosts[0])
        {
            Instantiate(spawnPrefabs[0], new Vector2(transform.position.x + 10, transform.position.y), Quaternion.identity);
            tower.UseMana(manaCosts[0]);
            nextAiSpawnTime = Time.time + aiSpawnCooldown;
        }
    }
}
