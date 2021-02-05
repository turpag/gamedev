using UnityEngine;
using UnityEngine.UI;

public class MyGameManager : MonoBehaviour {

    public EnemyData[] enemySpawns;
    int score = 0;
    public Text gameover;

    void Start () {
        for (int i = 0; i < enemySpawns.Length; i++) {
            enemySpawns[i].firstSpawnTimer = enemySpawns[i].firstSpawnTime; //Reset the first spawn time for each enemy
        }
    }

    void Update () {
        for ( int i = 0; i < enemySpawns.Length; i++) {
            if (enemySpawns[i]!=null) // Check to see if this GameObject exists before attempting to use its variables
            {
                enemySpawns[i].firstSpawnTimer -= Time.deltaTime;
                if (enemySpawns[i].firstSpawnTimer > 0) // Wait firstSpawnTime seconds before you spawn this enemy type
                {
                    continue;
                }

                float lerpX = Mathf.Abs(enemySpawns[i].firstSpawnTime) / enemySpawns[i].timeUntilMaxSpawnDelay;
                lerpX = Mathf.Clamp(lerpX, 0.0f, 1.0f);
                enemySpawns[i].currentSpawnDelay = Mathf.Lerp(enemySpawns[i].maxSpawnDelay, enemySpawns[i].minSpawnDelay, lerpX);
                enemySpawns[i].spawnTimer -= Time.deltaTime;
                if (enemySpawns[i].spawnTimer <= 0)
                {
                    enemySpawns[i].spawnTimer = enemySpawns[i].currentSpawnDelay;
                    Instantiate(enemySpawns[i].prefab);
                    score += enemySpawns[i].score;
                    Debug.Log("Score: " + score);
                }
            }
        }
	}
    
    public void GameOver()
    {
        gameover.text = "Game Over!";
    }
}
