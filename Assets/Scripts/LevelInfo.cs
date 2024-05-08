using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class LevelInfo : ScriptableObject
{
    [SerializeField] private List<SpawnerInfo> spawnList;

    public IEnumerable<SpawnerInfo> GetInfo()
    {
        for (int i = 0; i < spawnList.Count; i++)
        {
            yield return spawnList[i];
        }
    }
    
}
