using UnityEngine;
using System.IO;

public class SaveManager : MonoBehaviour 
{
    private string rutaArchivo; 

    void Awake() 
    {
        rutaArchivo = Application.persistentDataPath + "/datosJugador.json";
    }

    public void GuardarJuego(int nivelActual, float saludActual, Vector3 posActual) 
    {
        PlayerData datos = new PlayerData();
        datos.nivel = nivelActual;
        datos.salud = saludActual;
        datos.posicion = new float[] { posActual.x, posActual.y, posActual.z };

        string json = JsonUtility.ToJson(datos);
        File.WriteAllText(rutaArchivo, json);
        Debug.Log("Juego Guardado en: " + rutaArchivo);
    }

    public PlayerData CargarJuego() 
    {
        if (File.Exists(rutaArchivo)) 
        {
            string json = File.ReadAllText(rutaArchivo);
            return JsonUtility.FromJson<PlayerData>(json);
        }
        return null; 
    }
}