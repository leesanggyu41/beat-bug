using UnityEngine;

public class Boss : MonoBehaviour
{
    [SerializeField]
    private Animator BossAnim;
    [SerializeField]
    private GameObject[] paturns;
    
    public int Hp;
    void Update()
    {
        //if (Input.GetKeyDown(KeyCode.Q))
        //{
        //    
        //}
    }

    public void Hit()
    {
        BossAnim.SetTrigger("Hit");
    }
}
