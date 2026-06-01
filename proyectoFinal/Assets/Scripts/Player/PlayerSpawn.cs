using UnityEngine;

public class PlayerSpawn : MonoBehaviour
{
    void Start()
    {
        string targetSpawnName = "start";

        if (GameState.Instance != null)
        {
            targetSpawnName = GameState.Instance.spawnPointName;
        }
        else
        {
            Debug.LogWarning("[PlayerSpawn] GameState.Instance es nulo. Se usará el punto de aparición por defecto: 'start'.");
        }

        SpawnPoint[] spawns = FindObjectsByType<SpawnPoint>(FindObjectsSortMode.None);

        foreach (var spawn in spawns)
        {
            if (spawn.spawnName == targetSpawnName)
            {
                transform.position = spawn.transform.position;
                Debug.Log("[PlayerSpawn] Jugador posicionado con éxito en el spawn point: " + targetSpawnName);
                return;
            }
        }

        Debug.LogError("[PlayerSpawn] No se encontró ningún objeto 'SpawnPoint' con el nombre: " + targetSpawnName);
    }
}