using UnityEngine;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviour
{
    [Header("Configuración de Botones")]
    public Button botonCargarPartida; // el boton principal del menu

    [Header("Ventana Flotante de Confirmación (UI)")]
    public GameObject panelConfirmacion; // el objeto del cuadro pop up
    public Text textoFecha;              // texto de la interfaz para la fecha
    public Text textoTiempo;             // texto de la interfaz para el tiempo de juego

    void Start()
    {
        // ssi SceneLoader.Instance es nulo, intentamos buscar el objeto en la escena
        if (SceneLoader.Instance == null)
        {
            SceneLoader cargadorAsistido = FindFirstObjectByType<SceneLoader>();
            if (cargadorAsistido == null)
            {
                Debug.LogWarning("[MainMenuManager] ¡Cuidado! No se encuentra ningún objeto con el script SceneLoader en la escena. Asegúrate de tener el objeto creado.");
            }
        }

        // si no hay archivo se desactiva el botón principal del menu
        if (botonCargarPartida != null && SaveManager.Instancia != null)
        {
            botonCargarPartida.interactable = SaveManager.Instancia.ExistePartidaGuardada();
        }

        //comprobacion
        if (panelConfirmacion != null)
        {
            panelConfirmacion.SetActive(false);
        }
    }

    public void StartGame()
    {
        if (SceneLoader.Instance != null)
        {
            SceneLoader.Instance.LoadScene("PlantaUno", "start");
        }
        else
        {
            Debug.LogError("[MainMenuManager] No se puede iniciar el juego porque SceneLoader.Instance sigue siendo nulo.");
        }
    }

    // boton "cargar partida" principal 
    public void AlPulsarCargarMenu()
    {
        if (SaveManager.Instancia != null)
        {
            PlayerData datos = SaveManager.Instancia.CargarJuego();

            if (datos != null && panelConfirmacion != null)
            {
                panelConfirmacion.SetActive(true);

                // se rellena los textos con los datos exactos
                if (textoFecha != null) textoFecha.text = "Última partida: " + datos.fechaGuardado;
                if (textoTiempo != null) textoTiempo.text = "Tiempo jugado: " + datos.tiempoJugadoTexto;
            }
        }
    }

    // boton "confirmar" 
    public void ConfirmarCarga()
    {
        if (SaveManager.Instancia != null)
        {
            PlayerData datos = SaveManager.Instancia.CargarJuego();
            if (datos != null)
            {
                if (SceneLoader.Instance != null)
                {
                    SceneLoader.Instance.LoadScene(datos.nombreEscena, "start");
                }
                else
                {
                    Debug.LogError("[MainMenuManager] No se puede cargar la partida porque SceneLoader.Instance es nulo.");
                }
            }
        }
    }

    // boton "cancelar"
    public void CancelarCarga()
    {
        if (panelConfirmacion != null)
        {
            panelConfirmacion.SetActive(false);
        }
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}