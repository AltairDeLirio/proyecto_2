using UnityEngine;
using Yarn.Unity;

//esto es un codigo de ejemplo, creado para probar que el sistema de dialogos funciona, no es parte del proyecto final, se puede eliminar o modificar segun sea necesario
public class KeyPickup : MonoBehaviour, IInteractable
{
    DialogueRunner runner;

    //se busca el DialogueRunner en la escena para poder modificar las variables de Yarn cuando el jugador interactua con este objeto
    void Start()
    {
        runner = FindFirstObjectByType<DialogueRunner>();
    }

    //cuando el jugador interactua con este objeto, se le da la llave y se destruye el objeto
    public void Interact()
    {
        runner.VariableStorage.SetValue("$hasKey", true);
        GameState.Instance.hasKey = true;

        Destroy(gameObject);
    }
}