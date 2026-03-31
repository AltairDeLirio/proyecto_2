using UnityEngine;

public class MainMenuManager : MonoBehaviour
{
    public void StartGame()
    {
        //nombre de la escena, nombre del punto de inicio
        SceneLoader.Instance.LoadScene("PlantaUno", "start");
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}