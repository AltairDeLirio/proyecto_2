using Yarn.Unity;

//existe para que Yarn lo pueda leer, no tiene que ir en ningún lado específico, lo importante es que esté en el proyecto
public class YarnCommands
{
    //Uso en Yarn
    //<<load_scene "PlantaUno" "PlantaDos">>
    [YarnCommand("load_scene")]
    public static void LoadScene(string sceneName, string spawnPoint)
    {
        SceneLoader.Instance.LoadScene(sceneName, spawnPoint);
    }
}