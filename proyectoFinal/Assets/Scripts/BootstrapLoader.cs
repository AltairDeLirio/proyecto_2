using UnityEngine;
using UnityEngine.SceneManagement;

public class BootstrapLoader : MonoBehaviour
{
    void Start()
    {
        //cargar la escena del menú principal al iniciar el juego
        SceneManager.LoadScene("MainMenu");
    }
}