using UnityEngine;
using System.IO;
using UnityEngine.SceneManagement; 

public class SaveManager : MonoBehaviour 
{
    // Para cceder desde cualquier script (singleton)
    public static SaveManager Instancia { get; private set; }

    private string rutaArchivo; 

    void Awake() 
    {
        // evita que se duplique al cambiar de escena
        if (Instancia == null)
        {
            Instancia = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        rutaArchivo = Application.persistentDataPath + "/datosJugador.json";
    }

   public void GuardarJuego(int nivelActual, float saludActual, Vector3 posActual, string tiempoDeJuego) 
{
    PlayerData datos = new PlayerData();
    datos.nivel = nivelActual;
    datos.salud = saludActual;
    datos.posicion = new float[] { posActual.x, posActual.y, posActual.z };
    datos.nombreEscena = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;

    // se guardan los datos extra para la vista previa del menú
    datos.fechaGuardado = System.DateTime.Now.ToString("dd/MM/yyyy HH:mm");
    datos.tiempoJugadoTexto = tiempoDeJuego; 

    string json = JsonUtility.ToJson(datos);
    File.WriteAllText(rutaArchivo, json);
    Debug.Log("Juego Guardado en: " + rutaArchivo);
}

    public PlayerData CargarJuego() 
    {
        if (ExistePartidaGuardada()) 
        {
            string json = File.ReadAllText(rutaArchivo);
            return JsonUtility.FromJson<PlayerData>(json);
        }
        return null; 
    }

    // para saber si el botón de cargar debe estar activo
    public bool ExistePartidaGuardada()
    {
        return File.Exists(rutaArchivo);
    }
}