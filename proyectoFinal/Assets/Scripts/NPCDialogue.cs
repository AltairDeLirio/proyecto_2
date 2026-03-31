using UnityEngine;
using Yarn.Unity;

public class NPCDialogue : MonoBehaviour, IInteractable
{
    //cual dialogo debe lanzar el NPC, se asigna en el inspector de Unity
    public string dialogueNode;

    DialogueRunner runner;

    void Start()
    {
        runner = FindFirstObjectByType<DialogueRunner>();
    }

    public void Interact()
    {
        if (!runner.IsDialogueRunning)
        {
            runner.StartDialogue(dialogueNode);
        }
    }
}