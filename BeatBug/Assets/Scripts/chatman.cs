using UnityEngine;
using static chatmanager;

public class DialogueManager : MonoBehaviour
{
    public chatmanager dialogueUI;
    public bool chatbool = false;
    void Update()
    {
        if (chatbool)
        {
            return;
        }
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
