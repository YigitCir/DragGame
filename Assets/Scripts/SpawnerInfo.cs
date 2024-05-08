using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class SpawnerInfo : ScriptableObject
{
    public EnemyType EnemyType = EnemyType.None;

    public float waitTime = 0;
}
