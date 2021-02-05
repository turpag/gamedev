using UnityEngine;

public class EnemyData : MonoBehaviour{ 

    public GameObject prefab;
    public float minSpawnDelay;  // Number of seconds between spawns at hardest difficulty
    public float maxSpawnDelay;  // Number of seconds between spawns at the start of the game
    public float timeUntilMaxSpawnDelay; // Number of seconds between the enemy spawning at its beginning rate and the enemy spawning at its maximum rate
    public float firstSpawnTime;     // How many seconds to wait before this enemy type spawns
    public int score;  // Points given when enemy spawns

    [HideInInspector]
    public float spawnTimer;
    [HideInInspector]
    public float currentSpawnDelay;
    [HideInInspector]
    public float firstSpawnTimer;

}
