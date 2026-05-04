using UnityEngine;
using Yarn.Unity;

public class PickupItem : MonoBehaviour
{
    public string variableName = "$hasKey";

    public void Interact()
    {
        var storage = FindFirstObjectByType<InMemoryVariableStorage>();

        if (storage != null)
            storage.SetValue(variableName, true);

        Debug.Log("Picked up item!");
        Destroy(gameObject);
    }
}