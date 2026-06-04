using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class MenuPausa : MonoBehaviour
{
    public GameObject objetoMenuPausa; // panel ui de menú
    private PlayerMovement jugadorMovement;
    private CharacterController jugadorController;

    private bool juegoPausado = false;
    
    // tiempo de juego real
    private float tiempoTranscurrido;

    void Start()
    {
        // se busca al jugador en la escena al iniciar
        jugadorMovement = FindFirstObjectByType<PlayerMovement>();
        
        if (jugadorMovement != null)
        {
            jugadorController = jugadorMovement.GetComponent<CharacterController>();
            
            // se carga la partida desde el menu principal? se reposiciona al jugador
            if (SaveManager.Instancia != null && SaveManager.Instancia.ExistePartidaGuardada())
            {
                PlayerData datos = SaveManager.Instancia.CargarJuego();
                if (datos != null)
                {
                    Vector3 posicionCargada = new Vector3(datos.posicion[0], datos.posicion[1], datos.posicion[2]);
                    
                    jugadorController.enabled = false;
                    jugadorMovement.transform.position = posicionCargada;
                    jugadorController.enabled = true;
                    
                    // el tiempo empieza de cero para esta sesión actual de juego
                    tiempoTranscurrido = 0f; 
                    
                    Debug.Log("Jugador colocado en su posición guardada");
                }
            }
        }
        
        // asegurar que el menú empiece desactivado
        if (objetoMenuPausa != null)
            objetoMenuPausa.SetActive(false);
    }

    void Update()
    {
        // el cronómetro solo avanza si el juego no está pausado
        if (!juegoPausado)
        {
            tiempoTranscurrido += Time.deltaTime;
        }

        // Presionar ESC para pausar/reanudar
        if (Keyboard.current != null && Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            Debug.Log("ESC presionado - Juego pausado: " + juegoPausado);
            if (juegoPausado)
                Reanudar();
            else
                Pausar();
        }
    }

    public void Reanudar()
    {
        objetoMenuPausa.SetActive(false);
        Time.timeScale = 1f; 
        juegoPausado = false;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Pausar()
    {
        objetoMenuPausa.SetActive(true);
        Time.timeScale = 0f; // congela el juego
        juegoPausado = true;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    // boton de guardado (vinculado al botón de la ui)
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

            // se guarda todo en el JSON pasando el tiempo real calculado
            SaveManager.Instancia.GuardarJuego(nivelActual, saludActual, posicionReal, tiempoDeJuegoReal);
            Debug.Log("Partida guardada. Tiempo registrado: " + tiempoDeJuegoReal);
        }
    }

    // boton cargar (vinculado al botón de la ui)
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

    // boton de reiniciar juego
    public void BotonReiniciar()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    // boton de volver al menu principal
    public void BotonVolverMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }

    // salir del juego
    public void QuitGame()
    {
        Application.Quit();
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}