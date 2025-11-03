using System.Collections;
using UnityEngine;
using TMPro;
using DG.Tweening;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private string Scenename;
    [SerializeField]
    private GameObject player;
    [SerializeField]
    private GameObject animplayer;
    [SerializeField]
    private Animator animator;
    [SerializeField]
    private GameObject DIeBackGround;
    [SerializeField]
    private Transform Dieposition;
    [SerializeField]
    private TextMeshProUGUI gameovertext;
    [SerializeField]
    private GameObject retrytext;
    [SerializeField]
    private AudioSource bgm;

    private bool retry = false;

    private void Update()
    {
        if(retry && Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene(Scenename);
        }
    }


    public void playerDie()
    {
        DIeBackGround.SetActive(true);
        bgm.Stop();
        StartCoroutine(DieAnim());
    }

    IEnumerator DieAnim()
    {

        Debug.Log("움직이는중");
        Vector3 startPosition = player.transform.position;
        float elapsed = 0f;

        while (elapsed < 1)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / 1;
            // SmoothStep로 더 자연스럽게
            t = Mathf.SmoothStep(0f, 1f, t);
            player.transform.position = Vector3.Lerp(startPosition, Dieposition.position, t);
            yield return null;
        }

        player.transform.position = Dieposition.position;

        yield return new WaitForSeconds(1.5f);

        paidin();
    }

    public void paidin()
    {
        
        gameovertext.DOFade(1f, 2f);
        player.SetActive(false);
        animplayer.SetActive(true);
        animator.SetBool("Die", true);

        Invoke("wer", 2f);
    }
    public void wer()
    {
        retrytext.SetActive(true);
        retry = true;
    }
}
