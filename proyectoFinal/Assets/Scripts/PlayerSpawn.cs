using UnityEngine;

public class PlayerSpawn : MonoBehaviour
{
    void Start()
    {
        //encontrar spawn
        SpawnPoint[] spawns = FindObjectsByType<SpawnPoint>(FindObjectsSortMode.None);

        foreach (var spawn in spawns)
        {
            //match spawn con spawn point name
            if (spawn.spawnName == GameState.Instance.spawnPointName)
            {
                transform.position = spawn.transform.position;
                return;
            }
        }
    }
}