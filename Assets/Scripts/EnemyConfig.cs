
using DefaultNamespace.Helpers;
using UnityEngine;

[CreateAssetMenu]
public class EnemyConfig : ConfigSingleton<EnemyConfig>
{
    
    [SerializeField] private Enemy[] enemyPrefabs;
    
    [Range(0, 100)] 
    [Tooltip("Respawn Timer")]
    public float respawnRate = 10;

    [Range(0, 5)] [Tooltip("Initial Delay Time")]
    public float initialSpawnDelay = 1;
    
    [Range(0, 100)] [Tooltip("Total number of Enemy")]
    public float totalNumberToSpawn = 1;
    
    [Range(0, 2)] [Tooltip("Enemy Type")]
    public int EnemyIndex = 1;

    public static Enemy GetEnemyPrefab(EnemyType type)
    {
        return Instance.enemyPrefabs[(int) type];
    }
}

public enum EnemyType
{
    Small = 1,
    
    Medium = 2,
    
    Large= 3,
    
    None = 0,
}

