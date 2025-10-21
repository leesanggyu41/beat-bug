using UnityEngine;
using static chatmanager;

public class DialogueManager : MonoBehaviour
{
    public chatmanager dialogueUI;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (!dialogueUI.gameObject.activeSelf)
            {
                dialogueUI.StartDialogue();
            }
            else
            {
                dialogueUI.NextCut();
            }
        }
    }
}
