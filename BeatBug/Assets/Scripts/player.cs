using UnityEngine;

public class player : MonoBehaviour
{
    [SerializeField]
    private bool isAttack;
    public Boss Boss;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(isAttack && Input.GetKeyDown(KeyCode.Space))
        {
            Boss.Hit();
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if(collision.tag == "AttackZone")
        {
            isAttack = true;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.tag == "AttackZone")
        {
            isAttack = false;
        }
    }
}
