using System.Collections;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private GameObject player;
    [SerializeField]
    private GameObject DIeBackGround;
    [SerializeField]
    private Transform Dieposition;
    

    public void playerDie()
    {
        DIeBackGround.SetActive(true);
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

    }
}
