using UnityEngine;

public class ChestSpawner : MonoBehaviour
{
    [Header("Префаб Сундука (з компонентом WorldChest)")]
    public GameObject chestPrefab; 

    [Header("Межі спавну на карті")]
    public float minX = -10f;
    public float maxX = 10f;
    public float minY = -10f;
    public float maxY = 10f;

    private float spawnTimer = 60f;

    void Update()
    {
        spawnTimer -= Time.deltaTime;
        if (spawnTimer <= 0f)
        {
            SpawnRandomChest();
            spawnTimer = 60f; // Скидаємо таймер знову на 1 хвилину
        }
    }

    void SpawnRandomChest()
    {
        if (chestPrefab == null) return;

        Vector2 randomPos = new Vector2(Random.Range(minX, maxX), Random.Range(minY, maxY));
        
        GameObject newChest = Instantiate(chestPrefab, randomPos, Quaternion.identity);
        WorldChest chestScript = newChest.GetComponent<WorldChest>();

        if (chestScript != null)
        {
            int roll = Random.Range(1, 11); 
            if (roll == 1)
            {
                chestScript.isRoyal = true;
                newChest.GetComponent<SpriteRenderer>().color = Color.yellow; 
                Debug.Log("<color=yellow>⭐ На карті з'явився безкоштовний Королівський Сундук!</color>");
            }
            else
            {
                chestScript.isRoyal = false;
                Debug.Log("📦 На карті заспавнився звичайний безкоштовний сундук.");
            }
        }
    }
}