using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInteraction : MonoBehaviour
{
    public float distance = 2f;

    public void OnInteract(InputAction.CallbackContext context)
    {
        if (!context.performed) return;

        Ray ray = new Ray(transform.position, transform.forward);

        if (Physics.Raycast(ray, out RaycastHit hit, distance))
        {
            Debug.Log("HIT OBJECT: " + hit.collider.name);
            Debug.Log("HIT ROOT: " + hit.collider.transform.root.name);

            var npc = hit.collider.GetComponentInParent<NPCDialogue>();
            if (npc != null)
            {
                Debug.Log("NPC FOUND");
                npc.StartDialogue();
                return;
            }

            var pickup = hit.collider.GetComponentInParent<PickupItem>();
            if (pickup != null)
            {
                Debug.Log("PICKUP FOUND");
                pickup.Interact();
                return;
            }

            Debug.Log("NO INTERACTABLE FOUND");
        }
        else
        {
            Debug.Log("RAY HIT NOTHING");
        }
    }
}