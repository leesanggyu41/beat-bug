using UnityEngine;

public class Boss : MonoBehaviour
{
    [SerializeField]
    private Animator BossAnim;
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            BossAnim.SetTrigger("Hit");
        }
    }
}
