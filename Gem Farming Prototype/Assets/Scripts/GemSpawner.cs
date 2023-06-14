using System.Collections.Generic;
using UnityEngine;

public class GemSpawner : MonoBehaviour
{
    public List<GameObject> spawners;
    [HideInInspector] public bool ableToSpawn;

    void Start()
    {
        SpawnGemBatched(ableToSpawn);
    }

    public void SpawnGem(Vector3 spawnPos)
    {
        GemController gemController = GameManager.Instance.GetAvailableGem();
        gemController.InitGem(spawnPos, this);
    }

    public void SpawnGemBatched(bool isRuntime)
    {
        if (!isRuntime || (spawners.Count > 0 && spawners[0] == null))
            return;

        for (int i = 0; i < spawners.Count; i++)
        {
            SpawnGem(spawners[i].transform.position);
        }
    }

    public void AddSpawnersToList(GameObject spawner)
    {
        spawners.Add(spawner);
    }

    public void ClearSpawners()
    {
        spawners.Clear();
    }
}
