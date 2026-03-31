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
        //guardar spawn point en el GameState para que el jugador pueda acceder a él después de cargar la escena
        GameState.Instance.spawnPointName = spawnPoint;

        //check que el juego no esté pausado antes de cargar la escena, para evitar problemas con el tiempo
        Time.timeScale = 1f;

        SceneManager.LoadScene(sceneName);
    }
}