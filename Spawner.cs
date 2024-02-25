using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Spawner : MonoBehaviour
{
    public GameObject[] spawnPrefabs; // Array of different entities to spawn
    public int[] manaCosts; // Mana costs for each prefab
    public float[] cooldowns;
    public Image[] cooldownImages;   
    public bool[] isGround; 
    private bool[] troopActive = new bool[10];
    private GameObject selectedPrefab;
    private int selectedPrefabManaCost;
    [SerializeField] Tower tower;


    void Start()
    {
        for (int i = 0; i < troopActive.Length; i++)
        {
            troopActive[i] = true;
        }
    }
    public void SetSelectedPrefab(int index)
    {
        if (index >= 0 && index < spawnPrefabs.Length)
        {
            selectedPrefab = spawnPrefabs[index];
            selectedPrefabManaCost = manaCosts[index];
            TrySpawnSelectedEntity(index);
        }
    }

    public void TrySpawnSelectedEntity(int index)
    {
        if (tower.getMana() >= manaCosts[index] && troopActive[index])
        {
            Vector2 spawnPosition = isGround[index] ? new Vector2(transform.position.x - 6, transform.position.y)
                                                    : new Vector2(transform.position.x - 6, transform.position.y + 2.5f);

            Instantiate(spawnPrefabs[index], spawnPosition, Quaternion.identity);
            tower.UseMana(manaCosts[index]);
            StartCoroutine(waitForCooldown(index));
        }
    }

    IEnumerator waitForCooldown(int index)
    {
        troopActive[index] = false;
        
        float cooldownEndTime = Time.time + cooldowns[index];

        while (Time.time < cooldownEndTime)
        {
            float remainingTime = cooldownEndTime - Time.time;
            cooldownImages[index].fillAmount = remainingTime / cooldowns[index];
            yield return null;
        }

        troopActive[index] = true;
        cooldownImages[index].fillAmount = 0.0f;
    }
}
