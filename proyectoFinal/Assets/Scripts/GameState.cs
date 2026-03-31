using UnityEngine;

public class GameState : MonoBehaviour
{
    public static GameState Instance;

    //ejemplo - se utilizara para el puzle, para saber si el jugador tiene los objetos necesarios para avanzar
    public bool hasKey;

    //sistema de spawnpoint
    public string spawnPointName;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;

            //guarda el estado del juego al cambiar de escena
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}