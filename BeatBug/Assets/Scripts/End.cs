using UnityEngine;
using UnityEngine.SceneManagement;

public class End : MonoBehaviour
{
    public GameObject texts;
    void Start()
    {
        Invoke("opentext", 5);
    }

    void opentext()
    {
        texts.SetActive(true);
    }

    public void gotitle()
    {
        SceneManager.LoadScene("startScene");
    }

}
