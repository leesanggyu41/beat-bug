using UnityEngine;

public class Boss : MonoBehaviour
{
    [SerializeField]
    private Animator BossAnim;
    
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
