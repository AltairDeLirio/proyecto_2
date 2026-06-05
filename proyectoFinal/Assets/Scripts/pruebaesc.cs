using UnityEngine;
using UnityEngine.InputSystem; // AÑADIR

public class PruebaESC : MonoBehaviour
{
    void Update()
    {
        if (Keyboard.current != null && Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            Debug.Log("¡ESC FUNCIONA EN PRUEBAESC!");
        }
    }
}