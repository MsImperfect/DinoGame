using UnityEngine;

public class Spawner : MonoBehaviour
{
    [System.Serializable]
    public struct SpawnableObject
    {
        public GameObject prefab;
        [Range(0f, 1f)]
        public float spawnChance;
    }

    [System.Serializable]
    public struct MirrageObstacle
    {
        public GameObject mirrageObstaclePrefabs;
        [Range (0f, 1f)]
        public float mirrageSpawnChance;
    }

    public SpawnableObject[] spawnObject;
    public MirrageObstacle[] obstacles;
    private float minSpawnRate = 1f;
    private float maxSpawnRate = 2f;

    private void OnEnable()
    {
        Invoke(nameof(Spawn), Random.Range(minSpawnRate,maxSpawnRate));
    }

    private void OnDisable()
    {
        CancelInvoke();
    }

    private void Spawn()
    {
        float spawnChance = Random.value;

        foreach (var obj in spawnObject)
        {
            if(spawnChance < obj.spawnChance)
            {
                GameObject obstacle = Instantiate(obj.prefab);
                obstacle.transform.position += transform.position;
                break;
            }
            spawnChance -= obj.spawnChance;
        }

        Invoke(nameof(Spawn), Random.Range(minSpawnRate, maxSpawnRate));
    }

    public void MirrageSpawn()
    {
        float mirrageSpawnChance = Random.value;

        foreach (var obj in obstacles)
        {
            if (mirrageSpawnChance < obj.mirrageSpawnChance)
            {
                GameObject obstacle = Instantiate(obj.mirrageObstaclePrefabs);
                obstacle.transform.position += transform.position;
                break;
            }
            mirrageSpawnChance -= obj.mirrageSpawnChance;
        }

        Invoke(nameof(MirrageSpawn), Random.Range(minSpawnRate, maxSpawnRate));
    }
}
