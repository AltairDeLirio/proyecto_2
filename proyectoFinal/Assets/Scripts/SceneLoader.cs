using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public static SceneLoader Instance;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    //cambio de escena y asignacion del spawn point
    public void LoadScene(string sceneName, string spawnPoint)
    {
        if (GameState.Instance == null)
        {
            GameState buscadorGameState = FindFirstObjectByType<GameState>();

            if (buscadorGameState != null)
            {
                GameState.Instance = buscadorGameState;
                GameState.Instance.spawnPointName = spawnPoint;
            }
            else
            {
                // si no existe te avisa en consola 
                Debug.LogWarning("[SceneLoader] No se pudo guardar el spawnPoint '" + spawnPoint + "' porque no existe ningún objeto 'GameState' en esta escena. Saltando este paso.");
            }
        }
        else
        {
            // si va bien se guarda el spawn point
            GameState.Instance.spawnPointName = spawnPoint;
        }

        //check que el juego no esté pausado antes de cargar la escena, para evitar problemas con el tiempo
        Time.timeScale = 1f;

        SceneManager.LoadScene(sceneName);
    }
}