using UnityEngine;
using Yarn.Unity;

public class NPCDialogue : MonoBehaviour
{
    public string nodeName;

    public void StartDialogue()
    {
        DialogueRunner runner = FindFirstObjectByType<DialogueRunner>();

        if (runner != null && !runner.IsDialogueRunning)
        {
            runner.StartDialogue(nodeName);
        }
    }
}