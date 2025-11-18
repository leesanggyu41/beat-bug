using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainStart : MonoBehaviour
{
    [SerializeField]
    private Animator move;
    public void Onstrt()
    {
        move.SetTrigger("go");
    }

    public void OnQuit()
    {
        Application.Quit();
    }

    public void MoveScene()
    {
        SceneManager.LoadScene("LV!");
    }
}
