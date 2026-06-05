using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class MenuPausa : MonoBehaviour
{
    public GameObject objetoMenuPausa;
    private PlayerMovement jugadorMovement;
    private CharacterController jugadorController;
    private bool juegoPausado = false;
    private float tiempoTranscurrido;

    void Start()
    {
        jugadorMovement = FindFirstObjectByType<PlayerMovement>();

        if (jugadorMovement != null)
        {
            jugadorController = jugadorMovement.GetComponent<CharacterController>();

            if (SaveManager.Instancia != null && SaveManager.Instancia.ExistePartidaGuardada())
            {
                PlayerData datos = SaveManager.Instancia.CargarJuego();
                if (datos != null)
                {
                    Vector3 posicionCargada = new Vector3(datos.posicion[0], datos.posicion[1], datos.posicion[2]);
                    jugadorController.enabled = false;
                    jugadorMovement.transform.position = posicionCargada;
                    jugadorController.enabled = true;
                    tiempoTranscurrido = 0f;
                    Debug.Log("Jugador colocado en su posición guardada");
                }
            }
        }

        if (objetoMenuPausa != null)
            objetoMenuPausa.SetActive(false);

        // Asegurar que el tiempo siempre esté en 1 (para que los botones funcionen)
        Time.timeScale = 1f;
    }

    void Update()
    {
        if (!juegoPausado)
        {
            tiempoTranscurrido += Time.deltaTime;
        }

        if (Keyboard.current != null && Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            Debug.Log("ESC presionado");
            if (juegoPausado)
                Reanudar();
            else
                Pausar();
        }
    }

    public void Reanudar()
    {
        objetoMenuPausa.SetActive(false);
        juegoPausado = false;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        // Reactivar movimiento del jugador
        if (jugadorMovement != null)
            jugadorMovement.enabled = true;
    }

    void Pausar()
    {
        objetoMenuPausa.SetActive(true);
        juegoPausado = true;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        // Desactivar movimiento del jugador en lugar de pausar el tiempo
        if (jugadorMovement != null)
            jugadorMovement.enabled = false;
    }

    public void BotonGuardar()
    {
        if (jugadorMovement != null)
        {
            int nivelActual = 1;
            float saludActual = 100f;
            Vector3 posicionReal = jugadorMovement.transform.position;

            int horas = Mathf.FloorToInt(tiempoTranscurrido / 3600f);
            int minutos = Mathf.FloorToInt((tiempoTranscurrido % 3600f) / 60f);
            int segundos = Mathf.FloorToInt(tiempoTranscurrido % 60f);
            string tiempoDeJuegoReal = string.Format("{0:00}h {1:00}m {2:00}s", horas, minutos, segundos);

            SaveManager.Instancia.GuardarJuego(nivelActual, saludActual, posicionReal, tiempoDeJuegoReal);
            Debug.Log("Partida guardada. Tiempo registrado: " + tiempoDeJuegoReal);
        }
    }

    public void BotonCargar()
    {
        PlayerData datos = SaveManager.Instancia.CargarJuego();

        if (datos != null && jugadorMovement != null && jugadorController != null)
        {
            Reanudar();
            Vector3 posicionCargada = new Vector3(datos.posicion[0], datos.posicion[1], datos.posicion[2]);
            jugadorController.enabled = false;
            jugadorMovement.transform.position = posicionCargada;
            jugadorController.enabled = true;
            Debug.Log("Partida cargada desde el menú de pausa");
        }
    }

    public void BotonReiniciar()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void BotonVolverMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }

    public void QuitGame()
    {
        Application.Quit();
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}